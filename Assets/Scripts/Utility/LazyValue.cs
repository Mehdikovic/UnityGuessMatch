using System;

public class LazyValue<T> {
    private T value;
    private bool initialized = false;
    private readonly Func<T> initializer;

    public LazyValue(Func<T> initializer) {
        this.initializer = initializer;
    }

    public T Value => Get();

    public void ForceInit() {
        if (!initialized) {
            value = initializer();
            initialized = true;
        }
    }

    public T Get() {
        if (initialized) { return value; }
        ForceInit();
        return value;
    }
}