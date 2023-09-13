using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    public static class UIUtil {
        public const float SHOW_FIRST_DURATION = 0.2f;
        public const float SHOW_SECOND_DURATION = 0.1f;

        public const float HIDE_FIRST_DURATION = 0.1f;
        public const float HIDE_SECOND_DURATION = 0.2f;

        public const float SHOW_SCALE_TARGET = 1.1f;
        public const float HIDE_SCALE_TARGET = 1.1f;

        static public void ShowWindowImmediate(RectTransform rectTransform, Action onComplete = null) {
            rectTransform.localScale = new Vector3(1f, 1f, 1f);
            rectTransform.gameObject.SetActive(true);
            onComplete?.Invoke();
        }

        static public void ShowWindow(RectTransform rectTransform, Action onComplete = null) {
            rectTransform.localScale = new Vector3(0f, 0f, 1f);
            rectTransform.gameObject.SetActive(true);

            bool prev = DOTween.defaultTimeScaleIndependent;
            DOTween.defaultTimeScaleIndependent = true;

            Sequence initial = DOTween.Sequence();
            initial.onComplete = () => {
                Sequence second = DOTween.Sequence();
                second.onComplete = () => { DOTween.defaultTimeScaleIndependent = prev; onComplete?.Invoke(); };
                second.Join(rectTransform.DOScaleX(1f, SHOW_SECOND_DURATION));
                second.Join(rectTransform.DOScaleY(1f, SHOW_SECOND_DURATION));
            };
            initial.Join(rectTransform.DOScaleX(SHOW_SCALE_TARGET, SHOW_FIRST_DURATION));
            initial.Join(rectTransform.DOScaleY(SHOW_SCALE_TARGET, SHOW_FIRST_DURATION));
        }

        static public void HideWindowImmediate(RectTransform rectTransform, Action onComplete = null) {
            rectTransform.gameObject.SetActive(false);
            rectTransform.localScale = new Vector3(0f, 0f, 1f);
            onComplete?.Invoke();
        }

        static public void HideWindow(RectTransform rectTransform, Action onComplete = null) {
            rectTransform.localScale = new Vector3(1f, 1f, 1f);

            bool prev = DOTween.defaultTimeScaleIndependent;
            DOTween.defaultTimeScaleIndependent = true;

            Sequence initial = DOTween.Sequence();
            initial.onComplete = () => {
                Sequence second = DOTween.Sequence();
                second.onComplete = () => {
                    onComplete?.Invoke();
                    rectTransform.gameObject.SetActive(false);
                    rectTransform.localScale = new Vector3(0f, 0f, 1f);
                    DOTween.defaultTimeScaleIndependent = prev;
                };
                second.Join(rectTransform.DOScaleX(0f, HIDE_SECOND_DURATION));
                second.Join(rectTransform.DOScaleY(0f, HIDE_SECOND_DURATION));
            };
            initial.Join(rectTransform.DOScaleX(HIDE_SCALE_TARGET, HIDE_FIRST_DURATION));
            initial.Join(rectTransform.DOScaleY(HIDE_SCALE_TARGET, HIDE_FIRST_DURATION));
        }

        static public void TransitionTo(WindowUI from, WindowUI to) {
            if (!from.IsActive || to.IsActive) { return; }

            from.BeginHide();

            RectTransform fromRect = from.GetRectWindow();
            RectTransform toRect = to.GetRectWindow();

            HideWindow(fromRect, onComplete: () => {
                from.EndHide();
                to.BeginShow();

                ShowWindow(toRect, onComplete: () => {
                    to.EndShow();
                });
            });
        }

        static public void TransitionShow(WindowUI to) {
            if (to.IsActive) { return; }

            RectTransform toRect = to.GetRectWindow();
            to.BeginShow();

            ShowWindow(toRect, onComplete: () => {
                to.EndShow();
            });
        }

        static public void TransitionHide(WindowUI from) {
            if (!from.IsActive) { return; }

            RectTransform toRect = from.GetRectWindow();
            from.BeginHide();

            HideWindow(toRect, onComplete: () => {
                from.EndHide();
            });
        }

        static public void ImmediateTo(WindowUI from, WindowUI to) {
            if (!from.IsActive || to.IsActive) { return; }

            from.BeginHide();

            RectTransform fromRect = from.GetRectWindow();
            RectTransform toRect = to.GetRectWindow();

            HideWindowImmediate(fromRect, onComplete: () => {
                from.EndHide();
                to.BeginShow();

                ShowWindowImmediate(toRect, onComplete: () => {
                    to.EndShow();
                });
            });
        }

        static public void ImmediateShow(WindowUI to) {
            if (to.IsActive) { return; }

            RectTransform toRect = to.GetRectWindow();
            to.BeginShow();

            ShowWindowImmediate(toRect, onComplete: () => {
                to.EndShow();
            });
        }

        static public void ImmediateHide(WindowUI from) {
            if (!from.IsActive) { return; }

            RectTransform toRect = from.GetRectWindow();
            from.BeginHide();

            HideWindowImmediate(toRect, onComplete: () => {
                from.EndHide();
            });
        }

        static public void UIElementsActivation(bool isActive, IEnumerable<Behaviour> elements) {
            elements.SafeForEach((e) => e.enabled = isActive);
        }
    }
}