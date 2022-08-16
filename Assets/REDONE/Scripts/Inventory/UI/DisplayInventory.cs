using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

namespace CampingTrip
{
    /// <summary>
    /// Displays inventory on UI.
    /// </summary>
    
    public class DisplayInventory : MonoBehaviour
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [FoldoutGroup("Inventory")][SerializeField]public SoInventoryObject inventory;

        [FoldoutGroup("Inventory Dictionary")][SerializeField] public Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

        private void Start()
        {
            CreateDisplay();
        }

        private void Update()
        {
            UpdateDisplay();
        }

        private void CreateDisplay()
        {
            for (var i = 0; i < inventory.container.Count; i++)
            {
                var obj = Instantiate(inventory.container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = this.transform.position;
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.container[i].amount.ToString("n0");
                    
                itemsDisplayed.Add(inventory.container[i], obj);
            }
        }

        public void UpdateDisplay()
        {
            for (var i = 0; i < inventory.container.Count; i++)
            {
                if (itemsDisplayed.ContainsKey(inventory.container[i]))
                {
                    itemsDisplayed[inventory.container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.container[i].amount.ToString("n0");
                }
                else
                {
                    var obj = Instantiate(inventory.container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                    obj.GetComponent<RectTransform>().localPosition = this.transform.position;
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.container[i].amount.ToString("n0");
                    
                    itemsDisplayed.Add(inventory.container[i], obj);
                }
            }
        }
    }
}