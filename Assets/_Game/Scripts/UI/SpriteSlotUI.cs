using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSlotUI : SlotUI {
    [SerializeField] private Image imageRenderer;

    public override void Setup(Card card) {
        base.Setup(card);
        
        if (card is CardSprite cardSprite) {
            imageRenderer.sprite = cardSprite.GetSprite();
        }
    }
}
