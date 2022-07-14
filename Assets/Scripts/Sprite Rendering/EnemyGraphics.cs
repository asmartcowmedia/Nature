using UnityEngine;
using Pathfinding;

public class EnemyGraphics : MonoBehaviour
{
    [SerializeField] private AIPath aiPath;

    [SerializeField] private Vector3
        scale;

    public void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f) transform.localScale = scale;
        else if (aiPath.destination.x <= -0.01f) transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
    }
}