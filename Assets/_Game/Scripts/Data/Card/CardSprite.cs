using UnityEngine;

public class CardSprite : Card {
    private readonly Sprite sprite;

    public CardSprite(int id, Sprite sprite) : base(id) {
        this.sprite = sprite;
    }

    public override object Clone() {
        return new CardSprite(GetID(), sprite);
    }

    public Sprite GetSprite() { return sprite; }
}
