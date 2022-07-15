using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CollectableManager : MonoBehaviour, IDataPersistence
{
    //public callable Function to load data
    public void LoadData(GameData data)
    {
        foreach (KeyValuePair<string, bool> pair in data.collectablesCollected)
        {
            if (pair.Value)
            {
                totalCollected++;
            }
        }
    }

    //Public callable function to save data
    public void SaveData(ref GameData data)
    {
    }

    //Variables for collectables
    [SerializeField] private int totalCollectables;

    [ShowInInspector] [ReadOnly] private int totalCollected;
}