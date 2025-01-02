using GD.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [SerializeField] private InventoryCollection inventoryCollection;
    [SerializeField] private Inventory binInventory;
    [SerializeField] private InventoryUI inventoryUI;

    private bool isPlayerNearby;

    private void Awake()
    {
        Debug.Log(isPlayerNearby);
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player is nearby and E key pressed");
            if (inventoryUI != null)
            {
                Debug.Log("Setting current bin inventory");
                inventoryUI.SetCurrentBinInventory(binInventory);
                Debug.Log("Toggling inventory panel");
                inventoryUI.ToggleInventoryPanel();
            }
            else
            {
                Debug.LogError("InventoryUI is null");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
