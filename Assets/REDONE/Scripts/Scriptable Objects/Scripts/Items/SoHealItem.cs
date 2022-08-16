using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    /// <summary>
    /// This is a healing Item used to heal the player on pickup or other logic determined within.
    /// </summary>
    
    [CreateAssetMenu(fileName = "Heal Item", menuName = "Cow's Compendium/Inventory System/Items/New Heal Item")]
    public class SoHealItem : SoItem
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Variables")][SerializeField] public float amountToHeal;
        
        public void Awake()
        {
            // set item type automatically
            itemType = ItemType.HealingItem;
        }
    }
}