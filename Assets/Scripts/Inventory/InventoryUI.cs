using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;

    private void Awake()
    {
        itemSlotContainer = transform.Find("ItemSlotContainer");
        itemSlotTemplate = transform.Find("ItemSlotTemplate");
    }

    public void SetInventory(Inventory inv)
    {
        inventory = inv;
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        var x = 0;
        var y = 0;
        var itemSlotCellSize = 30f;
        
        foreach (Item item in inventory.GetItemList())
        {
            var itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            x++;

            if (x > 4)
            {
                x = 0;
                y++;
            }
        }
    }
}