using System;
using UnityEngine;
using System.Collections.Generic;

namespace UI {
    public class WindowUI : MonoBehaviour {
        [Header("Root Parent for the Window")]
        [SerializeField] protected RectTransform selfRectTransform = null;
        [SerializeField] private UIElementTag[] uiElements;

        public event Action OnHide;
        public event Action OnShow;
        public event Action OnClose;

        public bool IsActive { get; private set; }
        public bool IsVisible => IsActive;

        protected virtual void Awake() {
            selfRectTransform.pivot = new Vector2(0, 1);
            selfRectTransform.gameObject.SetActive(false);
        }

        public UIElementTag[] GetUIElements() => uiElements;
        public RectTransform GetRectWindow() => selfRectTransform;

        public void BeginHide() {
            IsActive = false;
            DisableUIElements();
            BeginHideCallback();
        }

        public void EndHide() {
            EndHideCallback();
            OnHide?.Invoke();
        }

        public void BeginShow() {
            IsActive = true;
            BeginShowCallback();
        }

        public void EndShow() {
            EnableUIElements();
            EndShowCallback();
            OnShow?.Invoke();
        }

        protected virtual void BeginHideCallback() { }
        protected virtual void EndHideCallback() { }
        protected virtual void BeginShowCallback() { }
        protected virtual void EndShowCallback() { }
        protected void EnableUIElements() { UIElementsActivation(true, uiElements); }
        protected void DisableUIElements() { UIElementsActivation(false, uiElements); }

        protected virtual void OnCloseInvoke() {
            OnClose?.Invoke();
        }

        static public void UIElementsActivation(bool isActive, IEnumerable<UIElementTag> elements) {
            elements.SafeForEach((e) => e.enabled = isActive);
        }

#if UNITY_EDITOR
        protected virtual void OnValidate() {
            uiElements = selfRectTransform.GetComponentsInChildren<UIElementTag>(true);
        }
#endif
    }
}