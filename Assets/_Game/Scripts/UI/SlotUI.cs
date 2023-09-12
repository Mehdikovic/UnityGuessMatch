using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour {
    static public SlotUI Create(SlotUI prefab, BaseSlot slot) {
        SlotUI slotUI = Instantiate(prefab);
        slotUI.Setup(slot);
        return slotUI;
    }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button button;
    [SerializeField] private float transitionDuration = 0.07f;

    [SerializeField] protected RectTransform hiddenSlot;
    [SerializeField] protected RectTransform shownSlot;

    private enum State { Hidden, Transition, Shown };
    private State state;
    private BaseSlot slot;
    private SlotManagerUI slotManagerUI;
    private int index = -1;

    public BaseSlot Slot() => slot;
    public int GetIndex() => index;

    public virtual void Setup(BaseSlot slot) {
        this.slot = slot;
    }

    public void Inject(int index, SlotManagerUI slotManagerUI) {
        this.index = index;
        this.slotManagerUI = slotManagerUI;
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

            // Send data to manager about this slot based on the current state
        });
    }

    private void DoFlipShow() {
        if (state == State.Transition) { return; }
        state = State.Transition;

        DoFlip(hiddenSlot, shownSlot, transitionDuration, State.Shown);
    }

    private void DoFlipHide() {
        if (state == State.Transition) { return; }
        state = State.Transition;

        DoFlip(shownSlot, hiddenSlot, transitionDuration, State.Hidden);
    }

    private void DoFlip(RectTransform from, RectTransform to, float duration, State toState) {
        Tween twenn = from.DOScaleX(0f, duration);

        twenn.onComplete = () => {
            from.gameObject.SetActive(false);
            to.gameObject.SetActive(true);

            to.localScale = to.localScale.WithX(0f);
            Tween t = to.DOScaleX(1f, duration);
            t.onComplete = () => state = toState;
        };
    }

    private void ShowUI() {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    private void HideUI() {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
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
