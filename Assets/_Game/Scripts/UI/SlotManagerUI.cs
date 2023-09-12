using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManagerUI : MonoBehaviour {

    private void Awake() {
        Player.OnAnySlotListReady += Player_OnAnySlotListReady;

        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }
    }

    private void OnDestroy() {
        Player.OnAnySlotListReady -= Player_OnAnySlotListReady;
    }

    private void Player_OnAnySlotListReady(List<BaseSlot> slots) {
        Debug.Log(slots.Count);
        slots.InsertRange(slots.Count, slots);
        Debug.Log(slots.Count);

        slots.SafeForEach(slot => {
            SlotUI slotUI = SlotUIFactory.Create(slot);
            slotUI.transform.SetParent(transform, false);
            slotUI.transform.ResetLocalTransform();
        });
    }
}
