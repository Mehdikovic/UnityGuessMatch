using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManagerUI : MonoBehaviour {

    private Dictionary<int, SlotUI> id2SlotUI;

    private void Awake() {
        id2SlotUI = new();

        Player.OnAnySlotListReady += Player_OnAnySlotListReady;
    }

    private void OnDestroy() {
        Player.OnAnySlotListReady -= Player_OnAnySlotListReady;
    }

    private void Player_OnAnySlotListReady(List<BaseSlot> slots) {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }

        int index = 0;

        slots.InsertRange(slots.Count, slots);
        slots = slots.ShuffleExtension(Random.Range(0, int.MaxValue));

        slots.SafeForEach(slot => {
            SlotUI slotUI = SlotUIFactory.Create(slot);
            slotUI.Inject(++index, this);
            slotUI.transform.SetParent(transform, false);
            slotUI.transform.ResetLocalTransform();
            id2SlotUI.Add(slotUI.GetIndex(), slotUI);
        });
    }
}
