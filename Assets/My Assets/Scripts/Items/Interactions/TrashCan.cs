using GD.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [SerializeField] private InventoryCollection inventoryCollection;
    [SerializeField] private Inventory binInventory;

    private bool isPlayerNearby;

    private void OnMouseDown()
    {
        if (isPlayerNearby)
        {
            PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();

            if (playerInventory != null)
            {
                playerInventory.InteractWithTrashCan(binInventory);
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
