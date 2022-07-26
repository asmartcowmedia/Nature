using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CampingTrip
{
public class PlayerAttack : MonoBehaviour
{
    // Serialized and editable from the Unity inspector, not editable in other scripts //
    [FoldoutGroup("Attachable Objects")]
    [Title("In Scene")][SerializeField] private GameObject attackHitBox;
    
    // Not editable in Unity Inspector or callable from other scripts //
    private Camera cameraReference;
    private PlayerControls inputSystem;
    private InputAction 
        attack,
        mouseInputPosition;

    // Default Unity functions //
    private void Awake() // Function called as soon as the object is instantiated, but before the Start function
    {
        inputSystem = new PlayerControls(); // Instantiates the "inputSystem" Variable with the actual player controls scheme
        
        // Set default values
        if (!cameraReference)
            cameraReference = FindObjectOfType<Camera>();
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
    }

    private void OnDisable() // Called when the object is disabled in the scene
    {
        // Disable input system on disabling the object
        attack.Disable();
        mouseInputPosition.Disable();
    }
    
    // Private Functions //
    private void Attack() // Function for the player to attack enemies
    {
        // Get the mouse direction from a personal script made with the function to get the mouse position
        var calculatedMousePosition = mouseInputPosition.ReadValue<Vector2>();
        var rot = CowsCompendium.GetMousePosition(cameraReference, calculatedMousePosition, transform);
        var attackDirection = CowsCompendium.GetAngleFromVectorFloat(rot);
            
        // Set the attack hit box rotation to that of the attack direction
        attackHitBox.transform.rotation = Quaternion.Euler(0, 0, attackDirection);
        
        // If the attack button was pressed, perform the attack logic
        if (attack.WasPressedThisFrame())
        {
        }
    }
}
}