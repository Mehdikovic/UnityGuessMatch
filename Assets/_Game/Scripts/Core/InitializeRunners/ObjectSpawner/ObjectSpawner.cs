using UnityEngine;

namespace Core.ObjectSpawner {
    public class ObjectSpawner<T> : MonoBehaviour where T : MonoBehaviour {
        [SerializeField] private GameObject gameObjectPrefab;

        private static bool isInitialized = false;

        private void Awake() {
            if (isInitialized) return;
            isInitialized = true;

            if (gameObjectPrefab) Instantiate(gameObjectPrefab);
        }

        private void OnDestroy() {
            isInitialized = false;
        }
    }
}