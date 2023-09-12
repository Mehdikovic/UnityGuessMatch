using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManagerUI : MonoBehaviour {

    private void Awake() {
        Player.OnAnySlotListReady += Player_OnAnySlotListReady;
    }

    private void OnDestroy() {
        Player.OnAnySlotListReady -= Player_OnAnySlotListReady;
    }

    private void Player_OnAnySlotListReady(List<BaseSlot> slots) {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }

        slots.InsertRange(slots.Count, slots);
        slots = slots.ShuffleExtension(Random.Range(0, int.MaxValue));

        slots.SafeForEach(slot => {
            SlotUI slotUI = SlotUIFactory.Create(slot);
            slotUI.transform.SetParent(transform, false);
            slotUI.transform.ResetLocalTransform();
        });
    }
}
