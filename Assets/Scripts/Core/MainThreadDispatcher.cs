using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MainThreadDispatcher : MonoSingleton<MainThreadDispatcher> {
    private readonly Queue<Action> dispatcherQueue = new();

    private void LateUpdate() {
        lock (dispatcherQueue) {
            while (dispatcherQueue.Count > 0) {
                Action action = dispatcherQueue.Dequeue();
                action.Invoke();
            }
        }
    }

    public void Enqueue(IEnumerator action) {
        lock (dispatcherQueue) {
            dispatcherQueue.Enqueue(() => {
                StartCoroutine(action);
            });
        }
    }

    public void Enqueue(Action action) {
        Enqueue(ActionAdaptor(action));
    }

    public Task EnqueueAsync(Action action) {
        TaskCompletionSource<bool> tsc = new();

        void LocalAdaptor() {
            try {
                action();
                tsc.TrySetResult(true);
            } catch (Exception ex) {
                tsc.SetException(ex);
            }
        }

        Enqueue(ActionAdaptor(LocalAdaptor));
        return tsc.Task;
    }

    public Task EnqueueAsync(IEnumerator action) {
        TaskCompletionSource<bool> tsc = new();

        IEnumerator LocalCoroutine() {
            yield return StartCoroutine(action);
            try {
                tsc.TrySetResult(true);
            } catch (Exception ex) {
                tsc.SetException(ex);
            }
        }

        Enqueue(LocalCoroutine());
        return tsc.Task;
    }

    public void Enable() {
        enabled = true;
    }

    public void Disable() {
        enabled = false;
    }

    private IEnumerator ActionAdaptor(Action action) {
        action();
        yield return null;
    }
}
