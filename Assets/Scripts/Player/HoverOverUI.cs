using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOverUI : MonoBehaviour
{
    [SerializeField] private string UILayer;

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
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysAsResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysAsResults);
        return raysAsResults;
    }
}