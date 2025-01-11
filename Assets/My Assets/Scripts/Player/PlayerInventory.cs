using GD.Items;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    #region Variables
    [SerializeField] private InventoryCollection inventoryCollection; //what invetory collection ?  scriptable object 
    [SerializeField] private Inventory inventory; //for resetting inventory at start of the game
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private Animator animator;
    private ItemInteraction nearbyItem; //interactable items 

    //Pickup notification
    [SerializeField] private GameObject pickupMessageGroup;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private Image itemIcon;
    [SerializeField] private float slideDuration = 1f;

    #endregion

    private void Awake()
    {
        inventoryUI = FindObjectOfType<InventoryUI>();

        pickupMessageGroup.SetActive(false);
    }


    #region Inventory Methods
    // using same logic I coded in our group project: https://github.com/Tomascus/Darkspire
    public void SetNearbyItem(ItemInteraction item)
    {
        nearbyItem = item; //set the nearby item to the item
    }

    public void ClearNearbyItem(ItemInteraction item) //clear because not near
    {
        if (nearbyItem == item)
        {
            nearbyItem = null;
        }
    }

    public void PickUpItem()
    {
        if (nearbyItem != null) //if the nearby item is not null
        {
            if (nearbyItem.ItemData != null) //and its data is not null
            {
                // Cast ItemData to the correct type
                ItemData itemData = nearbyItem.ItemData as ItemData;
                if (itemData != null)
                {
                    try
                    {
                        // Assuming the first inventory in the collection is the player's inventory
                        Inventory playerInventory = inventoryCollection[0];
                        playerInventory.Add(itemData, 1);
                        Debug.Log($"Item added to inventory: {itemData.ItemName}");
                        ShowItemPickupNotification(itemData.ItemIcon, itemData.ItemName);
                        if (animator != null)
                        {
                            animator.SetTrigger("PickUp");
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                }
                else
                {
                    Debug.LogError("ItemData is not of type ItemData.");
                }
            }
            else
            {
                Debug.LogError("ItemData is null.");
            }

            Destroy(nearbyItem.gameObject);
            nearbyItem = null;
        }
    }
    #endregion

    #region PICKUP UI
    public void ShowItemPickupNotification(Sprite icon, string text)
    {
        if (pickupMessageGroup != null && itemIcon != null && itemText != null)
        {
            itemIcon.sprite = icon;
            itemText.text = text;
            itemCount.text = "+1";

            StartCoroutine(DisplayItemPickupNotification());
        }
    }

    private IEnumerator DisplayItemPickupNotification()
    {
        // Show the notification immediately
        pickupMessageGroup.SetActive(true);
        pickupMessageGroup.transform.localPosition = new Vector3(pickupMessageGroup.transform.localPosition.x, 0, pickupMessageGroup.transform.localPosition.z);

        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Slide out downwards
        float elapsedTime = 0f;
        Vector3 startPosition = pickupMessageGroup.transform.localPosition;
        Vector3 endPosition = new Vector3(pickupMessageGroup.transform.localPosition.x, -Screen.height, pickupMessageGroup.transform.localPosition.z);

        while (elapsedTime < slideDuration)
        {
            pickupMessageGroup.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / slideDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        pickupMessageGroup.transform.localPosition = endPosition;

        pickupMessageGroup.SetActive(false);
    }
    #endregion
}