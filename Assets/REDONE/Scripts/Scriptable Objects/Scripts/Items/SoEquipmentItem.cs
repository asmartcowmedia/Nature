using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    /// <summary>
    /// This is an equipment item.
    /// </summary>
    
    [CreateAssetMenu(fileName = "Equipment Item", menuName = "Cow's Compendium/Inventory System/Items/New Equipment Item")]
    public class SoEquipmentItem : SoItem
    {
        public void Awake()
        {
            // setup default item type
            itemType = ItemType.Equipment;
        }
    }
}