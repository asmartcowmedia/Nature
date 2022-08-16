using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    /// <summary>
    /// This is a scriptable object set up for inventory usage.
    /// This allows for multiple inventory types and settings!
    /// </summary>
    
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Cow's Compendium/Inventory System/New Inventory")]
    public class SoInventoryObject : ScriptableObject, ISerializationCallbackReceiver
    {
        // Variables //
        [FoldoutGroup("Variables")] [SerializeField] public ItemDatabase database;
        [FoldoutGroup("Variables")][SerializeField] public List<InventorySlot> container = new List<InventorySlot>(); // Actual inventory list

        // adds an item to the inventory list
        public void AddItem(SoItem _item, int _amount)
        {
            // iterates through the list to see if the item is on it
            for (var i = 0; i < container.Count; i++)
            {
                // if the item is on it
                if (container[i].item == _item)
                {
                    // add another item to the list and return
                    container[i].AddAmount(_amount);
                    return;
                }
            } 
            
            // adds item to container if not returned
            container.Add(new InventorySlot(database.GetId[_item], _item, _amount));
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            for (var i = 0; i < container.Count; i++) 
                container[i].item = database.GetItem[container[i].id];
        }
    }

    // serializable class for each item/slot
    [System.Serializable]
    public class InventorySlot
    {
        // variables //
        public int id;
        public SoItem item;
        public int amount;

        // publicly accessible inventory slot
        public InventorySlot(int _id, SoItem _item, int _amount)
        {
            // sets the item and amount to pass
            id = _id;
            item = _item;
            amount = _amount;
        }
        
        
        // adds item
        public void AddAmount(int value)
        {
            // add value to amount of items
            amount += value;
        }
    }
}