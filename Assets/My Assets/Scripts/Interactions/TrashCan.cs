using GD.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    #region Variables
    [SerializeField] private InventoryCollection inventoryCollection;
    [SerializeField] private Inventory binInventory;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private ItemType acceptedItems;
    [SerializeField] private GameObject pressIndicator;

    private bool isPlayerNearby;
    #endregion


    private void Update()
    {
        //interaction which will be moved later
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player is nearby and E key pressed");
            if (inventoryUI != null)
            {
                Debug.Log("Setting current bin inventory");
                inventoryUI.SetCurrentBinInventory(binInventory, this);
                Debug.Log("Toggling inventory panel");
                inventoryUI.ToggleInventoryPanel();
            }
            else
            {
                Debug.LogError("InventoryUI is null");
            }
        }
    }


    public bool CanAcceptItemType(ItemData itemData) //check if type is correct
    {
        return itemData.ItemType == acceptedItems; //Item passing in must match this can's type, used in inventoryUI script
    }

    #region Collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            pressIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            pressIndicator.SetActive(false);
        }
    }
    #endregion
}
