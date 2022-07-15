using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator 
        animator,
        attacker;

    [SerializeField] private CharacterController player;

    [SerializeField] private Stamina stamina;

    [SerializeField] private string
        verticalString,
        horizontalString,
        defaultStateString,
        walkUpAnimString,
        walkDownAnimString,
        walkRightAnimString,
        walkLeftAnimString,
        idleDownAnimString,
        idleUpAnimString,
        idleLeftAnimString,
        idleRightAnimString,
        attackingAnim1String;

    public float[] attackLength;

    public bool 
        isWalkingUp,
        isWalkingRight,
        isWalkingLeft,
        isIdle;

    private string
        animationName,
        currentState,
        currentAttackState;

    private bool resetEnum;

    private IEnumerator EndAttack(float time)
    {
        ChangeAttackState(attackingAnim1String);
        
        yield return new WaitForSeconds(time);

        while (player.isAttacking)
        {
            ChangeAttackState(attackingAnim1String);
        
            yield return new WaitForSeconds(time);
        }
    }

    private void Awake()
    {
        if (animator == null) Debug.Log("Animation controller not selected, please do so in the AnimationController.cs!!");
        if (player == null) Debug.Log("Animation controller not selected, please do so in the CharacterController.cs!!");
    }

    private void FixedUpdate()
    {
        UpdateAnimStates();
        UpdateAttackStates();
    }

    private void UpdateAttackStates()
    {
        if (Input.GetButton("Fire1"))
        {
            if (stamina.stamina > 0 && stamina.stamina - player.staminaDrain >= 0)
            {
                StartCoroutine(EndAttack(attackLength[0]));
            }
        }
    }

    private void UpdateAnimStates()
    {
        var vertical = Input.GetAxis("Vertical") * 100;
        var horizontal = Input.GetAxis("Horizontal") * 100;
        
        animator.SetFloat(verticalString, vertical);
        animator.SetFloat(horizontalString, horizontal);
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

        if (vertical > 0 && horizontal > 0)
            isIdle = false;

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

    private void ChangeAnimationState(string state)
    {
        if (currentState == state) return;
        
        animator.Play(state, 0);

        currentState = state;
    }

    private void ChangeAttackState(string state)
    {
        if (currentAttackState == state) return;

        attacker.PlayInFixedTime(state, 0);

        currentAttackState = state;
    }
}