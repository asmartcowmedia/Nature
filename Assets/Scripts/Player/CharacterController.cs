using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterController : MonoBehaviour, IDataPersistence
{
      //----------------------------------------//
     // Exposed Variables (Editable in editor) //
    //----------------------------------------//
    [FoldoutGroup("Attachable Objects")][Title("Rigidbodies")][SerializeField] private Rigidbody2D rigidBody;
    
    [FoldoutGroup("Attachable Objects")][Title("Rigidbodies")][SerializeField] private InventoryUI inventoryUI;

    [FoldoutGroup("Attachable Objects")][Title("Transforms")][SerializeField] private Transform graphics;
    [FoldoutGroup("Attachable Objects")][SerializeField] private Transform attackDirection;

    [FoldoutGroup("Attachable Objects")][Title("Other")][SerializeField] private FOV fov;
    [FoldoutGroup("Attachable Objects")][SerializeField] private Camera cameraReference;
    [FoldoutGroup("Attachable Objects")][SerializeField] private AnimationController animationController;

    [FoldoutGroup("Player Variables")][Title("Stamina")][SerializeField] private Stamina stamina;
    [FoldoutGroup("Player Variables")][SerializeField] public float staminaDrain;

    [FoldoutGroup("Player Variables")][Title("Movement")][SerializeField] private float movementSpeed;
    [FoldoutGroup("Player Variables")][SerializeField] private float inWaterDrag;
    [FoldoutGroup("Player Variables")][SerializeField] private float normalDrag;
    [FoldoutGroup("Player Variables")][SerializeField] private string waterTrigger;
    
    [FoldoutGroup("Player Variables")][Title("Attacking")][SerializeField] public float attackDamage;
    [FoldoutGroup("Player Variables")][ReadOnly] public bool isAttacking;

    [FoldoutGroup("Graphics")][SerializeField] public Vector3 graphicsScale;

    [FoldoutGroup("Inventory")][ShowInInspector][ReadOnly] private Inventory inventory;
    //----------------------------------------//
    
      //-------------------------------------------------//
     // Non-Exposed Variables (Not Editable in editor) //
    //-----------------------------------------------//
    private Vector2 velocity;
    private Vector3 mousePosition;
    //----------------------------------------//
    
      //---------------------------//
     // Save/Load Data Functions //
    //-------------------------//
    public void LoadData(GameData data)
    {
        transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = transform.position;
    }
    //----------------------------------------//

      //-------------------------//
     // Default Unity Functions //
    //-------------------------//
    private void Awake()
    {
        //Setting all variables initial state
        if (rigidBody == null) gameObject.GetComponent<Rigidbody2D>();
        if (animationController == null) gameObject.GetComponent<AnimationController>();
        rigidBody.gravityScale = 0f;
        rigidBody.angularDrag = normalDrag;
        rigidBody.drag = normalDrag;

        inventory = new Inventory();
        inventoryUI.SetInventory(inventory);
    }

    private void Update()
    {
        Move();
        MousePosition();
        UpdateGraphicsScale();
        Attack();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if water and call water drag function
        if (other.CompareTag(waterTrigger))
        {
            SetWaterDrag();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //End trigger events and reset drag
        if (other.CompareTag(waterTrigger))
        {
            ResetDrag();
        }
    }
    //----------------------------------------//

      //------------------//
     // Custom Functions //
    //------------------//
    //Function to update the graphics scale determined by mouse position to flip character animations and sprite
    private void UpdateGraphicsScale()
    { 
        if (animationController.isWalkingUp)
        {
            if (mousePosition.x >= 0.01f) graphics.localScale = new Vector3(-graphicsScale.x, graphicsScale.y, graphicsScale.z);
            else if (mousePosition.x <= -0.01f) graphics.localScale = new Vector3(graphicsScale.x, graphicsScale.y, graphicsScale.z);
        }
        
        if (animationController.isWalkingRight)
            graphics.localScale = new Vector3(graphicsScale.x, graphicsScale.y, graphicsScale.z);
                
        if (animationController.isWalkingLeft)
            graphics.localScale = new Vector3(-graphicsScale.x, graphicsScale.y, graphicsScale.z);
    }

    //Function to capture mouse position and translate it to world space for use in other functionality
    private void MousePosition()
    {
        mousePosition = cameraReference.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraReference.transform.position.z * -1));
        mousePosition = transform.InverseTransformPoint(mousePosition);
        
        fov.SetAimDirection(mousePosition);
        fov.SetOrigin(transform.position);
    }

    //Character Movement Function
    private void Move()
    {
        //Gets the velocity from the X and Y Axis of the Unity input system. This corresponds to WASD keys
        velocity.x = Input.GetAxis("Horizontal");
        velocity.y = Input.GetAxis("Vertical");

        //Normalize the velocity so that diagonal movement is not faster than horizontal or vertical
        velocity.Normalize();
        velocity *= 500 * (movementSpeed * Time.deltaTime);

        //Add the velocity to the rigidbody, moving character along desired direction
        rigidBody.AddForce(velocity);
    }

    //Character Attack Function
    private void Attack()
    {
        //Get aim direction from mouse input
        var dir = fov.GetAimDirection(mousePosition);

        //Mak the attack direction the actual rotation
        attackDirection.rotation = Quaternion.Euler(0, 0, dir);

        //When hitting mouse1 ("Fire1") check if has enough stamina and set attack to true and drain stamina proportionately
        if (Input.GetButtonDown("Fire1"))
        {
            if (stamina.stamina > 0 && stamina.stamina - staminaDrain >= 0)
            {
                isAttacking = true;
                stamina.DrainStamina(staminaDrain);
            }
        }
    }

    //Function to set the drag of the character rigidbody to a desired drag in water
    private void SetWaterDrag()
    {
        rigidBody.drag = inWaterDrag;
        rigidBody.angularDrag = inWaterDrag;
    }

    //Function to reset the drag to default
    private void ResetDrag()
    {
        rigidBody.drag = normalDrag;
        rigidBody.angularDrag = normalDrag;
    }
}
