using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class InventorySlot_UI : MonoBehaviour
{
    [SerializeField] private Image 
        itemSprite,
        itemCountTextBg;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField][ReadOnly] private InventorySlot assignedInventorySlot;

    private Button button;

    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    public InventoryDisplay ParentDisplay { get; private set; }

    private void Awake()
    {
        ClearSlot();

        button = GetComponent<Button>();
        
        button?.onClick.AddListener(OnUISlotClick);

        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    public void Init(InventorySlot slot)
    {
        assignedInventorySlot = slot;
        UpdateUISlot(slot);
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.ItemData != null)
        {
            itemSprite.gameObject.SetActive(true);
            itemSprite.sprite = slot.ItemData.icon;
        }
        else
            ClearSlot();

        if (slot.StackSize > 1)
        {
            itemCountTextBg.gameObject.SetActive(true);
            
            itemCount.text = slot.StackSize.ToString();
            itemCount.gameObject.SetActive(true);
        }
        else
        {
            itemCount.gameObject.SetActive(false);
            itemCountTextBg.gameObject.SetActive(false);
        }
    }

    public void UpdateUISlot()
    {
        if (assignedInventorySlot != null)
            UpdateUISlot(assignedInventorySlot);
    }

    private void OnUISlotClick()
    {
        ParentDisplay?.SlotClicked(this);
    }

    public void ClearSlot()
    {
        assignedInventorySlot?.ClearSlot();
        itemSprite.gameObject.SetActive(false);
    }
} 