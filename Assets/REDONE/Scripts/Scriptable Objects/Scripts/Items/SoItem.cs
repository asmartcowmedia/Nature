using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CampingTrip
{
    /// <summary>
    /// The default container for all item types.
    /// </summary>
    
    // enum for item types
    public enum ItemType
    {
        Weapon,
        HealingItem,
        DamagingItem,
        Equipment,
        Default
    }
    
    // enum for attributes
    public enum Attributes
    {
        Agility,
        Intellect,
        Stamina,
        Strength
    }
    
    public abstract class SoItem : ScriptableObject
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Variables")] 
        [Title("Item Data", TitleAlignment = TitleAlignments.Centered)][SerializeField] public int id;
        [FoldoutGroup("Variables")][SerializeField] public Sprite uiDisplay;
        [FoldoutGroup("Variables")][SerializeField] public ItemType itemType;
        [FoldoutGroup("Variables")][TextArea(15,20)][SerializeField] public string description;
        [FoldoutGroup("Variables")][SerializeField] public ItemBuff[] buffs;

        public Item CreateItem()
        {
            var newItem = new Item(this);
            return newItem;
        }
    }

    [Serializable]
    public class Item
    {
        [Title("Item Data", TitleAlignment = TitleAlignments.Centered)][SerializeField] public string name;
        [SerializeField] public int id;
        [SerializeField] public ItemBuff[] buffs;

        public Item(SoItem item)
        {
            name = item.name;
            id = item.id;
            buffs = new ItemBuff[item.buffs.Length];

            for (var i = 0; i < buffs.Length; i++)
            {
                buffs[i] = new ItemBuff(item.buffs[i].min, item.buffs[i].max) { attribute = item.buffs[i].attribute };
            }
        }
    }

    [Serializable]
    public class ItemBuff
    {
        [Title("Item Data", TitleAlignment = TitleAlignments.Centered)][SerializeField] public Attributes attribute;
        [SerializeField] public int value;
        [SerializeField] public int min;
        [SerializeField] public int max;

        public ItemBuff (int _min, int _max)
        {
            min = _min;
            max = _max;

            GenerateValue();
        }

        public void GenerateValue()
        {
            value = UnityEngine.Random.Range(min, max);
        }
    }
}