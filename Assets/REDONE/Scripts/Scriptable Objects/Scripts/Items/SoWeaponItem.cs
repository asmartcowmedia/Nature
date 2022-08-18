using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    /// <summary>
    /// This is a weapon item.
    /// DEPRECIATED
    /// </summary>
    
    [CreateAssetMenu(fileName = "Weapon Item", menuName = "Cow's Compendium/Inventory System/Items/New Weapon Item")]
    public class SoWeaponItem : SoItem
    {
        public void Awake()
        {
            // setup default item type
            itemType = ItemType.Weapon;
        }
    }
}