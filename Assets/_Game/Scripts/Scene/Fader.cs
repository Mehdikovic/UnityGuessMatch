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

        public IEnumerator FadeInCoroutine(float duration = 0.4f) {
            Initialize();
            float timer = 0f;
            while (timer <= duration) {
                float alphaValue = Mathf.Max(timer / duration, image.color.a);
                image.color = new(colorToFadeIn.r, colorToFadeIn.g, colorToFadeIn.b, alphaValue);
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
            image.color = colorToFadeIn;
        }

        public IEnumerator FadeOutCoroutine(float duration = 0.4f) {
            Initialize();
            float timer = 0f;
            while (timer <= duration) {
                float alphaValue = Mathf.Min(1f - timer / duration, image.color.a);
                image.color = new(colorToFadeIn.r, colorToFadeIn.g, colorToFadeIn.b, alphaValue);
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
            image.color = colorToFadeOut;
        }

        private void Initialize() {
            if (isInitialized) { return; }
            isInitialized = true;

            colorToFadeIn = new(image.color.r, image.color.g, image.color.b, 1f);
            colorToFadeOut = new(image.color.r, image.color.g, image.color.b, 0f);
        }
    }
}