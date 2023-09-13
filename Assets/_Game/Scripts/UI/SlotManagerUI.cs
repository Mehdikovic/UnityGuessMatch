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

        Player.OnAnyCardListReady += Player_OnAnyCardListReady;
        Player.OnAnyMatchStateBefore += Player_OnAnyMatchStateBefore;
        Player.OnAnyMatchStateAfter += Player_OnAnyMatchStateAfter;

    }

    private void OnDestroy() {
        Player.OnAnyCardListReady -= Player_OnAnyCardListReady;
        Player.OnAnyMatchStateBefore -= Player_OnAnyMatchStateBefore;
        Player.OnAnyMatchStateAfter -= Player_OnAnyMatchStateAfter;
    }

    public void SelectCard(SlotUI slotUI) {
        Player.Instance.Select(slotUI.GetCard());
    }

    private void Player_OnAnyCardListReady(List<Card> cards) {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }

        List<Card> allCards = new();

        cards.SafeForEach(card => {
            allCards.Add(card.Clone() as Card);
            allCards.Add(card.Clone() as Card);
        });

        allCards = allCards.ShuffleExtension(Random.Range(0, int.MaxValue));
        int index = 0;

        allCards.SafeForEach(card => {
            SlotUI slotUI = SlotUIFactory.Create(card);
            slotUI.Inject(++index, this);
            slotUI.name = $"SlotUI-id {slotUI.GetCard().GetID()}-index {slotUI.GetIndex()}";
            slotUI.transform.SetParent(transform, false);
            slotUI.transform.ResetLocalTransform();
            id2SlotUI.Add(slotUI.GetIndex(), slotUI);
        });
    }

    private void Player_OnAnyMatchStateBefore(object sender, Player.OnAnyMatchStateChangeEventArgs args) {
        id2SlotUI[args.firstCardIndex].DisableInteraction();
        id2SlotUI[args.secondCardIndex].DisableInteraction();
        if (args.result == Player.Result.Successful) {
            StartCoroutine(ShakeCOR());
        }
    }

    private void Player_OnAnyMatchStateAfter(object sender, Player.OnAnyMatchStateChangeEventArgs args) {
        if (args.result == Player.Result.Failed) {
            id2SlotUI[args.firstCardIndex].DoFlipHideForce(onSecondStageComplete: () => id2SlotUI[args.firstCardIndex].EnableInteraction());
            id2SlotUI[args.secondCardIndex].DoFlipHideForce(onSecondStageComplete: () => id2SlotUI[args.secondCardIndex].EnableInteraction());
        }
    }

    private IEnumerator ShakeCOR() {
        yield return new WaitForSeconds(.15f);
        rectTransform.DOShakeRotation(.2f);
        rectTransform.DOShakePosition(.2f);
    }
}
