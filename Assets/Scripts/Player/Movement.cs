using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    
    [SerializeField] private float
        movementSpeed;

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
    }

    private void Move()
    {
        _velocity.x = Input.GetAxis("Horizontal");
        _velocity.y = Input.GetAxis("Vertical");

        _velocity *= 500 * (movementSpeed * Time.deltaTime);

        rigidBody.AddForce(_velocity);
    }
}
