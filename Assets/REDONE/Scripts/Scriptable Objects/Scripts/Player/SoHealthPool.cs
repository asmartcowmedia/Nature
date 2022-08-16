using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    [CreateAssetMenu(menuName = "Cow's Compendium/Character Stats/Health Pool")]
    public class SoHealthPool : ScriptableObject
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Variables")] 
        [Title("Health")][SerializeField] public float maxHealth;
        
        // Not editable in unity inspector, read only, and not editable in other scripts //
        [FoldoutGroup("Debug")]
        [Title("Read Only")][SerializeField][ReadOnly] public float currentHealth;
        [FoldoutGroup("Debug")][SerializeField][ReadOnly] public bool isTakingDamage;
        [FoldoutGroup("Debug")][SerializeField][ReadOnly] public bool isHealing;
        [FoldoutGroup("Debug")][SerializeField][ReadOnly] public float timesDamaged;
        [FoldoutGroup("Debug")][SerializeField][ReadOnly] public float timesHealed;
        [FoldoutGroup("Debug")][SerializeField][ReadOnly] public float howLongToDamageOrHeal;
    }
}