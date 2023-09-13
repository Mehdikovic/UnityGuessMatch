using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour {
    static public SlotUI Create(SlotUI prefab, Card card) {
        SlotUI slotUI = Instantiate(prefab);
        slotUI.Setup(card);
        return slotUI;
    }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button button;
    [SerializeField] private float transitionDuration = 0.07f;

    [SerializeField] protected RectTransform hiddenSlot;
    [SerializeField] protected RectTransform shownSlot;

    private enum State { Hidden, Transition, Shown };
    private State state;
    private Card card;
    private SlotManagerUI slotManagerUI;
    private int index = -1;

    public Card GetCard() => card;
    public int GetIndex() => index;

    public virtual void Setup(Card card) {
        this.card = card;
    }

    public void Inject(int index, SlotManagerUI slotManagerUI) {
        this.index = index;
        this.slotManagerUI = slotManagerUI;
        card.SetIndex(index);
    }

    private void Awake() {
        hiddenSlot.gameObject.SetActive(true);
        shownSlot.gameObject.SetActive(false);
        state = State.Hidden;

        button.onClick.AddListener(() => {
            if (state == State.Hidden) {
                DoFlipShow();
            } else {
                DoFlipHide();
            }

            slotManagerUI.SelectCard(this);
            // Send data to manager about this slot based on the current state
        });
    }

    public void DoFlipShow(Action onFirstStageComplete = null, Action onSecondStageComplete = null) {
        if (state == State.Transition) { return; }
        state = State.Transition;

        DoFlip(hiddenSlot, shownSlot, transitionDuration, State.Shown, onFirstStageComplete, onSecondStageComplete);
    }

    public void DoFlipHide(Action onFirstStageComplete = null, Action onSecondStageComplete = null) {
        if (state == State.Transition) { return; }
        state = State.Transition;

        DoFlip(shownSlot, hiddenSlot, transitionDuration, State.Hidden, onFirstStageComplete, onSecondStageComplete);
    }

    public void DoFlipHideForce(Action onFirstStageComplete = null, Action onSecondStageComplete = null) {
        DoFlip(shownSlot, hiddenSlot, transitionDuration, State.Hidden, onFirstStageComplete, onSecondStageComplete);
    }

    public void DisableInteraction() {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public void EnableInteraction() {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    private void DoFlip(
        RectTransform from,
        RectTransform to,
        float duration,
        State toState,
        Action onFirstStageComplete = null,
        Action onSecondStageComplete = null) {
        Tween twenn = from.DOScaleX(0f, duration);

        twenn.onComplete = () => {
            from.gameObject.SetActive(false);
            to.gameObject.SetActive(true);
            onFirstStageComplete?.Invoke();

            to.localScale = to.localScale.WithX(0f);
            Tween t = to.DOScaleX(1f, duration);
            t.onComplete = () => {
                state = toState;
                onSecondStageComplete?.Invoke();
            };
        };
    }

    private void ShowUI() {
        canvasGroup.alpha = 1f;
        EnableInteraction();
    }

    public void DoHide() {
        DisableInteraction();
        transform.DORotate(Vector3.forward * 180f, .2f, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
        transform.DOScale(0f, .6f).onComplete = () => canvasGroup.alpha = 0f;
    }

#if UNITY_EDITOR
    private void OnValidate() {
        if (!canvasGroup) canvasGroup = GetComponent<CanvasGroup>();
        if (!button) button = GetComponent<Button>();

        if (!hiddenSlot) {
            foreach (Transform t in transform) {
                if (t.name.StringContains("hidden")) {
                    hiddenSlot = t.GetComponent<RectTransform>();
                }
            }
        }

        if (!shownSlot) {
            foreach (Transform t in transform) {
                if (t.name.StringContains("shown")) {
                    shownSlot = t.GetComponent<RectTransform>();
                }
            }
        }
    }
#endif
}
