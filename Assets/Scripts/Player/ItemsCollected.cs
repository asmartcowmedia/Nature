using System;
using UnityEngine;

public class ItemsCollected : MonoBehaviour, IDataPersistence
{
    [SerializeField] private InventoryHolder inv;

    [SerializeField] private InventoryItemData
        machete,
        infectedMachete,
        headlamp,
        map,
        infectedMap;

    public bool
        macheteCollected,
        infectedMacheteCollected,
        headlampCollected,
        mapCollected,
        infectedMapCollected;
    
    public static ItemsCollected Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene! Destroying new one, keeping old!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadData(GameData data)
    {
        macheteCollected = data.macheteCollected;
        infectedMacheteCollected = data.infectedMacheteCollected;
        headlampCollected = data.headlampCollected;
        mapCollected = data.mapCollected;
        infectedMapCollected = data.infectedMapCollected;
        
        if (macheteCollected)
            inv.InventorySystem.AddToInventory(machete, 1);
        if (infectedMacheteCollected)
            inv.InventorySystem.AddToInventory(infectedMachete, 1);
        if (headlampCollected)
            inv.InventorySystem.AddToInventory(headlamp, 1);
        if (mapCollected)
            inv.InventorySystem.AddToInventory(map, 1);
        if (infectedMapCollected)
            inv.InventorySystem.AddToInventory(infectedMap, 1);
    }

    public void SaveData(GameData data)
    {
        data.macheteCollected = macheteCollected;
        data.infectedMacheteCollected = infectedMacheteCollected;
        data.headlampCollected = headlampCollected;
        data.mapCollected = mapCollected;
        data.infectedMapCollected = infectedMapCollected;
    }
    

    public void UpdateSave(InventoryItemData data)
    {
        switch (data.id)
        {
            case 1:
                macheteCollected = true;
                break;
            case 2:
                infectedMacheteCollected = true;
                break;
            case 3:
                headlampCollected = true;
                break;
            case 4:
                mapCollected = true;
                break;
            case 5:
                infectedMapCollected = true;
                break;
        }
    }
}