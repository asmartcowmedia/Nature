using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CollectableManager : MonoBehaviour, IDataPersistence
{
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

    public void SaveData(ref GameData data)
    {
    }

    [SerializeField] private int totalCollectables;

    [ShowInInspector] [ReadOnly] private int totalCollected;
}