using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    /// <summary>
    /// Can you guess what it is? Its an inventory!
    /// This will be added to player or other objects to give them an inventory.
    /// Very useful!
    /// </summary>
    
    public class Inventory : MonoBehaviour
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Inventory")][SerializeField]public SoInventoryObject inventory;

        private void OnTriggerEnter2D(Collider2D col)
        {
            // if collides with a pickup
            if (col.CompareTag("Pickup"))
            {
                // check if item is on pickup
                var item = col.GetComponent<Item>();

                // return if no item
                if (!item) return;
                
                // add item and destroy object
                inventory.AddItem(item.item, 1);
                Destroy(col.gameObject);
            }
        }

        private void OnApplicationQuit()
        {
            // clears inventory on app quit
            inventory.container.Clear();
        }
    }
}