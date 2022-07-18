using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class GameData
{
    //Variables to save
    public long lastUpdated;
    
    public List<InventorySlot> inventorySlots;
    
    public float 
        health,
        cameraZoom;

    public Vector3
        playerPosition,
        cameraPosition;
    
    public bool
        macheteCollected,
        infectedMacheteCollected,
        headlampCollected,
        mapCollected,
        infectedMapCollected;

    //Dictionary for all saved data
    [FormerlySerializedAs("CollectablesCollected")] public SerializableDictionary<string, bool> collectablesCollected;
    
    //publically callable function to retrieve base gamedata
    public GameData()
    {
        health = 100;
        playerPosition = Vector3.zero;
        cameraPosition = new Vector3(0, 0, -5);
        cameraZoom = 100;
        collectablesCollected = new SerializableDictionary<string, bool>();
        inventorySlots = new List<InventorySlot>(10);
    }

    public int GetPercentageComplete()
    {
        var totalCollected = 0;
        foreach (var collected in collectablesCollected.Values)
        {
            if (collected)
            {
                totalCollected++;
            }
        }

        var percentageCompleted = 0;
        if (collectablesCollected.Count > 0)
        {
            percentageCompleted = (totalCollected * 100 / collectablesCollected.Count);
        }

        return percentageCompleted;
    }
}