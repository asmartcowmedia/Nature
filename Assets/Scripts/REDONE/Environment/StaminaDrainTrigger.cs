using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    public class StaminaDrainTrigger : MonoBehaviour
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Variables")] 
        [Title("Damage")][SerializeField] private float amountToDrain;
        [FoldoutGroup("Variables")][SerializeField] private bool isInterval;
        [ShowIf("isInterval")][FoldoutGroup("Variables")][SerializeField] private bool ignoreExitTrigger;
        [ShowIf("isInterval")][FoldoutGroup("Variables")][SerializeField] private bool isContinuous;
        [ShowIf("isInterval")][EnableIf("@this.isInterval && this.isContinuous == false")][FoldoutGroup("Variables")][SerializeField] private float timesToDrain;
        [ShowIf("isInterval")][FoldoutGroup("Variables")][SerializeField] private float timeBetweenDrains;
        
        // Default Unity Functions //
        private void OnTriggerEnter2D(Collider2D col)
        {
            // if trigger player
            if (col.gameObject.CompareTag("Player"))
            {
                // is interval?
                switch (isInterval)
                {
                    // if not continuous
                    case true when !isContinuous:
                        col.gameObject.GetComponent<PlayerStamina>().Drain(amountToDrain, isInterval, timeBetweenDrains, timesToDrain);
                        break;
                    // if continuous
                    case true when isContinuous:
                        col.gameObject.GetComponent<PlayerStamina>().Drain(amountToDrain, isInterval, timeBetweenDrains, Mathf.Infinity);
                        break;
                    // else
                    default:
                        col.gameObject.GetComponent<PlayerStamina>().Drain(amountToDrain);
                        break;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // if trigger player
            if (other.gameObject.CompareTag("Player") && isContinuous && !ignoreExitTrigger)
            {
                // reset
                other.gameObject.GetComponent<PlayerStamina>().ResetStaminaDrainCounter();
                other.gameObject.GetComponent<PlayerStamina>().ResetStaminaGainCounter();
            }
        }
    }
}