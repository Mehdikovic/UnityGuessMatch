using UnityEngine;

public class SpriteSlot : BaseSlot {
    private readonly Sprite sprite;

    public SpriteSlot(int id, Sprite sprite) : base(id) {
        this.sprite = sprite;
    }

    public override object Clone() {
        return new SpriteSlot(GetID(), sprite);
    }

    public Sprite GetSprite() { return sprite; }
}
