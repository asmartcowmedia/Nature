using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    public class PlayerHp : MonoBehaviour
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Variables")] 
        [Title("Health")][SerializeField] private float maxHealth;
        
        // Not editable in unity inspector, read only, and not editable in other scripts //
        [FoldoutGroup("Debug")]
        [Title("Read Only")][SerializeField][ReadOnly] private float currentHealth;
        [FoldoutGroup("Debug")][SerializeField][ReadOnly] private bool isTakingDamage;
        [FoldoutGroup("Debug")][SerializeField][ReadOnly] private bool isHealing;
        [FoldoutGroup("Debug")][SerializeField][ReadOnly] private float timesDamaged;
        [FoldoutGroup("Debug")][SerializeField][ReadOnly] private float timesHealed;
        [FoldoutGroup("Debug")][SerializeField][ReadOnly] private float howLongToDamagePlayer;

        // IEnumerators //
        private IEnumerator IntervalDamage(float damage, float time, float timesToDamage)
        {
            // if both taking damage and healing, reset variables
            if (isTakingDamage && isHealing)
            {
                ResetDamageCounter();
                ResetHealCounter();
            }

            // while isTakingDamage is true
            while (isTakingDamage && !isHealing)
            {
                if (currentHealth <= 0)
                {
                    currentHealth = 0;
                    ResetDamageCounter();
                    yield break;
                }
                    
                // debug how long the time is set to when calling IEnumerator
                howLongToDamagePlayer = time;
                
                // Call damage function to damage player for amount
                Damage(damage);

                // wait for seconds... duh
                yield return new WaitForSeconds(time);

                // add one to times damaged
                timesDamaged++;

                // check if times damaged is greater than or equal to the times to damage the player and set false if it is
                if (!(timesDamaged >= timesToDamage)) continue;
                isTakingDamage = false;
                timesDamaged = 0;
            }
        }
        
        private IEnumerator IntervalHealing(float heal, float time, float timesToHeal)
        {
            // if both taking damage and healing, reset variables
            if (isTakingDamage && isHealing)
            {
                ResetDamageCounter();
                ResetHealCounter();
            }
            
            // while isHealing is true
            while (isHealing && !isTakingDamage)
            {
                if (currentHealth >= maxHealth)
                {
                    currentHealth = maxHealth;
                    ResetHealCounter();
                    yield break;
                }
                
                // heal the player for amount
                Heal(heal);

                // wait for seconds... duh
                yield return new WaitForSeconds(time);

                // add one to times healed
                timesHealed++;

                // check if times healed is greater than or equal to times to heal the player and set false if it is
                if (!(timesHealed >= timesToHeal)) continue;
                isHealing = false;
                timesHealed = 0;
            }
        }
        
        // Default Unity functions //
        private void Awake()
        {
            // initialize values
            timesHealed = 0;
            timesDamaged = 0;
        }

        private void Start()
        {
            currentHealth = maxHealth;
        }

        // Public functions : Callable from other scripts or functions //
        // Kills Player outright, might also play animation, or not, idk yet.
        public void KillPlayer()
        {
            Destroy(gameObject);
        }

        // public function to get is healing from other scripts
        public bool GetIsHealing()
        {
            return isHealing;
        }

        // public function to get is healing from other scripts
        public bool GetIsTakingDamage()
        {
            return isTakingDamage;
        }
        
        // Resets current health bool to false, stopping IEnumerator
        public void ResetHealCounter()
        {
            isHealing = false;
        }

        // Resets current damage bool to false, stopping IEnumerator
        public void ResetDamageCounter()
        {
            isTakingDamage = false;
        }
        
        // - Set of functions to damage the player with various variables (see last to see how it works)
        public void Damage(float amountToDamage)
        {
            // straight up damage the player for amount, no more, no less
            currentHealth -= amountToDamage;
        }
        
        public void Damage(float amountToDamage, bool intervalDamage) // (see last to see how it works)
        {
            if (!intervalDamage) return;
            
            isTakingDamage = true;

            StartCoroutine(IntervalDamage(amountToDamage, .5f, 1));
        }
        
        public void Damage(float amountToDamage, bool intervalDamage, float timeBetweenDamage) // (see last to see how it works)
        {
            if (!intervalDamage) return;
            
            isTakingDamage = true;
            
            StartCoroutine(IntervalDamage(amountToDamage, timeBetweenDamage, 1));
        }
        
        public void Damage(float amountToDamage, bool intervalDamage, float timeBetweenDamage, float timesToDamage)
        {
            // If there is not interval damage to be had, return
            if (!intervalDamage) return;
            
            // set isTakingDamage to true, therefore fulfilling the while loop inside the coroutine
            isTakingDamage = true;
            
            // start taking interval damage
            StartCoroutine(IntervalDamage(amountToDamage, timeBetweenDamage, timesToDamage));
        }
        //-//
        
        // - Set of functions to heal the player with various variables (see last to see how it works)
        public void Heal(float amountToHeal)
        {
            // straight up heal the player for amount, no more, no less
            currentHealth += amountToHeal;
        }

        public void Heal(float amountToHeal, bool intervalHeal) // (see last to see how it works)
        {
            if (!intervalHeal) return;

            isHealing = true;
            
            StartCoroutine(IntervalHealing(amountToHeal, .5f, 1)); // (see last to see how it works)
        }

        public void Heal(float amountToHeal, bool intervalHeal, float timeBetweenHeal) // (see last to see how it works)
        {
            if (!intervalHeal) return;

            isHealing = true;
            
            StartCoroutine(IntervalHealing(amountToHeal, timeBetweenHeal, 1));
        }

        public void Heal(float amountToHeal, bool intervalHeal, float timeBetweenHeal, float timesToHeal)
        {
            // if the intervalHeal is false, return
            if (!intervalHeal) return;

            // set isHealing to true, therefore fulfilling the coroutine while loop
            isHealing = true;
            
            // start healing in intervals
            StartCoroutine(IntervalHealing(amountToHeal, timeBetweenHeal, timesToHeal));
        }
        //-//
    }
}