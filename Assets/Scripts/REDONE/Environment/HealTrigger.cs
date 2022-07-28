using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    public class HealTrigger : MonoBehaviour
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Variables")] 
        [Title("Damage")][SerializeField] private float amountToHeal;
        [FoldoutGroup("Variables")][SerializeField] private bool isInterval;
        [ShowIf("isInterval")][FoldoutGroup("Variables")][SerializeField] private bool ignoreExitTrigger;
        [ShowIf("isInterval")][FoldoutGroup("Variables")][SerializeField] private bool isContinuous;
        [ShowIf("isInterval")][EnableIf("@this.isInterval && this.isContinuous == false")][FoldoutGroup("Variables")][SerializeField] private float timesToHeal;
        [ShowIf("isInterval")][FoldoutGroup("Variables")][SerializeField] private float timeBetweenHeal;
        
        // Default Unity functions //
        private void OnTriggerEnter2D(Collider2D col)
        {
            // if the collider is tagged with player
            if (col.gameObject.CompareTag("Player"))
            {
                // switch statement to determine if isInterval is true, if so, check if continuous is true
                switch (isInterval)
                {
                    // When is only interval, not continuous, apply times to damage
                    case true when !isContinuous:
                        col.gameObject.GetComponent<PlayerHp>()
                            .Heal(amountToHeal, isInterval, timeBetweenHeal, timesToHeal);
                        break;
                    // When is continuous, apply infinite damages
                    case true when isContinuous:
                        col.gameObject.GetComponent<PlayerHp>()
                            .Heal(amountToHeal, isInterval, timeBetweenHeal, Mathf.Infinity);
                        break;
                    // If undetermined, default to standard single damage of amount
                    default:
                        col.gameObject.GetComponent<PlayerHp>().Heal(amountToHeal);
                        break;
                }
            }
        }
        
        private void OnTriggerExit2D(Collider2D other) // on trigger exit... duh
        {
            // if the collider is tagged with player and isContinuous is true and is not ignoring exit triggers
            if (other.gameObject.CompareTag("Player") && isContinuous && !ignoreExitTrigger)
            {
                // reset both health and damage ticks
                other.gameObject.GetComponent<PlayerHp>().ResetHealCounter();
                other.gameObject.GetComponent<PlayerHp>().ResetDamageCounter();
            }
        }
    }
}