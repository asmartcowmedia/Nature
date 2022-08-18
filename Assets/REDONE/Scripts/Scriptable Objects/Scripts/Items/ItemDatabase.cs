using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    /// <summary>
    /// Database for all serialized items created.
    /// </summary>
   
    [CreateAssetMenu(fileName = "New Item Database", menuName = "Cow's Compendium/Inventory System/New Database")]
    public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Database")][SerializeField]public SoItem[] items;

        [FoldoutGroup("Database")][SerializeField] public Dictionary<int, SoItem> GetItem = new Dictionary<int, SoItem>();
        
        public void OnBeforeSerialize()
        {
            // create new dictionary
            GetItem = new Dictionary<int, SoItem>();
        }

        public void OnAfterDeserialize()
        {
            // for each item
            for (var i = 0; i < items.Length; i++)
            {
                // add item to database
                items[i].id = i;
                GetItem.Add(i,items[i]);
            }
        }
    }
}