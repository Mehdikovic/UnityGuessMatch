public abstract class BaseSlot {
    private readonly int id;

    public BaseSlot(int id) {
        this.id = id;
    }

    public int GetID() => id;

    public bool IsSameSlot(BaseSlot other) {
        return Equals(other);
    }

    public override bool Equals(object obj) {
        return obj is BaseSlot slot && GetID() == slot.GetID();
    }

    public override int GetHashCode() {
        return GetID();
    }
}
