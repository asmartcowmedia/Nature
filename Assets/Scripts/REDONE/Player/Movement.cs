using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

namespace CampingTrip
{
[RequireComponent(typeof(Rigidbody2D))] // Requirement of a rigidbody component added to the player object
public class Movement : MonoBehaviour
{
    // Serialized and editable from the Unity inspector, not editable in other scripts //
    [FoldoutGroup("Attachable Objects")]
    [Title("Physics")][SerializeField] private new Rigidbody2D rigidbody;
    
    [FoldoutGroup("Player Variables")]
    [Title("Movement")][SerializeField] private float movementSpeed;
    [FoldoutGroup("Player Variables")][SerializeField] private float sprintMultiplier;
    [FoldoutGroup("Player Variables")][SerializeField] private float angularDrag;
    [FoldoutGroup("Player Variables")][SerializeField] private float drag;
    
    // Non-editable from the Unity inspector, not editable in other scripts //
    private Vector2 velocity;
    private PlayerControls inputSystem;
    private InputAction 
        move,
        sprint;
    
    // Default Unity functions //
    private void Awake() // Function called as soon as the object is instantiated, but before the Start function
    {
        inputSystem = new PlayerControls(); // Instantiates the "inputSystem" Variable with the actual player controls scheme

        // if the rigidbody variable is not set, it sets it to the rigidbody component attached to the player object
        if (!rigidbody)
            rigidbody = GetComponent<Rigidbody2D>(); 
    }

    private void OnEnable() // Called when the object is active in the scene
    {
        // Set all the variables for the input system to that of their counterparts in the input scheme
        move = inputSystem.Player.Move;
        sprint = inputSystem.Player.Sprint;
        
        // Enable all input systems when activating object
        move.Enable();
        sprint.Enable();
    }

    private void Start() // Called right before first frame has started, but after OnEnable and Awake
    {
        // Setting up the rigidbody with default variables
        rigidbody.gravityScale = 0;
        rigidbody.angularDrag = angularDrag;
        rigidbody.drag = drag;
    }

    private void Update() // Called every frame
    {
        // Call functions
        PlayerMovement();
    }

    private void OnDisable() // Called when the object is disabled in the scene
    {
        // Disable input system on disabling the object
        move.Disable();
        sprint.Disable();
    }

    // Private functions //
    private void PlayerMovement() // Player movement. All functionality of movement will be implemented here
    { 
        // This portion of code sets the velocity of the character to that of the "move" function of the input system
        velocity.x = move.ReadValue<Vector2>().x;
        velocity.y = move.ReadValue<Vector2>().y;

        // This portion normalizes the velocity and then multiplies it by the speed set above and the delta time
        // This prevents faster diagonal movement
        velocity.Normalize();
        velocity *= 500 * (movementSpeed * Time.deltaTime);
        
        // If the sprint button is held down, will multiply movement speed by the sprint multiplier
        if (sprint.IsPressed())
            velocity *= sprintMultiplier;

        // Implementation of the movement forces on the player character
        rigidbody.AddForce(velocity);
    }
}
}