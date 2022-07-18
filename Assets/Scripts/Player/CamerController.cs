using UnityEngine;
using Sirenix.OdinInspector;

public class CamerController : MonoBehaviour, IDataPersistence
{
      //----------------------------------------//
     // Exposed Variables (Editable in editor) //
    //----------------------------------------//
    [FoldoutGroup("Attachable Objects")][Title("Camera")][SerializeField] private Camera cam;
    
    [FoldoutGroup("Attachable Objects")][Title("GameObjects")][SerializeField] private GameObject player;

    [FoldoutGroup("Variables")][Title("Zoom")][SerializeField] private float zoomSensitivity;
    [FoldoutGroup("Variables")][SerializeField] private float initialZoomDistance;
    [FoldoutGroup("Variables")][SerializeField] private Vector2 zoomClamp;
    
    [FoldoutGroup("Variables")][Title("Speed")][SerializeField] private float camLagSpeed;
    
    [FoldoutGroup("Variables")][Title("Layers")][SerializeField] private float camParalaxLayer;
    //----------------------------------------//
    
      //------------------------------------------------//
     // Non-Exposed Variables (Not Editable in editor) //
    //------------------------------------------------//
    private float currentZoom;
    //----------------------------------------//
    
      //---------------------//
     // Save/Load Functions //
    //---------------------//
    public void LoadData(GameData data)
    {
        transform.position = data.cameraPosition;
        currentZoom = data.cameraZoom;
        initialZoomDistance = data.cameraZoom;
    }

    public void SaveData(ref GameData data)
    {
        data.cameraPosition = transform.position;
        data.cameraZoom = currentZoom;
    }
    //----------------------------------------//

      //-------------------------//
     // Default Unity Functions //
    //-------------------------//
    private void Start()
    {
        //Checks for any errors
        ErrorChecks();
        
        //Sets default variable stats
        zoomSensitivity *= 500;
        currentZoom = initialZoomDistance;
    }

    private void Update()
    {
        Follow();
        Zoom();

        //If press middle mouse, reset zoom distance
        if (Input.GetButton("Fire3")) currentZoom = initialZoomDistance;
    }
    //----------------------------------------//

      //------------------//
     // Custom Functions //
    //------------------//
    //Function to follow the player character
    private void Follow()
    {
        var camPos = transform.position;
        var playerPos = player.transform.position;

        var camPosSlerp = Vector3.Lerp(playerPos, camPos, camLagSpeed);

        camPos = new Vector3(camPosSlerp.x, camPosSlerp.y, camParalaxLayer);

        transform.position = camPos;
    }

    //Function to zoom with mouse scroll
    private void Zoom()
    {
        var pos = transform.position;
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * (zoomSensitivity * Time.deltaTime);

        if (currentZoom >= zoomClamp.y)
        {
            currentZoom = zoomClamp.y;
        }
        if (currentZoom <= zoomClamp.x)
        {
            currentZoom = zoomClamp.x;
        }
        
        cam.fieldOfView = currentZoom;
    }

    //Function to run checks to make sure things happen right
    private void ErrorChecks()
    {
        if (cam == null)
        {
            cam = Camera.main;
            if (cam != null)
            {
                Debug.Log("!!Camera not found!! Quitting application....");
                Application.Quit();
            }
        }

        if (player == null) Debug.Log("!!No player GameObject selected, game will run, but will not follow character!");
    }
}