using System;
using UnityEngine;

public class FOV : MonoBehaviour
{
    [SerializeField] private float 
        fov = 90f,
        angle = 0f,
        viewDistance = 50f;
    
    [SerializeField] private int rayCount = 3;

    [SerializeField] private LayerMask mask;

    [SerializeField] private Vector3 origin;

    private Mesh _mesh;

    private float _startingAngle;

    private void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        origin = Vector3.zero;
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
}