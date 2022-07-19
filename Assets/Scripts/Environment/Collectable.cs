using UnityEngine;
using Sirenix.OdinInspector;

public class Collectable : MonoBehaviour, IDataPersistence
{
    //!! NOT SET UP YET!!\\
    [SerializeField] private CollectableManager manager;
    
    [SerializeField][ReadOnly] private string id;
    [SerializeField][ReadOnly] private bool collected;

    [ContextMenu("Generate GUID for ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    
    public void LoadData(GameData data)
    {
        data.collectablesCollected.TryGetValue(id, out collected);
        if (collected)
            collectable.gameObject.SetActive(false);
    }

    public void SaveData(GameData data) 
    {
    }

    [SerializeField] private GameObject collectable;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!collected)
        {
            Collect();
        }
    }

    private void Collect()
    {
        manager.Collect(id, true);
        collectable.SetActive(false);
    }
}