using System;

public abstract class BaseSlot : ICloneable {
    private readonly int id;
    private int index;

    public BaseSlot(int id) {
        this.id = id;
    }

    public int GetID() => id;
    public int GetIndex() => index;

    public abstract object Clone();

    public bool IsSameSlot(BaseSlot other) {
        return Equals(other);
    }

    public override bool Equals(object obj) {
        return obj is BaseSlot slot && GetID() == slot.GetID();
    }

    public override int GetHashCode() {
        return GetID();
    }

    public void SetIndex(int index) {
        this.index = index;
    }
}
