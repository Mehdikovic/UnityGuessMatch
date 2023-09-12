using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour, ISingleton<T> where T : MonoBehaviour {
    private static T instance;
    public static T Instance => instance;

    protected void Awake() {
        OnAwakeBefore();

        if (instance == null) instance = this as T;
        else if (instance != (this as T)) Destroy(gameObject);

        OnAwakeAfter();
    }

    protected virtual void OnAwakeBefore() { }
    protected virtual void OnAwakeAfter() { }

    protected void OnDestroy() {
        OnDestroyBefore();
        if (instance == (this as T)) { instance = null; }
        OnDestroyAfter();
    }

    protected virtual void OnDestroyAfter() { }
    protected virtual void OnDestroyBefore() { }
}
