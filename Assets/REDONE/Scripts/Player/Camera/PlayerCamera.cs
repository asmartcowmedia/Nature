using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CampingTrip
{
public class PlayerCamera : MonoBehaviour
{
    // Serialized and editable from the Unity inspector, not editable in other scripts //
    [FoldoutGroup("Attachable Objects")] 
    [Title("In Scene")][SerializeField] private GameObject player;
    
    [FoldoutGroup("Attachable Objects")] 
    [Title("In Components")][SerializeField] private Camera cameraComponent;
    
    [FoldoutGroup("Camera Variables")] 
    [Title("Movement")][SerializeField] private float lerpTime;
    [FoldoutGroup("Camera Variables")][SerializeField] private float cameraZoomSpeed;
    [FoldoutGroup("Camera Variables")][SerializeField] private Vector2 cameraZoomClamp;
    
    // Not editable in the Unity inspector, but visible, not callable from other scripts //
    [FoldoutGroup("Debugging")]
    [Title("Read Only")][ReadOnly][SerializeField] private float cameraZoomDistance;
    
    // Not editable in Unity Inspector or callable from other scripts //
    private PlayerControls inputSystem;
    private InputAction zoom;

    // Default Unity functions //
    private void Awake() // Function called as soon as the object is instantiated, but before the Start function
    {
        inputSystem = new PlayerControls(); // Instantiates the "inputSystem" Variable with the actual player controls scheme
        
        // Attach game objects if they are not currently attached
        if (!player)
            player = FindObjectOfType<Movement>().gameObject;
        if (!cameraComponent)
            cameraComponent = GetComponent<Camera>();
    }

    private void OnEnable() // Called when the object is active in the scene
    {
        // Set all the variables for the input system to that of their counterparts in the input scheme
        zoom = inputSystem.Player.CameraZoom;
        
        // Enable all input systems when activating object
        zoom.Enable();
    }
    
    private void Update() // Called every frame
    {
        // Call functions
        FollowPlayer();
        CameraZoom();
    }

    private void OnDisable() // Called when the object is disabled in the scene
    {
        // Disable input system on disabling the object
        zoom.Disable();
    }

    // Private functions //
    private void FollowPlayer() // A function to follow the player
    {
        // Follow player on lerp trajectory from the position of the cam to the player position
        var posLerp = Vector2.Lerp(transform.position, player.transform.position, lerpTime);
        var posVector = new Vector3(posLerp.x, posLerp.y, -15);

        // Implement logic to move camera to payer
        transform.position = posVector;
    }

    private void CameraZoom()
    {
        //Set the camera zoom clamp and then insert that zoom into the position vector
        cameraZoomDistance -= zoom.ReadValue<Vector2>().y * (cameraZoomSpeed * Time.deltaTime);

        if (cameraZoomDistance >= cameraZoomClamp.y)
            cameraZoomDistance = cameraZoomClamp.y;
        if (cameraZoomDistance <= cameraZoomClamp.x)
            cameraZoomDistance = cameraZoomClamp.x;

        // Implement logic for zooming camera
        cameraComponent.fieldOfView = cameraZoomDistance;
    }
}
}