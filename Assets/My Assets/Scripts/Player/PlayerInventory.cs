using GD.Items;
using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private InventoryCollection inventoryCollection; //what invetory collection ?  scriptable object 
    [SerializeField] private Inventory inventory; //for resetting inventory at start of the game
    [SerializeField] private InventoryUI inventoryUI;
    private ItemInteraction nearbyItem; //interactable items 

    private void Awake()
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
    }

    public void SetNearbyItem(ItemInteraction item)
    {
        nearbyItem = item;
    }

    public void ClearNearbyItem(ItemInteraction item)
    {
        if (nearbyItem == item)
        {
            nearbyItem = null;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            PickUpItem();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.ToggleInventoryPanel();
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

    //public void InteractWithTrashCan(Inventory binInventory)
    //{
    //    Debug.Log("InteractWithTrashCan called");
    //    if (inventoryUI != null)
    //    {
    //        Debug.Log("Setting current bin inventory");
    //        inventoryUI.SetCurrentBinInventory(binInventory);
    //        Debug.Log("Toggling inventory panel");
    //        inventoryUI.ToggleInventoryPanel();
    //    }
    //    else
    //    {
    //        Debug.LogError("InventoryUI is null");
    //    }
    //}

}