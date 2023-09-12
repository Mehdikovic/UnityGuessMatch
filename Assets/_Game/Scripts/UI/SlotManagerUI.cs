using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManagerUI : MonoBehaviour {

    private Dictionary<int, SlotUI> id2SlotUI;
    private RectTransform rectTransform;

    private void Awake() {
        id2SlotUI = new();
        rectTransform = GetComponent<RectTransform>();

        Player.OnAnySlotListReady += Player_OnAnySlotListReady;
        Player.OnAnyMatchStateBefore += Player_OnAnyMatchStateBefore;
        Player.OnAnyMatchStateAfter += Player_OnAnyMatchStateAfter;

    }

    private void OnDestroy() {
        Player.OnAnySlotListReady -= Player_OnAnySlotListReady;
        Player.OnAnyMatchStateBefore -= Player_OnAnyMatchStateBefore;
        Player.OnAnyMatchStateAfter -= Player_OnAnyMatchStateAfter;
    }

    public void SelectSlot(SlotUI slotUI) {
        Player.Instance.Select(slotUI.GetSlot());
    }

    private void Player_OnAnySlotListReady(List<BaseSlot> slots) {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }

        List<BaseSlot> allSlots = new();

        slots.SafeForEach(slot => {
            allSlots.Add(slot.Clone() as BaseSlot);
            allSlots.Add(slot.Clone() as BaseSlot);
        });

        allSlots = allSlots.ShuffleExtension(Random.Range(0, int.MaxValue));
        int index = 0;

        allSlots.SafeForEach(slot => {
            SlotUI slotUI = SlotUIFactory.Create(slot);
            slotUI.Inject(++index, this);
            slotUI.name = $"SlotUI-id {slotUI.GetSlot().GetID()}-index {slotUI.GetIndex()}";
            slotUI.transform.SetParent(transform, false);
            slotUI.transform.ResetLocalTransform();
            id2SlotUI.Add(slotUI.GetIndex(), slotUI);
        });
    }

    private void Player_OnAnyMatchStateBefore(object sender, Player.OnAnyMatchStateChangeEventArgs args) {
        id2SlotUI[args.firstSlotIndex].DisableInteraction();
        id2SlotUI[args.secondSlotIndex].DisableInteraction();
        if (args.result == Player.Result.Successful) {
            StartCoroutine(ShakeCOR());
        }
    }

    private void Player_OnAnyMatchStateAfter(object sender, Player.OnAnyMatchStateChangeEventArgs args) {
        if (args.result == Player.Result.Failed) {
            id2SlotUI[args.firstSlotIndex].DoFlipHideForce(onSecondStageComplete: () => id2SlotUI[args.firstSlotIndex].EnableInteraction());
            id2SlotUI[args.secondSlotIndex].DoFlipHideForce(onSecondStageComplete: () => id2SlotUI[args.secondSlotIndex].EnableInteraction());
        } else {
            //CinemachineShake.Instance.Shake(2f, .2f);
            
        }
    }

    private IEnumerator ShakeCOR() {
        yield return new WaitForSeconds(.15f);
        rectTransform.DOShakeRotation(.2f);
        rectTransform.DOShakePosition(.2f);
    }
}
