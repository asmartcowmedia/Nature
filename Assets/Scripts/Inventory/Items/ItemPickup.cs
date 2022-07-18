using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private InventoryHolder inventory;

    [SerializeField] private ItemsCollected itemsCollected;
    
    public float pickupRadius = 1f;

    public InventoryItemData ItemData;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (!inventory) return;

            if (inventory.InventorySystem.AddToInventory(ItemData, ItemData.amountToPickUp))
            {
                itemsCollected.UpdateSave(ItemData);
                Destroy(gameObject);
            }
        }
    }
}