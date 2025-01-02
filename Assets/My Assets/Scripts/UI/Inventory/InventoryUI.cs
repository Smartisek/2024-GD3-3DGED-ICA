using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform PlayerItemSlotContainer;
    [SerializeField] private Transform BinItemSlotContainer;

    private Inventory currentBinInventory; //will access the current bin inventory clicking on 
    public bool IsInventoryOpen { get; private set; }

    private void Start()
    {
        // Hide the inventory panel at the start
        inventoryPanel.SetActive(false);
        IsInventoryOpen = false;
        UpdateInventoryUI();
        
    }

    public void OnInventoryChange()
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
        IsInventoryOpen = !IsInventoryOpen;
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        if (IsInventoryOpen)
        {
            UpdateInventoryUI(); //update the inventory UI when the panel is viewed
        }
        

    }

    public bool IsInventoryPanelActive() //check for in inventory state 
    {
        return inventoryPanel.activeSelf;
    }

    public void SetCurrentBinInventory(Inventory binInventory)
    {
        currentBinInventory = binInventory;
        UpdateInventoryUI();
    }

    public void HandleItemDrop(ItemSlotUI itemSlotUI)
    {
        if(currentBinInventory != null)
        {
            playerInventory.Remove(itemSlotUI.ItemData, itemSlotUI.ItemCount);
            currentBinInventory.Add(itemSlotUI.ItemData, itemSlotUI.ItemCount);
            UpdateInventoryUI();
            currentBinInventory.RaiseOnChange();
        }
    }
}
