using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    public class DamageTrigger : MonoBehaviour
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Variables")] 
        [Title("Damage")][SerializeField] private float amountToDamage;
        [FoldoutGroup("Variables")][SerializeField] private bool isInterval;
        [ShowIf("isInterval")][FoldoutGroup("Variables")][SerializeField] private bool ignoreExitTrigger;
        [ShowIf("isInterval")][FoldoutGroup("Variables")][SerializeField] private bool isContinuous;
        [ShowIf("isInterval")][EnableIf("@this.isInterval && this.isContinuous == false")][FoldoutGroup("Variables")][SerializeField] private float timesToDamage;
        [ShowIf("isInterval")][FoldoutGroup("Variables")][SerializeField] private float timeBetweenDamages;
        
        // Default Unity functions //
        private void OnTriggerEnter2D(Collider2D col) // on trigger enter... duh
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
                            .Damage(amountToDamage, isInterval, timeBetweenDamages, timesToDamage);
                        break;
                    // When is continuous, apply infinite damages
                    case true when isContinuous:
                        col.gameObject.GetComponent<PlayerHp>()
                            .Damage(amountToDamage, isInterval, timeBetweenDamages, Mathf.Infinity);
                        break;
                    // If undetermined, default to standard single damage of amount
                    default:
                        col.gameObject.GetComponent<PlayerHp>().Damage(amountToDamage);
                        break;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other) // on trigger exit... duh
        {
            // if the collider is tagged with player and isContinuous is true and is not ignoring the exit trigger
            if (other.gameObject.CompareTag("Player") && isContinuous && !ignoreExitTrigger)
            {
                // reset both health and damage ticks
                other.gameObject.GetComponent<PlayerHp>().ResetHealCounter();
                other.gameObject.GetComponent<PlayerHp>().ResetDamageCounter();
            }
        }
    }
}