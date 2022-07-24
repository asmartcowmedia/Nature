using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

public class Plate : MonoBehaviour
{
    [SerializeField] private new SpriteRenderer renderer;
    [SerializeField] private Sprite active;
    [SerializeField] private Sprite inactive;
    [SerializeField] private bool doesDeactivate;
    [SerializeField] private float timeToDeactivate;

    [ReadOnly] public bool isActive;

    private IEnumerator Wait()
    {
        if (doesDeactivate)
        {
            yield return new WaitForSeconds(timeToDeactivate);
            isActive = false;
        
            renderer.sprite = inactive;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            UpdateRender();
        }
    }

    private void UpdateRender()
    {
        if (renderer.sprite == inactive)
        {
            renderer.sprite = active;
        
            isActive = true;

            StartCoroutine(Wait());
        }
    }
}