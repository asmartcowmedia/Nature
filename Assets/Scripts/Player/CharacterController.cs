using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;

    [SerializeField] private FOV fov;
    
    [SerializeField] private float
        movementSpeed;

    [SerializeField] private Camera cameraReference;

    private Vector2
        _velocity;

    private Vector3
        _mousePosition;

    private void Awake()
    {
        if (rigidBody == null) gameObject.GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = 0f;
        rigidBody.angularDrag = 8f;
        rigidBody.drag = 8f;
    }

    private void Update()
    {
        Move();
        MousePosition();
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
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            Debug.Log("IN WATER");
        }
    }

    private void SetWaterDrag()
    {
    }

    private void ResetDrag()
    {
    }
}
