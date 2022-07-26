using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CampingTrip
{
    public class PlayerAnimations : MonoBehaviour
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Attachable Objects")]
        [Title("In Scene")][SerializeField] private GameObject attackHitBoxTriggerCollider;
        
        [FoldoutGroup("Attachable Objects")]
        [Title("In Components")][SerializeField] private Animator attackAnimator;
        
        // Not editable in unity inspector and not editable in other scripts //
        private PlayerControls inputSystem;
        private InputAction attack;
        
        // Ienumerators //
        private IEnumerator EndAttack(float time) // Wait and end animation
        {
            yield return new WaitForSeconds(time);
        
            attackAnimator.SetBool("Attack1", false);
            attackHitBoxTriggerCollider.SetActive(false);
        }
    
        // Default Unity functions //
        private void Awake() // Function called as soon as the object is instantiated, but before the Start function
        {
            inputSystem = new PlayerControls(); // Instantiates the "inputSystem" Variable with the actual player controls scheme
            
            // Set default variables if null
            if (!attackAnimator)
                Debug.Log("Attack animator not set! Please set the attack animator in the PlayerAnimations script component panel");
        }

        private void OnEnable() // Called when the object is active in the scene
        {
            // Set all the variables for the input system to that of their counterparts in the input scheme
            attack = inputSystem.Player.Fire;
        
            // Enable all input systems when activating object
            attack.Enable();
        }

        private void Update() // Called every frame
        {
            // Call functions
            Attack();
        }

        private void OnDisable() // Called when the object is disabled in the scene
        {
            // Disable input system on disabling the object
            attack.Disable();
        }
        
        // Private functions //
        private void Attack() // Function to play attack animation when attacking
        {
            // If attack was pressed, play the animation and start a coroutine to wait for seconds while the animation plays
            if (attack.WasPressedThisFrame())
            {
                attackHitBoxTriggerCollider.SetActive(true);
                attackAnimator.SetBool("Attack1", true);

                StartCoroutine(EndAttack(.4f));
            }
        }
    }
}