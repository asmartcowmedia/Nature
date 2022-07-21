using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class InventoryHolder : MonoBehaviour
{
    //Keep item between scenes
    public static InventoryHolder Instance { get; private set; }

    private void KeepOnDestroy()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one InventoryHolder in the scene! Destroying new one, keeping old!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    [SerializeField] private int inventorySize;

    [SerializeField] protected InventorySystem inventorySystem;

    public InventorySystem InventorySystem => inventorySystem;
    
    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequested;

    private void Awake()
    {
        inventorySystem = new InventorySystem(inventorySize);
    }
}