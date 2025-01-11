using GD.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GD.State
{
    /// <summary>
    /// Store reference to entities/objects that the conditions need to check against.
    /// </summary>
    public class ConditionContext
    {
        // Used by the conditions to get the current state of the player
        private PlayerInventory player;

        // Used by the conditions to get the current state of the inventory
        private InventoryCollection inventoryCollection;

        // Used by the conditions to get the current state of the game object
        private GameObject gameObject;

        public PlayerInventory Player { get => player; set => player = value; }
        public InventoryCollection InventoryCollection { get => inventoryCollection; set => inventoryCollection = value; }
        public GameObject GameObject { get => gameObject; set => gameObject = value; }

        // Add other context dependencies here

        public ConditionContext(PlayerInventory player, InventoryCollection inventoryCollection, GameObject gameObject)
        {
            Player = player;
            InventoryCollection = inventoryCollection;
            GameObject = gameObject;
        }

        public ConditionContext(PlayerInventory player, InventoryCollection inventoryCollection)
            : this(player, inventoryCollection, null)
        {
        }

        public ConditionContext(PlayerInventory player)
          : this(player, null, null)
        {
        }

        public ConditionContext(InventoryCollection inventoryCollection)
            : this(null, inventoryCollection, null)
        {
        }

        public ConditionContext(GameObject gameObject)
            : this(null, null, gameObject)
        {
        }

        public ConditionContext()
           : this(null, null, null)
        {
        }
    }
}
