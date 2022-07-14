using System;
using UnityEngine;

public class CamerController : MonoBehaviour
{
    [SerializeField] private float
        zoomSensitivity, 
        initialZoomDistance,
        camLagSpeed,
        camParalaxLayer;

    [SerializeField] private Vector2
        zoomClamp;

    [SerializeField] private Camera cam;

    [SerializeField] private GameObject player;
    
    private float
        _currentZoom;

    private void Start()
    {
        ErrorChecks();
        
        zoomSensitivity *= 500;
        _currentZoom = initialZoomDistance;
    }

    private void Update()
    {
        Follow();
        Zoom();

        if (Input.GetButton("Fire3")) _currentZoom = initialZoomDistance;
    }

    private void Follow()
    {
        var camPos = transform.position;
        var playerPos = player.transform.position;

        var camPosSlerp = Vector3.Lerp(playerPos, camPos, camLagSpeed);

        camPos = new Vector3(camPosSlerp.x, camPosSlerp.y, camParalaxLayer);

        transform.position = camPos;
    }

    private void Zoom()
    {
        var pos = transform.position;
        _currentZoom -= Input.GetAxis("Mouse ScrollWheel") * (zoomSensitivity * Time.deltaTime);

        if (_currentZoom >= zoomClamp.y)
        {
            _currentZoom = zoomClamp.y;
        }
        if (_currentZoom <= zoomClamp.x)
        {
            _currentZoom = zoomClamp.x;
        }
        
        cam.fieldOfView = _currentZoom;
    }

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
