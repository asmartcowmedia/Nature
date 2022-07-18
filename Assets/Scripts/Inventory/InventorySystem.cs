using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;

    public List<InventorySlot> InventorySlots => inventorySlots;

    public int InventorySize => inventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;
    
    public void LoadData(GameData data)
    {
        inventorySlots = data.inventorySlots;
    }

    public void SaveData(ref GameData data)
    {
        data.inventorySlots = inventorySlots;
    }

    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        if (ContainsItem(itemToAdd, out List<InventorySlot> invSlot)) // Exist?
        {
            foreach (var slot in invSlot)
            {
                if (slot.RoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
        }
        
        if (HasFreeSlot(out InventorySlot freeSlot)) // First available slot
        {
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }

        return false;
    }

    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot)
    {
        invSlot = inventorySlots.Where(i => i.ItemData == itemToAdd).ToList();

        return invSlot != null;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null);
        
        return freeSlot != null;
    }
}