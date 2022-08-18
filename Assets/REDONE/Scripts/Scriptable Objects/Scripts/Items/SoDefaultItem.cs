using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    /// <summary>
    /// This is an item that is default, meaning it can be pretty much anything.
    /// It is set up so that if we do not know what the item should be, this is what we will use.
    /// DEPRECIATED
    /// </summary>
    
    [CreateAssetMenu(fileName = "Default Item", menuName = "Cow's Compendium/Inventory System/Items/New Default Item")]
    public class SoDefaultItem : SoItem
    {
        public void Awake()
        {
            itemType = ItemType.Default;
        }
    }
}