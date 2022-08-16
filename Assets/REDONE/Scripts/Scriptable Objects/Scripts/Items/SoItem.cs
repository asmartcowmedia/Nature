using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    /// <summary>
    /// The default container for all item types.
    /// </summary>
    
    // enum for item types
    public enum ItemType
    {
        Weapon,
        HealingItem,
        DamagingItem,
        Equipment,
        Default
    }
    
    public abstract class SoItem : ScriptableObject
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Variables")] 
        [Title("Item Data", TitleAlignment = TitleAlignments.Centered)][SerializeField] public GameObject prefab;
        [FoldoutGroup("Variables")][SerializeField] public ItemType itemType;
        [FoldoutGroup("Variables")][TextArea(15,20)][SerializeField] public string description;
    }
}