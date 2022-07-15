using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class GameData
{
    public float 
        health,
        cameraZoom;

    public Vector3
        playerPosition,
        cameraPosition;

    [FormerlySerializedAs("CollectablesCollected")] public SerializableDictionary<string, bool> collectablesCollected;

    public GameData()
    {
        health = 50;
        playerPosition = Vector3.zero;
        cameraPosition = new Vector3(0, 0, -5);
        cameraZoom = 0;
        collectablesCollected = new SerializableDictionary<string, bool>();
    }
}