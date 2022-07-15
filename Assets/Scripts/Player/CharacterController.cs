using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;

    [SerializeField] private Transform 
        graphics,
        attackDirection;

    [SerializeField] private Stamina stamina;

    [SerializeField] public float staminaDrain;

    [SerializeField] private Vector3 graphicsScale;

    [SerializeField] private FOV fov;
    
    [SerializeField] private float
        movementSpeed,
        inWaterDrag,
        normalDrag;

    [SerializeField] private Camera cameraReference;

    [SerializeField] private AnimationController animationController;

    [ReadOnly] public bool
        isAttacking;
    
    private Vector2
        _velocity;

    private Vector3
        _mousePosition;

    private void Awake()
    {
        if (rigidBody == null) gameObject.GetComponent<Rigidbody2D>();
        if (animationController == null) gameObject.GetComponent<AnimationController>();
        rigidBody.gravityScale = 0f;
        rigidBody.angularDrag = normalDrag;
        rigidBody.drag = normalDrag;
    }

    private void Update()
    {
        Move();
        MousePosition();
        UpdateGraphicsScale();
        Attack();
    }

    private void UpdateGraphicsScale()
    { 
        if (animationController.isWalkingUp)
        {
            if (_mousePosition.x >= 0.01f) graphics.localScale = new Vector3(-graphicsScale.x, graphicsScale.y, graphicsScale.z);
            else if (_mousePosition.x <= -0.01f) graphics.localScale = new Vector3(graphicsScale.x, graphicsScale.y, graphicsScale.z);
        }
        
        if (animationController.isWalkingRight)
            graphics.localScale = new Vector3(graphicsScale.x, graphicsScale.y, graphicsScale.z);
                
        if (animationController.isWalkingLeft)
            graphics.localScale = new Vector3(-graphicsScale.x, graphicsScale.y, graphicsScale.z);
    }

    private void MousePosition()
    {
        _mousePosition = cameraReference.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraReference.transform.position.z * -1));
        _mousePosition = transform.InverseTransformPoint(_mousePosition);
        
        fov.SetAimDirection(_mousePosition);
        fov.SetOrigin(transform.position);
    }

    private void Move()
    {
        _velocity.x = Input.GetAxis("Horizontal");
        _velocity.y = Input.GetAxis("Vertical");

        _velocity.Normalize();
        _velocity *= 500 * (movementSpeed * Time.deltaTime);

        rigidBody.AddForce(_velocity);
    }

    private void Attack()
    {
        var dir = fov.GetAimDirection(_mousePosition);

        attackDirection.rotation = Quaternion.Euler(0, 0, dir);

        if (Input.GetButtonDown("Fire1"))
        {
            if (stamina.stamina > 0 && stamina.stamina - staminaDrain >= 0)
            {
                isAttacking = true;
                stamina.DrainStamina(staminaDrain);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            SetWaterDrag();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            ResetDrag();
        }
    }

    private void SetWaterDrag()
    {
        rigidBody.drag = inWaterDrag;
        rigidBody.angularDrag = inWaterDrag;
    }

    private void ResetDrag()
    {
        rigidBody.drag = normalDrag;
        rigidBody.angularDrag = normalDrag;
    }
}
