using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    /// <summary>
    /// This is a weapon item.
    /// </summary>
    
    [CreateAssetMenu(fileName = "Weapon Item", menuName = "Cow's Compendium/Inventory System/Items/New Weapon Item")]
    public class SoWeaponItem : SoItem
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Variables")][SerializeField] public float amountToDamage;
        [FoldoutGroup("Variables")][SerializeField] public float amountOfStaminaToDrain;

        public void Awake()
        {
            // setup default item type
            itemType = ItemType.Weapon;
        }
    }
}