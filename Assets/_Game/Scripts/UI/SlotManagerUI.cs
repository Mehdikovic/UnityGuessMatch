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

        ConfigGrid(GameManager.Instance.GetLevelDataSO());

        List<Card> allCards = new();

        cards.SafeForEach(card => {
            allCards.Add(card.Clone<Card>());
            allCards.Add(card.Clone<Card>());
        });

        allCards = allCards.ShuffleExtension(UnityEngine.Random.Range(0, int.MaxValue));
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

    private void ConfigGrid(LevelDataSO levelSO) {
        grid.cellSize = new(levelSO.width, levelSO.height);
        grid.spacing = new(levelSO.xSpace, levelSO.ySpace);
        grid.constraintCount = levelSO.columnCount;
    }

#if UNITY_EDITOR
    [Header("EDIT MDOE")]
    public LevelDataSO levelSO;
    public bool evaluateInEditMode = true;
    private void OnValidate() {
        FillReferences();

        if (Application.isPlaying) { return; }
        if (!evaluateInEditMode) { return; }

        LoadGridData();
    }

    IEnumerator DestroyCOR(GameObject go) {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }

    [ContextMenu("LoadGridData")]
    private void LoadGridData() {
        ConfigGrid(levelSO);

        Transform template = transform.GetChild(0);
        template.gameObject.SetActive(false);

        foreach (Transform child in transform) {
            if (child == template) { continue; }
            StartCoroutine(DestroyCOR(child.gameObject));
        }

        int length = levelSO.count;

        for (int i = 0; i < length; i++) {
            Transform t = Instantiate(template, transform);
            t.ResetLocalTransform();
            t.gameObject.SetActive(true);
        }
    }

    [ContextMenu("SaveGridData")]
    private void SaveGridData() {
        levelSO.width = grid.cellSize.x;
        levelSO.height = grid.cellSize.y;
        levelSO.xSpace = grid.spacing.x;
        levelSO.ySpace = grid.spacing.y;
        levelSO.columnCount = grid.constraintCount;
    }
#endif
}
