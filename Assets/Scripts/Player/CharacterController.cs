using UnityEditorInternal;
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
        FaceMouse();
    }

    private void FaceMouse()
    {
        var mouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        var mouseToWorld = cameraReference.ScreenToWorldPoint(mouse);
        
        Debug.Log(mouseToWorld);
        
        Debug.DrawLine(transform.position, mouseToWorld, Color.cyan);
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
