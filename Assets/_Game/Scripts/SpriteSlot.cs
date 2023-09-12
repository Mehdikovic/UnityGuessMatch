using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSlot : BaseSlot {
    private Sprite sprite;

    public override bool IsSameSlot(BaseSlot other) {
        return other is SpriteSlot otherSpriteSlot && sprite == otherSpriteSlot.sprite;
    }

    public Sprite GetSprite() { return sprite; }
}
