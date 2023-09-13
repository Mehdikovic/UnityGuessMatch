using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotManagerUI : MonoBehaviour {
    [SerializeField] private GridLayoutGroup grid;
    [HideInInspector][SerializeField] private RectTransform rectTransform;

    private Dictionary<int, SlotUI> id2SlotUI;

    private void Awake() {
        id2SlotUI = new();
        FillReferences();

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

        float width = GameManager.Instance.GetWidth();
        float height = GameManager.Instance.GetHeight();

        float xSpace = GameManager.Instance.GetXSpace();
        float ySpace = GameManager.Instance.GetYSpace();

        int columnCount = GameManager.Instance.GetColumnCount();

        grid.cellSize = new(width, height);
        grid.spacing = new(xSpace, ySpace);
        grid.constraintCount = columnCount;

        List<Card> allCards = new();

        cards.SafeForEach(card => {
            allCards.Add(card.Clone<Card>());
            allCards.Add(card.Clone<Card>());
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

    private void FillReferences() {
        if (grid == null) grid = GetComponent<GridLayoutGroup>();
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
    }

#if UNITY_EDITOR
    private void OnValidate() {
        FillReferences();
    }
#endif
}
