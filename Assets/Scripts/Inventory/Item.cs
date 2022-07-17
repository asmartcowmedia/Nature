using UnityEngine;

public class Item
{
    public enum ItemType
    {
        Machete,
        InfectedMachete,
        Map,
        InfectedMap,
        Headlamp,
    }

    public ItemType itemType;
    public int amount;
}