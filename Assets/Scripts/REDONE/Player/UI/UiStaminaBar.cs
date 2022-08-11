using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    public class UiStaminaBar : MonoBehaviour
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Attachable Objects")]
        [Title("In Scene")][SerializeField] private GameObject fillBar;
        [FoldoutGroup("Attachable Objects")][SerializeField] private GameObject staminaBarPanel;
        [FoldoutGroup("Attachable Objects")][SerializeField] private SoStaminaPool staminaStats;
        
        // Default Unity functions //
        private void Update()
        {
            UpdateStaminaBar();
        }

        // Private functions //
        private void UpdateStaminaBar()
        {
            var fillRect = fillBar.gameObject.GetComponent<RectTransform>();
            var hpBarRect = staminaBarPanel.gameObject.GetComponent<RectTransform>();
            
            hpBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, staminaStats.maxStamina);
            fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, staminaStats.currentStamina);

        }
    }
}