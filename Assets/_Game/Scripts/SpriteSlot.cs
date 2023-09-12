using UnityEngine;

public class SpriteSlot : BaseSlot {
    private readonly Sprite sprite;

    public SpriteSlot(Sprite sprite) {
        this.sprite = sprite;
    }

    public Sprite GetSprite() { return sprite; }

    public override bool IsSameSlot(BaseSlot other) {
        return other is SpriteSlot otherSpriteSlot && sprite == otherSpriteSlot.sprite;
    }
}
