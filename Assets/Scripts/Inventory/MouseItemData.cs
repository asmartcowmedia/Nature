using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MouseItemData : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemCount;

    public InventorySlot InventorySlot;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        InventorySlot.AssignItem(invSlot);
        itemSprite.sprite = invSlot.ItemData.icon;
        itemCount.text = invSlot.StackSize.ToString();
        gameObject.SetActive(true);
    }
}