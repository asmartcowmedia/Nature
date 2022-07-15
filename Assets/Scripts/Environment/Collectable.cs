using UnityEngine;
using Sirenix.OdinInspector;

public class Collectables : MonoBehaviour, IDataPersistence
{
    //!! NOT SET UP YET!!\\
    
    [ShowInInspector][ReadOnly] private string id;

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

    public void SaveData(ref GameData data)
    {
        if (data.collectablesCollected.ContainsKey(id))
            data.collectablesCollected.Remove(id);

        data.collectablesCollected.Add(id, collected);
    }

    private bool collected = false;

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
        collectable.SetActive(false);
    }
}