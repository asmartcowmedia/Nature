using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public int id;
    public string displayName;
    public string itemType;
    [TextArea(4, 4)] public string description;
    
    public Sprite icon;
    public int amountToPickUp;
    public int MaxStackSize;
}