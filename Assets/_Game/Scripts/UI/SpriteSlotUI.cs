using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSlotUI : SlotUI {
    [SerializeField] private Image imageRenderer;

    public override void Setup(BaseSlot slot) {
        base.Setup(slot);
        
        if (slot is SpriteSlot spriteSlot) {
            imageRenderer.sprite = spriteSlot.GetSprite();
        }
    }
}
