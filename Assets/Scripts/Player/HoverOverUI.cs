using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HoverOverUI : MonoBehaviour
{
    [SerializeField] private string UILayer;
    
    private PlayerControls input;
    private static InputAction
        mousePosition;

    private void OnEnable()
    {
        mousePosition = input.Player.MousePosition;
        
        mousePosition.Enable();
    }

    private void OnDisable()
    {
        mousePosition.Disable();
    }

    private void Awake()
    {
        input = new PlayerControls();
    }

    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    public bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaycastResults)
    {
        for (int i = 0; i < eventSystemRaycastResults.Count; i++)
        {
            RaycastResult curRaysAsResult = eventSystemRaycastResults[i];

            if (curRaysAsResult.gameObject.layer == LayerMask.NameToLayer(UILayer))
                return true;
        }

        return false;
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePosition.ReadValue<Vector2>();
        List<RaycastResult> raysAsResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysAsResults);
        return raysAsResults;
    }
}