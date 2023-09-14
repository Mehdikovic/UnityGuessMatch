using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement {
    public class LoadSceneManager : MonoSingleton<LoadSceneManager> {
        [SerializeField] private LoadSceneFader fader;

        private int currentSceneIndex = 0;
        private Coroutine sceneLoaderCO;

        protected override void OnAwakeAfter() {
            fader.Initialize();
        }

        private IEnumerator Start() {
            fader.FadeIn();
            sceneLoaderCO = StartCoroutine(fader.FadeOutCoroutine(0.3f));
            yield return sceneLoaderCO;
            sceneLoaderCO = null;
        }

        public bool IsFreeToLoad() { return sceneLoaderCO == null; }

        public void LoadScene(int sceneIndex) {
            if (sceneLoaderCO != null) return;
            sceneLoaderCO = StartCoroutine(LoadSceneAsyncByIndexOrName(sceneIndex, null));
        }

        public void LoadScene(string sceneName) {
            if (sceneLoaderCO != null) return;
            sceneLoaderCO = StartCoroutine(LoadSceneAsyncByIndexOrName(-1, sceneName));
        }

        public void Restart() {
            if (sceneLoaderCO != null) return;
            sceneLoaderCO = StartCoroutine(RestartSceneAsync());
        }

        public void LoadNext() {
            if (sceneLoaderCO != null) return;
            sceneLoaderCO = StartCoroutine(LoadNextSceneAsync());
        }

        public void LoadPrevious() {
            if (sceneLoaderCO != null) return;
            sceneLoaderCO = StartCoroutine(LoadPreviousSceneAsync());
        }

        private IEnumerator RestartSceneAsync() {
            yield return LoadSceneAsyncByIndexOrName(SceneManager.GetActiveScene().buildIndex, null);
        }

        private IEnumerator LoadNextSceneAsync() {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings) yield break;

            currentSceneIndex = nextSceneIndex;

            yield return LoadSceneAsyncByIndexOrName(currentSceneIndex, null);
        }

        private IEnumerator LoadPreviousSceneAsync() {
            int previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
            if (previousSceneIndex < 0) yield break;

            currentSceneIndex = previousSceneIndex;

            yield return LoadSceneAsyncByIndexOrName(currentSceneIndex, null);
        }

        private IEnumerator LoadSceneAsyncByIndexOrName(int sceneIndex, string sceneName) {
            yield return fader.FadeInCoroutine();

            AsyncOperation asyncOperation = sceneIndex >= 0
                ? SceneManager.LoadSceneAsync(sceneIndex)
                : SceneManager.LoadSceneAsync(sceneName);

            while (!asyncOperation.isDone) {
                yield return null;
            }

            yield return fader.FadeOutCoroutine();
            sceneLoaderCO = null;
        }

        private IEnumerator LoadSceneImmediateByIndexOrName(int sceneIndex, string sceneName) {
            if (sceneIndex >= 0) {
                SceneManager.LoadScene(sceneIndex);
            } else {
                SceneManager.LoadScene(sceneName);
            }

            yield return new WaitForSeconds(1f);
            yield return fader.FadeOutCoroutine(0.5f);
            sceneLoaderCO = null;
        }
    }
}