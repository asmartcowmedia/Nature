using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Character Stats/Stamina Pool")]
    public class SoStaminaPool : ScriptableObject
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Variables")] 
        [Title("Stamina", TitleAlignment = TitleAlignments.Centered)][SerializeField] public float maxStamina;
        [FoldoutGroup("Variables")][SerializeField] public bool staminaRegen;
        [FoldoutGroup("Variables")][ShowIf("@staminaRegen")][Title("Stamina Regen Settings", TitleAlignment = TitleAlignments.Centered)][SerializeField] public float standardRegenSpeed;
        [FoldoutGroup("Variables")][ShowIf("@staminaRegen")][SerializeField] public float standardRegenAmount;
        [FoldoutGroup("Variables")][ShowIf("@staminaRegen")][SerializeField] public float timeBeforeStandardRegen;
        
        // Not editable in unity inspector, read only, and not editable in other scripts //
        [FoldoutGroup("Debug")]
        [Title("Read Only")][SerializeField][ReadOnly] public float currentStamina;
        [FoldoutGroup("Debug")][SerializeField][ReadOnly] public bool isUsingStamina;
        [FoldoutGroup("Debug")][SerializeField][ReadOnly] public bool isGainingStamina;
        [FoldoutGroup("Debug")][SerializeField][ReadOnly] public float timesDrainingStamina;
        [FoldoutGroup("Debug")][SerializeField][ReadOnly] public float timesGainingStamina;
        [FoldoutGroup("Debug")][SerializeField][ReadOnly] public float howLongToDrainOrGain;
    }
}