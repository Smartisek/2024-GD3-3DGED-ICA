using GD.Items;
using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    #region Variables
    [SerializeField] private InventoryCollection inventoryCollection; //what invetory collection ?  scriptable object 
    [SerializeField] private Inventory inventory; //for resetting inventory at start of the game
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private Animator animator;
    private ItemInteraction nearbyItem; //interactable items 
    #endregion

    private void Awake()
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
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
}