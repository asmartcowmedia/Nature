using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private InventoryHolder inventory;
    
    public float pickupRadius = 1f;

    public InventoryItemData ItemData;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (!inventory) return;

            if (inventory.InventorySystem.AddToInventory(ItemData, 1))
            {
                Destroy(gameObject);
            }
        }
    }
}