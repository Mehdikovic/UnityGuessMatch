using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameManagement {
    public class Fader : MonoSingleton<Fader> {
        [SerializeField] private Image image;

        private Color colorToFadeOut;
        private Color colorToFadeIn;
        private bool isInitialized = false;

        protected override void OnAwakeBefore() {
            Initialize();
        }

        public void FadeIn() {
            Initialize();
            image.color = colorToFadeIn;
        }

        public void FadeOut() {
            Initialize();
            image.color = colorToFadeOut;
        }

        public IEnumerator FadeInCoroutine(float duration = .5f) {
            Initialize();
            yield return FadeCOR(colorToFadeIn, 1f, duration);
        }

        public IEnumerator FadeOutCoroutine(float duration = .5f) {
            Initialize();
            yield return FadeCOR(colorToFadeOut, 0f, duration);
        }

        private IEnumerator FadeCOR(Color colorTarget, float alphaTarget, float duration) {
            float timer = 0f;
            float current = image.color.a;

            if (current == alphaTarget) { yield break; }

            while (timer <= duration) {
                float value = Mathf.Lerp(current, alphaTarget, timer / duration);
                image.color = new(colorTarget.r, colorTarget.g, colorTarget.b, value);
                timer += Time.unscaledDeltaTime;
                yield return null;
            }

            image.color = colorTarget;
        }

        private void Initialize() {
            if (isInitialized) { return; }
            isInitialized = true;

            colorToFadeIn = new(image.color.r, image.color.g, image.color.b, 1f);
            colorToFadeOut = new(image.color.r, image.color.g, image.color.b, 0f);
        }
    }
}