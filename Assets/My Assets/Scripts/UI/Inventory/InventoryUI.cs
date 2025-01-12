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
    [SerializeField] private TextMeshProUGUI binType;
    [SerializeField] private TextMeshProUGUI plantingSpot;
    [SerializeField] private UI ui;

    private Inventory currentBinInventory; //will access the current bin inventory when interacting
    private TrashCan currentTrashCan; //will get reference to the current trash can (this) in trash can script
    private Planter currentPlantingSpot;
    public bool IsInventoryOpen { get; private set; } //accessor for other scripts
    #endregion

    private void Start()
    {
        ui = FindObjectOfType<UI>(); //find the UI script

        // Hide the inventory panel at the start
        inventoryPanel.SetActive(false);
        IsInventoryOpen = false; //initially false
        TrashCounter.ResetRecycledItems(); //reset the trash counter

        UpdateInventoryUI();
        
    }

    #region Inventory UI Methods
    public void OnInventoryChange() //when there is change in inventory update the ui
    {
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI() //to have up to date inventory 
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

    public void ToggleInventoryPanel() //show the inventory panel
    {
        Debug.Log("Toggling inventory panel");
        IsInventoryOpen = !IsInventoryOpen; //toggle the inventory state
        inventoryPanel.SetActive(!inventoryPanel.activeSelf); // show the panel 
        binType.text = currentTrashCan != null ? currentTrashCan.name : currentPlantingSpot.name;

        if (IsInventoryOpen)
        {
            UpdateInventoryUI(); //update the inventory UI when the panel is viewed
        }
    }

    public void ToggleOffInventoryPanel()
    {
        inventoryPanel.SetActive(false);
        IsInventoryOpen = false;
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

    public void SetCurrentPlantInventory(Inventory binInventory, Planter plantingSpot) //used in planting spot script to set the current bin inventory
    {
        currentBinInventory = binInventory;
        currentTrashCan = null;
        currentPlantingSpot = plantingSpot;
        UpdateInventoryUI();
    }

    public void HandleItemDrop(ItemSlotUI itemSlotUI)
    {
        if (currentBinInventory != null && itemSlotUI.ItemData != null) //check for nulls 
        {
            if (currentTrashCan != null && currentTrashCan.CanAcceptItemType(itemSlotUI.ItemData)) //check if the (this) trash can can accept that type of item
            {
                playerInventory.Remove(itemSlotUI.ItemData, 1); //take away from player inventory 
                currentBinInventory.Add(itemSlotUI.ItemData, 1); //give to trash inventory 
                TrashCounter.IncrementRecycledItems(); //increment the trash counter
                UpdateInventoryUI();
                currentBinInventory.RaiseOnChange(); //notify subscribers of the change
                
                SoundManager.PlaySound("CORRECT", 1);
            } else if (currentPlantingSpot != null && itemSlotUI.ItemData.ItemType == ItemType.Seed) //check if the item is a seed for planting spot
            {
                playerInventory.Remove(itemSlotUI.ItemData, 1); //take away from player inventory 
                currentBinInventory.Add(itemSlotUI.ItemData, 1); //give to planting inventory 
                currentPlantingSpot.PlantTree(itemSlotUI.ItemData); //plant the tree
                UpdateInventoryUI();
                currentBinInventory.RaiseOnChange(); //notify subscribers of the change
                SoundManager.PlaySound("CORRECT", 1);
            }
            else
            {
                SoundManager.PlaySound("WRONG", 1);
                ui.WrongBin();
            }
        }
        else
        {
            Debug.Log("ItemData is null or currentBinInventory is null.");
        }
    }
    #endregion
}
