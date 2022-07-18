using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private MouseItemData mouseInventoryItem;

    [SerializeField] private CharacterController player;
    
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;

    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;

    protected virtual void Start()
    {
    }

    public abstract void AssignSlot(InventorySystem invToDisplay);

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot) // slot value : Backend slot
            {
                slot.Key.UpdateUISlot(updatedSlot); // slot key : UI slot
            }
        }
    }

    public void SlotClicked(InventorySlot_UI clickedSlot)
    {
        if (clickedSlot.AssignedInventorySlot.ItemData != null)
        {
            if (clickedSlot.AssignedInventorySlot.ItemData.id == 1)
            {
                player.isHoldingMachete = true;
                
                player.isHoldingInfectedMap = false;
                player.isHoldingMap = false;
                player.isHoldingInfectedMachete = false;
            }
            if (clickedSlot.AssignedInventorySlot.ItemData.id == 2)
            {
                player.isHoldingInfectedMachete = true;
                
                player.isHoldingInfectedMap = false;
                player.isHoldingMap = false;
                player.isHoldingMachete = false;
            }
            if (clickedSlot.AssignedInventorySlot.ItemData.id == 3)
            {
                player.isHoldingHeadlamp = true;
                
                player.isHoldingInfectedMap = false;
                player.isHoldingMap = false;
            }
            if (clickedSlot.AssignedInventorySlot.ItemData.id == 4)
            {
                player.isHoldingMap = true;
                
                player.isHoldingInfectedMap = false;
                player.isHoldingMachete = false;
                player.isHoldingInfectedMachete = false;
            }
            if (clickedSlot.AssignedInventorySlot.ItemData.id == 5)
            {
                player.isHoldingInfectedMap = true;
                
                player.isHoldingMap = false;
                player.isHoldingMachete = false;
                player.isHoldingInfectedMachete = false;
            }
        }
    }
}