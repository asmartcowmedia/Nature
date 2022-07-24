using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEditor;

public class PlatePuzzle : MonoBehaviour
{
    public new SpriteRenderer renderer;
    [SerializeField] private Sprite active;
    [SerializeField] private Sprite inactive;
    [SerializeField] private PlatePuzzle[] plates;

    [ReadOnly] public bool isActive;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            UpdatePlates();
        }
    }

    private void UpdatePlates()
    {
        if (renderer.sprite == inactive)
        {
            for (int i = 0; i < plates.Length; i++)
            {
                if (plates[i].isActive)
                {
                    plates[i].renderer.sprite = inactive;
                    plates[i].isActive = false;
                }

                if (!plates[i].isActive)
                {
                    plates[i].renderer.sprite = active;
                    plates[i].isActive = true;
                }
            }
        }
    }
}