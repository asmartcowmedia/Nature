using System.Collections;
using System.Collections.Generic;
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
        [FoldoutGroup("Attachable Objects")][SerializeField] private Camera cameraReference;
        [FoldoutGroup("Attachable Objects")][SerializeField] private SpriteRenderer spriteRenderer;
        
        [FoldoutGroup("Attachable Objects")]
        [Title("In Components")][SerializeField] private Animator attackAnimator;
        [FoldoutGroup("Attachable Objects")][SerializeField] private Animator playerAnimator;
        
        [FoldoutGroup("Animation Strings")]
        [Title("Attacks")][SerializeField] private List<string> anim;
        
        [FoldoutGroup("Debug")]
        [Title("ReadOnly")][SerializeField][ReadOnly] private string currentAnimation;
        [FoldoutGroup("Debug")][SerializeField][ReadOnly] public Vector2 movementDirection; // this variable is editable from anywhere, it is public

        [FoldoutGroup("Debug")][Title("Bools")][SerializeField][ReadOnly]private bool isFacingUp;
        
        // Not editable in unity inspector and not editable in other scripts //
        private PlayerControls inputSystem;
        private InputAction 
            attack,
            mouseInputPosition;
        
        // IEnumerators //
        private IEnumerator EndAttack(float time) // Wait and end animation
        {
            // Wait for provided seconds
            yield return new WaitForSeconds(time);
        
            // Set the animation to false as well as the hit box trigger collider
            attackAnimator.SetBool(anim[0], false);
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
            mouseInputPosition = inputSystem.Player.MousePosition;
        
            // Enable all input systems when activating object
            attack.Enable();
            mouseInputPosition.Enable();
        }

        private void Update() // Called every frame
        {
            // Call functions
            Attack();
            Movement();
            GetMouseDirection();
        }

        private void OnDisable() // Called when the object is disabled in the scene
        {
            // Disable input system on disabling the object
            attack.Disable();
            mouseInputPosition.Disable();
        }
        
        // Private functions //
        private void Attack() // Function to play attack animation when attacking
        {
            // If attack was pressed, play the animation and start a coroutine to wait for seconds while the animation plays
            if (!attack.WasPressedThisFrame() || attackAnimator.GetBool(anim[0])) return;
            
            attackHitBoxTriggerCollider.SetActive(true);
            attackAnimator.SetBool(anim[0], true);

            // Calls the coroutine of EndAttack to end the attack animation
            StartCoroutine(EndAttack(.4f));
        }

        private void Movement()
        {
            // grab the movement vectors to change animation states
            var horizontalMovement = movementDirection.x;
            var verticalMovement = movementDirection.y;

            // if there is no vertical or horizontal movement, swap between upward or downward idle animations
            if (verticalMovement == 0 && horizontalMovement == 0)
            {
                if (isFacingUp)
                {
                    ChangeAnimationState(anim[2]);
                }
                if (!isFacingUp)
                {
                    ChangeAnimationState(anim[3]);
                }
            }
            
            // if vertical movement is non-existent, swap between animations of walking horizontally
            if (verticalMovement == 0 && horizontalMovement != 0)
            {
                // swap between animations on horizontal movement
                switch (horizontalMovement)
                {
                    case > 0:
                        ChangeAnimationState(anim[9]);
                        isFacingUp = false;
                        break;
                    case < 0:
                        ChangeAnimationState(anim[8]);
                        isFacingUp = false;
                        break;
                }
            }

            // if horizontal movement is non-existent, swap between animations of walking vertically
            if (horizontalMovement == 0 && verticalMovement != 0)
            {
                // swap between animations on vertical movement
                switch (verticalMovement)
                {
                    case > 0:
                        ChangeAnimationState(anim[6]);
                        isFacingUp = true;
                        break;
                    case < 0:
                        ChangeAnimationState(anim[7]);
                        isFacingUp = false;
                        break;
                }
            }
        }

        private void GetMouseDirection()
        {
            // Get the mouse direction from a personal script made with the function to get the mouse position
            var calculatedMousePosition = mouseInputPosition.ReadValue<Vector2>();
            var mouse = CowsCompendium.GetMousePosition(cameraReference, calculatedMousePosition, transform);

            spriteRenderer.flipX = mouse.x switch
            {
                // fip the sprite toward the mouse position IF the player is not moving upwards
                < 0 when movementDirection.y <= 0 => true,
                > 0 when movementDirection.y <= 0 => false,
                _ => spriteRenderer.flipX
            };
        }

        private void ChangeAnimationState(string animationBeingPassed) // function to change the current animation state to the passed animation string
        {
            // if the current animation matches the animation string being passed, return and do not complete function
            if (currentAnimation == animationBeingPassed) return;
            
            // play the passed animation via string call
            playerAnimator.Play(animationBeingPassed, 0);

            // place the animation being played into the current animation so we can see what is happening in debug
            currentAnimation = animationBeingPassed;
        }
    }
}