using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.Mathematics;

namespace CampingTrip
{
    public class StaminaGainTrigger : MonoBehaviour
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Variables")] 
        [Title("Damage")][SerializeField] private float amountToGain;
        [FoldoutGroup("Variables")][SerializeField] private bool isInterval;
        [ShowIf("isInterval")][FoldoutGroup("Variables")][SerializeField] private bool ignoreExitTrigger;
        [ShowIf("isInterval")][FoldoutGroup("Variables")][SerializeField] private bool isContinuous;
        [ShowIf("isInterval")][EnableIf("@this.isInterval && this.isContinuous == false")][FoldoutGroup("Variables")][SerializeField] private float timesToGain;
        [ShowIf("isInterval")][FoldoutGroup("Variables")][SerializeField] private float timeBetweenGain;

        // Default Unity functions //
        private void OnTriggerEnter2D(Collider2D col)
        {
            // if player
            if (col.gameObject.CompareTag("Player"))
            {
                // is interval
                switch (isInterval)
                {
                    // is not continuous
                    case true when !isContinuous:
                        col.gameObject.GetComponent<PlayerStamina>()
                            .Gain(amountToGain, isInterval, timeBetweenGain, timesToGain);
                        break;
                    // is continuous
                    case true when isContinuous:
                        col.gameObject.GetComponent<PlayerStamina>().Gain(amountToGain, isInterval, timeBetweenGain, Mathf.Infinity);
                        break;
                    // else
                    default:
                        col.gameObject.GetComponent<PlayerStamina>().Gain(amountToGain);
                        break;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // if player
            if (other.gameObject.CompareTag("Player"))
            {
                // reset
                other.gameObject.GetComponent<PlayerStamina>().ResetStaminaDrainCounter();
                other.gameObject.GetComponent<PlayerStamina>().ResetStaminaGainCounter();
            }
        }
    }
}