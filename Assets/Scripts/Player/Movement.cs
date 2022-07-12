using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float
        movementSpeed,
        zoomSensitivity;

    [SerializeField] private Vector2
        zoomClamp;

    [SerializeField] private Camera cam;

    private Vector2
        _velocity;

    private Vector3
        _currentZoom;

    private void Start()
    {
        if (cam == null) cam = Camera.main;
        zoomSensitivity *= 50;
    }

    private void Update()
    {
        Move();
        Zoom();
    }

    private void Move()
    {
        _velocity.x = Input.GetAxis("Horizontal");
        _velocity.y = Input.GetAxis("Vertical");

        _velocity *= movementSpeed * Time.deltaTime;

        transform.Translate(_velocity);
    }

    private void Zoom()
    {
        var pos = cam.transform.position;
        _currentZoom.z += Input.GetAxis("Mouse ScrollWheel") * (zoomSensitivity * Time.deltaTime);

        if (_currentZoom.z <= zoomClamp.y)
        {
            _currentZoom.z = zoomClamp.y;
        }
        if (_currentZoom.z >= zoomClamp.x)
        {
            _currentZoom.z = zoomClamp.x;
        }
        
        cam.transform.localPosition = new Vector3(0, 0, _currentZoom.z);
    }
}
