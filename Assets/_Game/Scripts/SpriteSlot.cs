using UnityEngine;

public class SpriteSlot : BaseSlot {
    private readonly Sprite sprite;

    public SpriteSlot(int id, Sprite sprite) : base(id) {
        this.sprite = sprite;
    }

    public Sprite GetSprite() { return sprite; }
}
