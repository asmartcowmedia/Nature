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

        [FoldoutGroup("Database")][SerializeField] public Dictionary<SoItem, int> GetId = new Dictionary<SoItem, int>();
        [FoldoutGroup("Database")][SerializeField] public Dictionary<int, SoItem> GetItem = new Dictionary<int, SoItem>();
        
        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            // create new dictionary
            GetId = new Dictionary<SoItem, int>();
            GetItem = new Dictionary<int, SoItem>();

            // for each item
            for (var i = 0; i < items.Length; i++)
            {
                // add item to database
                GetId.Add(items[i], i);
                GetItem.Add(i,items[i]);
            }
        }
    }
}