using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameManagement {
    public class LoadSceneFader : MonoSingleton<LoadSceneFader> {
        [SerializeField] private Image image;

        private Color colorToFadeOut;
        private Color colorToFadeIn;
        private bool isInitialized = false;

        protected override void OnAwakeBefore() {
            Initialize();
        }

        public void Initialize() {
            if (isInitialized) { return; }
            isInitialized = true;
            
            colorToFadeIn = new(image.color.r, image.color.g, image.color.b, 1f);
            colorToFadeOut = new(image.color.r, image.color.g, image.color.b, 0f);
        }

        public void FadeIn() {
            image.color = colorToFadeIn;
        }

        public void FadeOut() {
            image.color = colorToFadeOut;
        }

        public IEnumerator FadeInCoroutine(float duration = 0.5f) {
            float timer = 0f;
            while (timer <= duration) {
                image.color = new(colorToFadeIn.r, colorToFadeIn.g, colorToFadeIn.b, timer / duration);
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
            image.color = colorToFadeIn;
        }

        public IEnumerator FadeOutCoroutine(float duration = 2f) {
            float timer = 0f;
            while (timer <= duration) {
                image.color = new(colorToFadeIn.r, colorToFadeIn.g, colorToFadeIn.b, 1f - timer / duration);
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
            image.color = colorToFadeOut;
        }
    }
}