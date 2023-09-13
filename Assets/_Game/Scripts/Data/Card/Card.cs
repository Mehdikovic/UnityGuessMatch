using System;

public abstract class Card : ICloneable {
    private readonly int id;
    private int index;

    public Card(int id) {
        this.id = id;
    }

    public int GetID() => id;
    public int GetIndex() => index;

    public abstract object Clone();

    public T Clone<T>() where T : Card {
        return (T) Clone();
    }

    public bool IsSameSlot(Card other) {
        return Equals(other);
    }

    public override bool Equals(object obj) {
        return obj is Card slot && GetID() == slot.GetID();
    }

    public override int GetHashCode() {
        return GetID();
    }

    public void SetIndex(int index) {
        this.index = index;
    }
}
