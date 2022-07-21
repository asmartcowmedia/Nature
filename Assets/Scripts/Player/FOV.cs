using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class FOV : MonoBehaviour
{
    //Keep item between scenes
    public static FOV Instance { get; private set; }

    private void KeepOnDestroy()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one FOV in the scene! Destroying new one, keeping old!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    [SerializeField] private float 
        fov = 90f,
        angle = 0f,
        viewDistance = 5f,
        headlampViewDistance = 10f,
        headlampFov = 120;
    
    [SerializeField] private int rayCount = 3;

    [SerializeField] private LayerMask mask;

    [SerializeField] private Vector3 origin;
    
    [ReadOnly] public bool headLampToggle;

    private Mesh _mesh;

    private float
        _startingAngle,
        originalViewDistance,
        originalFov;

    private void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        origin = Vector3.zero;

        originalViewDistance = viewDistance;
        originalFov = fov;
    }

    private void Awake()
    {
        KeepOnDestroy();
    }

    private void Update()
    {
        if (headLampToggle)
        {
            fov = headlampFov;
            viewDistance = headlampViewDistance;
        }

        if (!headLampToggle)
        {
            fov = originalFov;
            viewDistance = originalViewDistance;
        }
    }

    private void LateUpdate()
    {
        angle = _startingAngle;
        var angleIncrease = fov / rayCount;
        var vertices = new Vector3[rayCount + 1 + 1];
        var uv = new Vector2[vertices.Length];
        var triangles = new int[rayCount * 3];

        vertices[0] = origin;

        var vertexIndex = 1;
        var triangleIndex = 0;
        for (var i = 0; i < rayCount; i++)
        {
            Vector3 vertex;

            var hit = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, mask);

            if (hit.collider == null)
            {
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                vertex = hit.point;
            }

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex -1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.triangles = triangles;
        _mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }

    private static Vector3 GetVectorFromAngle(float angle)
    {
        var angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        _startingAngle = GetAngleFromVectorFloat(aimDirection) + fov / 2f;
    }

    public float GetAimDirection(Vector3 aimDirection)
    {
        return GetAngleFromVectorFloat(aimDirection);
    }
}