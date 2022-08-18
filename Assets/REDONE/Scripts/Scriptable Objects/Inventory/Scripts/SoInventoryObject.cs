using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;

namespace CampingTrip
{
    /// <summary>
    /// This is a scriptable object set up for inventory usage.
    /// This allows for multiple inventory types and settings!
    /// </summary>
    
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Cow's Compendium/Inventory System/New Inventory")]
    public class SoInventoryObject : ScriptableObject
    {
        // Variables //
        [FoldoutGroup("Variables")][SerializeField] public string savePath;
        [FoldoutGroup("Variables")][SerializeField] public ItemDatabase database;
        [FoldoutGroup("Variables")][SerializeField] public InventoryItems container; // Actual inventory list

        // adds an item to the inventory list
        public void AddItem(Item _item, int _amount)
        {
            // if has buffs
            if (_item.buffs.Length > 0)
            {
                // adds item to container
                SetEmptySlot(_item, _amount);
                return;
            }
            
            // iterates through the list to see if the item is on it
            for (var i = 0; i < container.items.Length; i++)
            {
                // if the item is on it
                if (container.items[i].id == _item.id)
                {
                    // add another item to the list and return
                    container.items[i].AddAmount(_amount);
                    return;
                }
            } 
            
            // adds item to container if not returned
            SetEmptySlot(_item, _amount);
        }

        public InventorySlot SetEmptySlot(Item _item, int _amount)
        {
            for (var i = 0; i < container.items.Length; i++)
            {
                if (container.items[i].id <= -1)
                {
                    container.items[i].UpdateSlot(_item.id, _item, _amount);
                    return container.items[i];
                }
            }

            // if full inv logic
            return null;
        }

        public void MoveItem(InventorySlot item1, InventorySlot item2)
        {
            var temp = new InventorySlot(item2.id, item2.item, item2.amount);
            
            item2.UpdateSlot(item1.id, item1.item, item1.amount);
            item1.UpdateSlot(temp.id, temp.item, temp.amount);
        }

        public void RemoveItem(Item _item)
        {
            for (var i = 0; i < container.items.Length; i++)
            {
                if (container.items[i].item == _item)
                {
                    container.items[i].UpdateSlot(-1, null, 0);
                }
            }
        }

        [ContextMenu("Save")]
        public void Save()
        {
            // create a json, binary formatter, and file to save
            var saveData = JsonUtility.ToJson(this, true);
            var bf = new BinaryFormatter();
            var file = File.Create(Application.persistentDataPath + savePath);
            
            // serialize the data and close
            bf.Serialize(file, saveData);
            file.Close();
        }

        [ContextMenu("Load")]
        public void Load()
        {
            // if the file exists
            if (File.Exists(Application.persistentDataPath + savePath))
            {
                // create formatter, file opener,
                var bf = new BinaryFormatter();
                var file = File.Open(Application.persistentDataPath + savePath, FileMode.Open);
                
                // deserialize and paste info to the SO and close
                JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);

                file.Close();
            }
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            container = new InventoryItems();
        }
    }

    [Serializable]
    public class InventoryItems
    {
        [Title("Inventory Items", TitleAlignment = TitleAlignments.Centered)][SerializeField] public InventorySlot[] items = new InventorySlot[24]; // Actual inventory
    }
    
    // serializable class for each item/slot
    [Serializable]
    public class InventorySlot
    {
        // variables //
        [Title("Item Information")]
        [SerializeField] public int id;
        [SerializeField] public int amount;
        [SerializeField] public Item item;

        // publicly accessible inventory slot
        public InventorySlot()
        {
            // sets the item and amount to pass
            id = -1;
            item = null;
            amount = 0;
        }
        
        // publicly accessible inventory slot
        public InventorySlot(int _id, Item _item, int _amount)
        {
            // sets the item and amount to pass
            id = _id;
            item = _item;
            amount = _amount;
        }
        
        public void UpdateSlot(int _id, Item _item, int _amount)
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