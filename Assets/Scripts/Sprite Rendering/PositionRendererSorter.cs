using UnityEngine;

public class PositionRendererSorter : MonoBehaviour
{
    [SerializeField] private bool runOnce = false;
    [SerializeField] private new Renderer renderer;

    [SerializeField] private int 
        sortingOrderBase = 5000,
        offset = 0;

    private void Awake()
    {
        if (renderer == null) 
            renderer = gameObject.GetComponent<Renderer>();
    }

    private void LateUpdate()
    {
        renderer.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
        if (runOnce) Destroy(this);
    }
}