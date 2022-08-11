using System;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

namespace CampingTrip
{
    public class DebuggingPanel : MonoBehaviour
    {
        [SerializeField] private bool showDebug;
        
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Attachable Objects")] 
        [Title("Health")][ShowIf("showDebug")][SerializeField] private SoHealthPool hpStats;
        [FoldoutGroup("Attachable Objects")][Title("Stamina")][ShowIf("showDebug")][SerializeField] private SoStaminaPool staminaStats;
        [FoldoutGroup("Attachable Objects")][Title("Debug")][ShowIf("showDebug")][SerializeField] private TextMeshProUGUI debugString;
        
        // DEBUG VARIABLES //
        [FoldoutGroup("Debug")]
        [Title("Health")][ShowIf("showDebug")][SerializeField][ReadOnly] private float currentHp;
        [FoldoutGroup("Debug")][ShowIf("showDebug")][SerializeField][ReadOnly] private bool isCurrentlyTakingDamage;
        [FoldoutGroup("Debug")][ShowIf("showDebug")][SerializeField][ReadOnly] private bool isCurrentlyHealing;
        [FoldoutGroup("Debug")][ShowIf("showDebug")][SerializeField][ReadOnly] private float amountOfTimesDamaged;
        [FoldoutGroup("Debug")][ShowIf("showDebug")][SerializeField][ReadOnly] private float amountOfTimesHealed;
        [FoldoutGroup("Debug")][ShowIf("showDebug")][SerializeField][ReadOnly] private float damageOrHealDuration;
        [Title("Stamina")]
        [FoldoutGroup("Debug")][ShowIf("showDebug")][SerializeField][ReadOnly] public float currentStamina;
        [FoldoutGroup("Debug")][ShowIf("showDebug")][SerializeField][ReadOnly] public bool isUsingStamina;
        [FoldoutGroup("Debug")][ShowIf("showDebug")][SerializeField][ReadOnly] public bool isGainingStamina;
        [FoldoutGroup("Debug")][ShowIf("showDebug")][SerializeField][ReadOnly] public float timesDrainingStamina;
        [FoldoutGroup("Debug")][ShowIf("showDebug")][SerializeField][ReadOnly] public float timesGainingStamina;
        [FoldoutGroup("Debug")][ShowIf("showDebug")][SerializeField][ReadOnly] public float howLongToDrainOrGain;

        private void Update()
        {
            UpdateDebug();
        }

        private void UpdateDebug()
        {
            switch (showDebug)
            {
                case false:
                    gameObject.SetActive(false);
                    break;
                
                case true:
                    UpdateUiDebugPanel();

                    gameObject.SetActive(true);
            
                    currentHp = hpStats.currentHealth;
                    isCurrentlyTakingDamage = hpStats.isTakingDamage;
                    isCurrentlyHealing = hpStats.isHealing;
                    amountOfTimesDamaged = hpStats.timesDamaged;
                    amountOfTimesHealed = hpStats.timesHealed;
                    damageOrHealDuration = hpStats.howLongToDamageOrHeal;

                    currentStamina = staminaStats.currentStamina;
                    isGainingStamina = staminaStats.isGainingStamina;
                    isUsingStamina = staminaStats.isUsingStamina;
                    timesDrainingStamina = staminaStats.timesDrainingStamina;
                    timesGainingStamina = staminaStats.timesGainingStamina;
                    howLongToDrainOrGain = staminaStats.howLongToDrainOrGain;
                    break;
            }
        }

        private void UpdateUiDebugPanel()
        {
            debugString.text =
                "Health: " + hpStats.currentHealth.ToString() + "\n"
                + "Currently taking damage? =" + isCurrentlyTakingDamage + "\n" +
                "Times taken damage: " + amountOfTimesDamaged + "\n" +
                "Currently healing? =" + isCurrentlyHealing + "\n" +
                "Times healed: " + amountOfTimesHealed + "\n" +
                "Duration to heal/damage: " + damageOrHealDuration + "\n" + "\n" +
                "Stamina: " + staminaStats.currentStamina + "\n" +
                "Currently gaining stamina? =" + staminaStats.isGainingStamina + "\n" +
                "Times gained stamina: " + staminaStats.timesGainingStamina + "\n" +
                "Currently draining stamina? =" + staminaStats.isUsingStamina + "\n" +
                "Times drained stamina: " + staminaStats.timesDrainingStamina + "\n" +
                "Duration to drain/gain stamina: " + staminaStats.howLongToDrainOrGain + "\n";
        }
    }
}