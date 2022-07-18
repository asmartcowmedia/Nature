using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class GameData
{
    //Variables to save
    public List<InventorySlot> inventorySlots;
    
    public float 
        health,
        cameraZoom;

    public Vector3
        playerPosition,
        cameraPosition;

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
}