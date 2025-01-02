using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    #region Variables
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform PlayerItemSlotContainer;
    [SerializeField] private Transform BinItemSlotContainer;

    private Inventory currentBinInventory; //will access the current bin inventory when interacting
    private TrashCan currentTrashCan; //will get reference to the current trash can (this) in trash can script
    public bool IsInventoryOpen { get; private set; } //accessor for other scripts
    #endregion

    private void Start()
    {
        // Hide the inventory panel at the start
        inventoryPanel.SetActive(false);
        IsInventoryOpen = false; //initially false
        UpdateInventoryUI();
        
    }

    #region Inventory UI Methods
    public void OnInventoryChange() //when there is change in inventory update the ui
    {
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        //clear existing slots to avoid duplicates
        foreach (Transform child in PlayerItemSlotContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in BinItemSlotContainer)
        {
            Destroy(child.gameObject);
        }

        //iterate through players inventory items 
        foreach (KeyValuePair<ItemData, int> item in playerInventory)
        {
            GameObject itemSlot = Instantiate(itemSlotPrefab, PlayerItemSlotContainer); //instantiate the item slot prefab with where to put it 
            ItemSlotUI itemSlotUI = itemSlot.GetComponent<ItemSlotUI>(); //get the item slot UI component
            itemSlotUI.SetItem(item.Key, item.Value); //set the item slot UI with the item and how many are in the inventory
            
        }

        // Iterate through bin's inventory items
        if (currentBinInventory != null)
        {
            foreach (KeyValuePair<ItemData, int> item in currentBinInventory)
            {
                GameObject itemSlot = Instantiate(itemSlotPrefab, BinItemSlotContainer); // Instantiate the item slot prefab with where to put it 
                ItemSlotUI itemSlotUI = itemSlot.GetComponent<ItemSlotUI>(); // Get the item slot UI component
                itemSlotUI.SetItem(item.Key, item.Value); // Set the item slot UI with the item and how many are in the inventory
            }
        }

    }

    public void ToggleInventoryPanel()
    {
        Debug.Log("Toggling inventory panel");
        IsInventoryOpen = !IsInventoryOpen; //toggle the inventory state
        inventoryPanel.SetActive(!inventoryPanel.activeSelf); // show the panel 

        if (IsInventoryOpen)
        {
            UpdateInventoryUI(); //update the inventory UI when the panel is viewed
        }
        

    }

    public bool IsInventoryPanelActive() //check for in inventory state for other scripts  
    {
        return inventoryPanel.activeSelf;
    }

    #endregion

    #region Bin Interaction Methods
    public void SetCurrentBinInventory(Inventory binInventory, TrashCan trashCan) //used in trashCan script to set the current bin inventory
    {
        currentBinInventory = binInventory;
        currentTrashCan = trashCan;
        UpdateInventoryUI();
    }

    public void HandleItemDrop(ItemSlotUI itemSlotUI)
    {
        if (currentBinInventory != null && itemSlotUI.ItemData != null) //check for nulls 
        {
            if (currentTrashCan != null && currentTrashCan.CanAcceptItemType(itemSlotUI.ItemData)) //check if the (this) trash can can accept that type of item
            {
                playerInventory.Remove(itemSlotUI.ItemData, itemSlotUI.ItemCount); //take away from player inventory 
                currentBinInventory.Add(itemSlotUI.ItemData, itemSlotUI.ItemCount); //give to trash inventory 
                UpdateInventoryUI();
                currentBinInventory.RaiseOnChange(); //notify subscribers of the change
            }
            else
            {
                Debug.Log("Item type not accepted by this trash can.");
            }
        }
        else
        {
            Debug.Log("ItemData is null or currentBinInventory is null.");
        }
    }
    #endregion
}
