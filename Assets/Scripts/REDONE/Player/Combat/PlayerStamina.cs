using System;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    public class PlayerStamina : MonoBehaviour
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Attachable Objects")] 
        [Title("Health")][SerializeField] public SoStaminaPool staminaStats;
        
        // Default Unity Functions //
        private void Awake()
        {
            // initiate values
            staminaStats.timesDrainingStamina = 0;
            staminaStats.timesGainingStamina = 0;
        }

        private void Start()
        {
            // reset stamina at beginning
            staminaStats.currentStamina = staminaStats.maxStamina;
        }

        private void LateUpdate()
        {
            StandardStaminaRegen(staminaStats.standardRegenSpeed);
        }

        // IEnumerators //
        private IEnumerator IntervalStaminaDrain(float drainAmount, float time, float timesToDrain)
        {
            // if both draining and gaining, reset variables
            if (staminaStats.isUsingStamina && staminaStats.isGainingStamina)
            {
                ResetStaminaDrainCounter();
                ResetStaminaGainCounter();
            }

            while (staminaStats.isUsingStamina && !staminaStats.isGainingStamina)
            {
                if (staminaStats.currentStamina <= 0)
                {
                    staminaStats.currentStamina = 0;
                    ResetStaminaDrainCounter();
                    yield break;
                }
                
                // debug how long the time is set to when calling the IEnumerator
                staminaStats.howLongToDrainOrGain = time;
            
                // call drain function for drain amount
                Drain(drainAmount);
            
                // well... this waits for a time...
                yield return new WaitForSeconds(time);
            
                // add one to times drained
                staminaStats.timesDrainingStamina++;
            
                // check if times drained
                if (!(staminaStats.timesDrainingStamina >= timesToDrain)) continue;
                staminaStats.isUsingStamina = false;
                staminaStats.timesDrainingStamina = 0;
            }
        }

        private IEnumerator IntervalStaminaGain(float staminaGain, float time, float timesToGain)
        {
            // if both using and gaining
            if (staminaStats.isUsingStamina && staminaStats.isGainingStamina)
            {
                ResetStaminaDrainCounter();
                ResetStaminaGainCounter();
            }

            while (staminaStats.isGainingStamina && !staminaStats.isUsingStamina)
            {
                if (staminaStats.currentStamina >= staminaStats.maxStamina)
                {
                    staminaStats.currentStamina = staminaStats.maxStamina;
                    ResetStaminaGainCounter();
                    yield break;
                }
                
                // gain stamina for amount
                Gain(staminaGain);
                
                // wait for seconds
                yield return new WaitForSeconds(time);
                
                // add one to times gained
                staminaStats.timesGainingStamina++;
                
                // check times gained
                if (!(staminaStats.timesGainingStamina >= timesToGain)) continue;
                staminaStats.isGainingStamina = false;
                staminaStats.timesGainingStamina = 0;
            }
        }

        private IEnumerator WaitThenRegen(float time, float betweenTime)
        {
            // while stamina is less than max
            while (staminaStats.currentStamina < staminaStats.maxStamina && !staminaStats.isUsingStamina)
            {
                // wait for time
                yield return new WaitForSeconds(time);
             
                // if over max, set to max, return
                if (staminaStats.currentStamina >= staminaStats.maxStamina)
                {
                    staminaStats.currentStamina = staminaStats.maxStamina;
                    yield break;
                }
                
                // gain stamina
                Gain(staminaStats.standardRegenAmount, true, betweenTime);
                
                staminaStats.timesGainingStamina++;
                
                // check times gained
                if (!(staminaStats.currentStamina >= staminaStats.maxStamina)) continue;
                staminaStats.isGainingStamina = false;
                staminaStats.timesGainingStamina = 0;
            }
        }

        // Public Functions //
        // resets current stamina gain
        public void ResetStaminaGainCounter()
        {
            staminaStats.isGainingStamina = false;
        }

        // resets current stamina usage
        public void ResetStaminaDrainCounter()
        {
            staminaStats.isUsingStamina = false;
        }
        
        // waits, then regenerates stamina over time
        public void StandardStaminaRegen(float timeToWaitBeforeRegen)
        {
            // if gaining/losing stamina and current stamina is less than max stamina, return
            if (staminaStats.isGainingStamina && staminaStats.currentStamina < staminaStats.maxStamina || staminaStats.isUsingStamina && staminaStats.currentStamina < staminaStats.maxStamina) return;

            // call regen routine
            StartCoroutine(WaitThenRegen(timeToWaitBeforeRegen, staminaStats.standardRegenSpeed));
        }
        
        // Set of functions to drain stamina from the player with various variables (see last on how it works)
        // just drains stamina once for amount given
        public void Drain(float amountToDrain)
        {
            staminaStats.currentStamina -= amountToDrain;
        }
        
        public void Drain(float amountToDrain, bool intervalDrain)
        {
            if (!intervalDrain) return;

            staminaStats.isUsingStamina = true;

            StartCoroutine(IntervalStaminaDrain(amountToDrain, .5f, 1));
        }

        public void Drain(float amountToDrain, bool intervalDrain, float timeBetweenDrains)
        {
            if (!intervalDrain) return;

            staminaStats.isUsingStamina = true;

            StartCoroutine(IntervalStaminaDrain(amountToDrain, timeBetweenDrains, 1));
        }

        public void Drain(float amountToDrain, bool intervalDrain, float timeBetweenDrains, float timesToDrain)
        {
            // if there is no interval drain, return
            if (!intervalDrain) return;
            
            // set draining to true
            staminaStats.isUsingStamina = true;
            
            // start taking interval drains
            StartCoroutine(IntervalStaminaDrain(amountToDrain, timeBetweenDrains, timesToDrain));
        }
        
        // set of functions to gain stamina to the player with various variables (see end to see how it works)
        // just gain stamina once for amount given
        public void Gain(float amountToGain)
        {
            staminaStats.currentStamina += amountToGain;
        }

        public void Gain(float amountToGain, bool intervalGain)
        {
            if (!intervalGain) return;

            staminaStats.isGainingStamina = true;

            StartCoroutine(IntervalStaminaGain(amountToGain, .5f, 1));
        }

        public void Gain(float amountToGain, bool intervalGain, float timeBetweenGains)
        {
            if (!intervalGain) return;

            staminaStats.isGainingStamina = true;

            StartCoroutine(IntervalStaminaGain(amountToGain, timeBetweenGains, 1));
        }

        public void Gain(float amountToGain, bool intervalGain, float timeBetweenGains, float timesToGain)
        {
            // if the interval is false, return
            if (!intervalGain) return;
            
            // set gaining to true
            staminaStats.isGainingStamina = true;
            
            // start gaining stamina
            StartCoroutine(IntervalStaminaGain(amountToGain, timeBetweenGains, timesToGain));
        }
    }
}