using UnityEngine;

public class MonoSingletonFinder<T> : MonoSingleton<T> where T : MonoBehaviour {
    private static T instance;

    new public static T Instance {
        get {
            return instance ??= FindObjectOfType<T>();
        }
    }
}
