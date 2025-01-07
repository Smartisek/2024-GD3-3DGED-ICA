using GD.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planter : MonoBehaviour
{
    #region Varaibles
    [SerializeField] private InventoryCollection plantInventoryCollection;
    [SerializeField] private Inventory plantingInventory;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private Transform treeSpawnPoint;
    [SerializeField] private GameObject pressIndicator;

    private bool isPlayerNearby;
    private bool treePlanted = false;
    #endregion

    private void Update()
    {
        //interaction which will be moved later
        if (isPlayerNearby && !treePlanted && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player is nearby and E key pressed");
            if (inventoryUI != null)
            {
                Debug.Log("Setting current planting inventory");
                inventoryUI.SetCurrentPlantInventory(plantingInventory, this);
                Debug.Log("Toggling inventory panel");
                inventoryUI.ToggleInventoryPanel();
            }
            else
            {
                Debug.LogError("InventoryUI is null");
            }
        }
    }

    public void PlantTree(ItemData itemData)
    {
        if(itemData.ItemType == ItemType.Seed)
        {
            Instantiate(treePrefab, treeSpawnPoint.position, treeSpawnPoint.rotation);
            Debug.Log("Tree planted");
            treePlanted = true;
            pressIndicator.SetActive(false);
            inventoryUI.ToggleOffInventoryPanel();
            
        }
    }

    #region Collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !treePlanted)
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