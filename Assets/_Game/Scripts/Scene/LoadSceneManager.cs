using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement {
    public class LoadSceneManager : MonoSingleton<LoadSceneManager> {
        [SerializeField] private float waitTimeBetweenSceneLoad = 1f;
        [SerializeField] private LoadSceneFader fader;

        private int currentSceneIndex = 0;
        private Coroutine sceneLoaderCO;
        private bool force;
        private WaitForSeconds waitTime;

        protected override void OnAwakeAfter() {
            waitTime = new WaitForSeconds(waitTimeBetweenSceneLoad);
            fader.FadeIn();
        }

        private void Start() {
            sceneLoaderCO = StartCoroutine(StartInitialFade());
        }

        public bool CanLoadScene() { return force || sceneLoaderCO == null; }

        public LoadSceneManager Force() {
            force = true;
            return this;
        }

        public void LoadScene(int sceneIndex) {
            if (!CanContinueLoadingScene()) { return; }
            sceneLoaderCO = StartCoroutine(LoadSceneAsyncByIndexOrName(sceneIndex, null));
        }

        public void LoadScene(string sceneName) {
            if (!CanContinueLoadingScene()) { return; }
            sceneLoaderCO = StartCoroutine(LoadSceneAsyncByIndexOrName(-1, sceneName));
        }

        public void Restart() {
            if (!CanContinueLoadingScene()) { return; }
            sceneLoaderCO = StartCoroutine(RestartSceneAsync());
        }

        public void LoadNext() {
            if (!CanContinueLoadingScene()) { return; }
            sceneLoaderCO = StartCoroutine(LoadNextSceneAsync());
        }

        public void LoadPrevious() {
            if (!CanContinueLoadingScene()) { return; }
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

            yield return waitTime;
            yield return fader.FadeOutCoroutine();
            sceneLoaderCO = null;
        }

        private IEnumerator LoadSceneImmediateByIndexOrName(int sceneIndex, string sceneName) {
            if (sceneIndex >= 0) {
                SceneManager.LoadScene(sceneIndex);
            } else {
                SceneManager.LoadScene(sceneName);
            }

            yield return waitTime;
            yield return fader.FadeOutCoroutine();
            sceneLoaderCO = null;
        }

        private bool CanContinueLoadingScene() {
            if (force) {
                this.StopCOR(ref sceneLoaderCO);
                force = false;
                return true;
            }

            return sceneLoaderCO == null;
        }

        private IEnumerator StartInitialFade() {
            yield return waitTime;
            yield return fader.FadeOutCoroutine();
            sceneLoaderCO = null;
        }
    }
}