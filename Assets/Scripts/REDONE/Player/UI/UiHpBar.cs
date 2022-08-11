using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    public class UiHpBar : MonoBehaviour
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Attachable Objects")]
        [Title("In Scene")][SerializeField] private GameObject fillBar;
        [FoldoutGroup("Attachable Objects")][SerializeField] private GameObject hpBarPanel;
        [FoldoutGroup("Attachable Objects")][SerializeField] private SoHealthPool hpStats;
        
        // Default Unity functions //
        private void Update()
        {
            UpdateHealthBar();
        }

        // Private functions //
        // function to update the health bar
        private void UpdateHealthBar()
        {
            var fillRect = fillBar.gameObject.GetComponent<RectTransform>();
            var hpBarRect = hpBarPanel.gameObject.GetComponent<RectTransform>();
            
            hpBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hpStats.maxHealth);
            fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hpStats.currentHealth);
        }
    }
}