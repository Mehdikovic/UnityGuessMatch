using UnityEngine;


namespace Core.ObjectSpawner {
    public class PersistentObjectSpawner<T> : MonoBehaviour where T : MonoBehaviour {
        [SerializeField] private GameObject gameObjectPrefab;

        private static bool isInitialized = false;

        private void Awake() {
            if (isInitialized) return;
            isInitialized = true;

            if (gameObjectPrefab) DontDestroyOnLoad(Instantiate(gameObjectPrefab));
        }
    }
}