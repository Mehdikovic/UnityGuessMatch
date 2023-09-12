using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EventSystemUtility {
    private readonly List<RaycastResult> results;
    private readonly EventSystem eventSystem;
    private PointerEventData eventData;

    public EventSystemUtility(EventSystem eventSystem) {
        this.eventSystem = eventSystem;
        results = new List<RaycastResult>();
    }

    public bool IsPointerOverUIObject(Vector2 position) {
        if (!(0 < position.x && position.x < Screen.width && 0 < position.y && position.y < Screen.height)) { return true; }

        results.Clear();
        eventData = new(eventSystem) { position = position };
        eventSystem.RaycastAll(eventData, results);
        return results.Count(hit => hit.sortingOrder >= 0) > 0;
    }

    public bool IsPointerOverUIObject() {
        Vector2 position = Mouse.current.position.ReadValue();
        
        if (!(0 < position.x && position.x < Screen.width && 0 < position.y && position.y < Screen.height)) { return true; }

        results.Clear();
        eventData = new(eventSystem) { position = position };
        eventSystem.RaycastAll(eventData, results);
        return results.Count(hit => hit.sortingOrder >= 0) > 0;
    }
}
