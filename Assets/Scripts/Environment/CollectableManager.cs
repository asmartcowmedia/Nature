using Sirenix.OdinInspector;
using UnityEngine;

public class CollectableManager : MonoBehaviour, IDataPersistence
{
    //public callable Function to load data
    public void LoadData(GameData data)
    {
        collected = data.collectablesCollected;
    }

    //Public callable function to save data
    public void SaveData(GameData data)
    {
        data.collectablesCollected = collected;
    }

    //Variables for collectables
    public SerializableDictionary<string, bool> collected;
    
    [SerializeField][ReadOnly] private int totalCollectables;
    [SerializeField][ReadOnly] private int totalCollected;

    public void Collect(string id, bool collect)
    {
        bool value;
        
        Debug.Log("Collecting...");
        if (!collected.ContainsKey(id))
        {
            Debug.Log("Collected " + id);
            collected.Add(id, collect);
        }
        else if (collected.ContainsKey(id) && collected.TryGetValue(id, out value))
        {
            Debug.Log("Collection contains Id, but the key value does not match, adjusting...");
            collected[id] = true;
        }
    }
}