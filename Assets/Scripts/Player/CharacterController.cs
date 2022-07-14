using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;

    [SerializeField] private Transform graphics;

    [SerializeField] private Vector3 graphicsScale;

    [SerializeField] private FOV fov;
    
    [SerializeField] private float
        movementSpeed,
        inWaterDrag,
        normalDrag;

    [SerializeField] private Camera cameraReference;

    private Vector2
        _velocity;

    private Vector3
        _mousePosition;

    private void Awake()
    {
        if (rigidBody == null) gameObject.GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = 0f;
        rigidBody.angularDrag = normalDrag;
        rigidBody.drag = normalDrag;
    }

    private void Update()
    {
        Move();
        MousePosition();
        UpdateGraphicsScale();
    }

    private void UpdateGraphicsScale()
    {
        if (_mousePosition.x >= 0.01f) graphics.localScale = graphicsScale;
        else if (_mousePosition.x <= -0.01f) graphics.localScale = new Vector3(-graphicsScale.x, graphicsScale.y, graphicsScale.z);

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
