using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

public class AnimationController : MonoBehaviour
{
      //----------------------------------------//
     // Exposed Variables (Editable in editor) //
    //----------------------------------------//
    [FoldoutGroup("Attachable Objects")][Title("Animators")][SerializeField] private Animator animator;
    [FoldoutGroup("Attachable Objects")][SerializeField] private Animator attacker;

    [FoldoutGroup("Attachable Objects")][Title("Character Controller")][SerializeField] private CharacterController player;

    [FoldoutGroup("Attachable Objects")][Title("GameObjects")][SerializeField] private GameObject attackTrigger;

    [FoldoutGroup("Attachable Objects")][Title("Stamina")][SerializeField] private Stamina stamina;

    [FoldoutGroup("Variables")][Title("Animation Strings")][SerializeField] private string verticalString;
    [FoldoutGroup("Variables")][SerializeField] private string horizontalString;
    [FoldoutGroup("Variables")][SerializeField] private string defaultStateString;
    [FoldoutGroup("Variables")][SerializeField] private string walkUpAnimString;
    [FoldoutGroup("Variables")][SerializeField] private string walkDownAnimString;
    [FoldoutGroup("Variables")][SerializeField] private string walkRightAnimString;
    [FoldoutGroup("Variables")][SerializeField] private string walkLeftAnimString;
    [FoldoutGroup("Variables")][SerializeField] private string idleDownAnimString;
    [FoldoutGroup("Variables")][SerializeField] private string idleUpAnimString;
    [FoldoutGroup("Variables")][SerializeField] private string idleLeftAnimString;
    [FoldoutGroup("Variables")][SerializeField] private string idleRightAnimString;
    [FoldoutGroup("Variables")][SerializeField] private string attackingAnim1String;

    [FoldoutGroup("Public Variables")][Title("Animation Variables")] public float[] attackLength;

    [FoldoutGroup("Public Variables")][Title("Bools")] public bool isWalkingUp;
    [FoldoutGroup("Public Variables")] public bool isWalkingRight;
    [FoldoutGroup("Public Variables")] public bool isWalkingLeft;
    [FoldoutGroup("Public Variables")] public bool isIdle;
    //----------------------------------------//
    
      //------------------------------------------------//
     // Non-Exposed Variables (Not Editable in editor) //
    //------------------------------------------------//
    private string
        animationName,
        currentState,
        currentAttackState;
    //----------------------------------------//
    
      //-----------------------//
     // IEnumerator Functions //
    //-----------------------//
    private IEnumerator EndAttack(float time)
    {
        yield return new WaitForSeconds(time);
        
        attacker.SetBool(attackingAnim1String, false);
        attackTrigger.SetActive(false);
    }
    //----------------------------------------//

      //-------------------------//
     // Default Unity Functions //
    //-------------------------//
    private void Awake()
    {
        //Set default values
        if (animator == null) Debug.Log("Animation controller not selected, please do so in the AnimationController.cs!!");
        if (player == null) Debug.Log("Animation controller not selected, please do so in the CharacterController.cs!!");
    }

    private void Update()
    {
        UpdateAnimStates();
        UpdateAttackStates();
    }
    //----------------------------------------//

      //------------------//
     // Custom Functions //
    //------------------//
    //Function to update the attack states in the animator
    private void UpdateAttackStates()
    {
        //If mouse1 is pressed, check stamina, and set the attack trigger to true so the animation plays and use the end attack ienumerator to set a delay for next attack.
        if (Input.GetButton("Fire1"))
        {
            if (stamina.stamina > 0 && stamina.stamina - (player.staminaDrain-5) >= 0)
            {
                attackTrigger.SetActive(true);
                attacker.SetBool(attackingAnim1String, true);

                StartCoroutine(EndAttack(.4f));
            }
        }
    }

    //Function (state machine) to update animation states for movement
    private void UpdateAnimStates()
    {
        //Set variables for vertical and horizontal movement for animator
        var vertical = Input.GetAxis("Vertical") * 100;
        var horizontal = Input.GetAxis("Horizontal") * 100;
        
        //set the vert and hor floats within the animator
        animator.SetFloat(verticalString, vertical);
        animator.SetFloat(horizontalString, horizontal);
        
        //If the vertical movement is non-existent (no up/down movement) then use left/right walk anims
        if (vertical == 0)
        {
            switch (horizontal)
            {
                case > 0:
                    ChangeAnimationState(walkRightAnimString);
                    isWalkingRight = true;
                    isWalkingLeft = false;
                    break;
                case < 0:
                    ChangeAnimationState(walkLeftAnimString);
                    isWalkingRight = false;
                    isWalkingLeft = true;
                    break;
            }
        }

        //If the vertical movement is there, use up/down walk anims
        switch (vertical)
        {
            case > 0:
                ChangeAnimationState(walkUpAnimString);
                isWalkingUp = true;
                isWalkingRight = false;
                isWalkingLeft = false;
                break;
            case < 0:
                ChangeAnimationState(walkDownAnimString);
                isWalkingUp = false;
                break;
        }

        //If there is either vertical OR horizontal movement, set idle to false
        if (vertical > 0 && horizontal > 0)
            isIdle = false;

        //If there is no movement, set idle to true and us the last directional stimulus to determine idle direction animation
        if (vertical == 0 && horizontal == 0)
        {
            isIdle = true;

            if (!isWalkingLeft && !isWalkingRight)
            {
                if (isWalkingUp)
                    ChangeAnimationState(idleUpAnimString);
                if (!isWalkingUp)
                    ChangeAnimationState(idleDownAnimString);
            }

            if (isWalkingLeft || isWalkingRight)
            {
                if (isWalkingRight)
                    ChangeAnimationState(idleRightAnimString);
                if (!isWalkingRight)
                    ChangeAnimationState(idleLeftAnimString);
            }
        }
    }

    //Function (state machine) to change animation state, just pushes all updates to the animator for the state to change
    private void ChangeAnimationState(string state)
    {
        if (currentState == state) return;
        
        animator.Play(state, 0);

        currentState = state;
    }
}