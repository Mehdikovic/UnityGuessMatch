using System.Collections;
using UnityEngine;

public static class MonoBehaviourExtension {
    // SECTION Coroutine
    static public void StopMonoCOR(MonoBehaviour mono, ref Coroutine coroutine) {
        if (coroutine != null) {
            mono.StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    static public void StopCOR(this MonoBehaviour mono, ref Coroutine coroutine) {
        StopMonoCOR(mono, ref coroutine);
    }

    static public void Kill(this Coroutine _, MonoBehaviour mono, ref Coroutine coroutine) {
        StopMonoCOR(mono, ref coroutine);
    }

    static public void RestartCOR(this MonoBehaviour mono, ref Coroutine coroutine, IEnumerator routine) {
        StopMonoCOR(mono, ref coroutine);
        mono.StartCoroutine(routine);
    }
    // END OF SECTION


    // SECTION GetComponentInParent
    static public bool TryGetComponentInParent<T>(this GameObject unityGameObject, out T component) where T : class {
        component = unityGameObject.GetComponentInParent(typeof(T)) as T;
        return component != null;
    }

    static public bool TryGetComponentInParent<T>(this Behaviour mono, out T component) where T : class {
        component = mono.GetComponentInParent(typeof(T)) as T;
        return component != null;
    }

    static public bool TryGetComponentInParent<T>(this GameObject unityGameObject, out T component, bool includeInactive) where T : class {
        component = unityGameObject.GetComponentInParent(typeof(T), includeInactive) as T;
        return component != null;
    }

    static public bool TryGetComponentInParent<T>(this Behaviour mono, out T component, bool includeInactive) where T : class {
        component = mono.GetComponentInParent(typeof(T), includeInactive) as T;
        return component != null;
    }

    static public bool TryGetComponentOrFirstParent<T>(this GameObject unityGameObject, out T component) where T : class {
        if (unityGameObject.TryGetComponent(typeof(T), out Component foundComponent)) {
            component = foundComponent as T;
            return true;
        }

        if (unityGameObject.transform.parent == null) { component = null; return false; }

        if (unityGameObject.transform.parent.TryGetComponent(typeof(T), out foundComponent)) {
            component = foundComponent as T;
            return true;
        }

        component = null;
        return false;
    }

    static public bool TryGetComponentOrFirstParent<T>(this Behaviour mono, out T component) where T : class {
        if (mono.TryGetComponent(typeof(T), out Component foundComponent)) {
            component = foundComponent as T;
            return true;
        }

        if (mono.transform.parent == null) { component = null; return false; }

        if (mono.transform.parent.TryGetComponent(typeof(T), out foundComponent)) {
            component = foundComponent as T;
            return true;
        }

        component = null;
        return false;
    }

    static public T GetComponentOrFirstParent<T>(this GameObject unityGameObject) where T : class {
        if (unityGameObject.TryGetComponent(typeof(T), out Component foundComponent)) {
            return foundComponent as T;
        }

        if (unityGameObject.transform.parent == null) { return null; }

        if (unityGameObject.transform.parent.TryGetComponent(typeof(T), out foundComponent)) {
            return foundComponent as T;
        }

        return null;
    }

    static public T GetComponentOrFirstParent<T>(this Behaviour mono) where T : class {
        if (mono.TryGetComponent(typeof(T), out Component foundComponent)) {
            return foundComponent as T;
        }

        if (mono.transform.parent == null) { return null; }

        if (mono.transform.parent.TryGetComponent(typeof(T), out foundComponent)) {
            return foundComponent as T;
        }

        return null;
    }
    // END OF SECTION
}
