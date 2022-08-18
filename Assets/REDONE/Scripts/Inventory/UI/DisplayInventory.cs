using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CampingTrip
{
    /// <summary>
    /// Displays inventory on UI.
    /// </summary>
    
    public class DisplayInventory : MonoBehaviour
    {
        // Serialized and editable from the Unity inspector, not editable in other scripts //
        [ReadOnly] public MouseItem mouseItem = new MouseItem();
        [FoldoutGroup("Prefabs")][SerializeField] public GameObject inventoryPrefab;
        [FoldoutGroup("Inventory")][SerializeField]public SoInventoryObject inventory;

        [FoldoutGroup("Inventory Dictionary")][SerializeField] public Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        
        // Private variables//
        private PlayerControls inputSystem;
        private InputAction mousePosition;
        private void Awake()
        {
            inputSystem = new PlayerControls(); // Instantiates the "inputSystem" Variable with the actual player controls scheme
        }

        private void OnEnable() // Called when the object is active in the scene
        {
            // Set all the variables for the input system to that of their counterparts in the input scheme
            mousePosition = inputSystem.Player.MousePosition;
        
            // Enable all input systems when activating object
            mousePosition.Enable();
        }

        private void OnDisable() // Called when the object is disabled in the scene
        {
            // Disable input system on disabling the object
            mousePosition.Disable();
        }

        private void Start()
        {
            CreateSlots();
        }

        private void Update()
        {
            UpdateSlots();
        }
        
        private void CreateSlots()
        {
            itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

            for (var i = 0; i < inventory.container.items.Length; i++)
            {
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                
                AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
                AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
                AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
                AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
                AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
                
                itemsDisplayed.Add(obj, inventory.container.items[i]);
            }
        }

        private void UpdateSlots()
        {
            foreach (var _slot in itemsDisplayed)
            {
                if (_slot.Value.id >= 0)
                {
                    _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite =
                        inventory.database.GetItem[_slot.Value.item.id].uiDisplay;
                    _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                    _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text =
                        _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
                }
                else
                {
                    _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                    _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                    _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
                }
            }
        }

        private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            var trigger = obj.GetComponent<EventTrigger>();
            var eventTrigger = new EventTrigger.Entry();

            eventTrigger.eventID = type;
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }

        public void OnEnter(GameObject obj)
        {
            mouseItem.hoverObj = obj;

            if (itemsDisplayed.ContainsKey(obj))
            {
                mouseItem.hoverItem = itemsDisplayed[obj];
            }
        }

        public void OnExit(GameObject obj)
        {
            mouseItem.hoverObj = null;
            mouseItem.hoverItem = null;
        }

        public void OnDragStart(GameObject obj)
        {
            var mouseObject = new GameObject();
            var rt = mouseObject.AddComponent<RectTransform>();

            rt.sizeDelta = new Vector2(50, 50);
            mouseObject.transform.SetParent(transform.parent);
            
            if (itemsDisplayed[obj].id >= 0)
            {
                var image = mouseObject.AddComponent<Image>();
                 
                image.sprite = inventory.database.GetItem[itemsDisplayed[obj].id].uiDisplay;

                image.raycastTarget = false;
            }

            mouseItem.obj = mouseObject;
            mouseItem.item = itemsDisplayed[obj];
        }

        public void OnDragEnd(GameObject obj)
        {
            if (mouseItem.hoverObj)
            {
                inventory.MoveItem(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObj]);
            }
            else
            {
                inventory.RemoveItem(itemsDisplayed[obj].item);
            }
            
            Destroy(mouseItem.obj);
            mouseItem.item = null;
        }

        public void OnDrag(GameObject obj)
        {
            if (mouseItem.obj != null)
            {
                mouseItem.obj.GetComponent<RectTransform>().position = mousePosition.ReadValue<Vector2>();
            }
        }
    }
    
    public class MouseItem
    {
        public GameObject obj;
        public InventorySlot item;
        public InventorySlot hoverItem;
        public GameObject hoverObj;
    }
}
