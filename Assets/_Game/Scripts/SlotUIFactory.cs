using UnityEngine;

public class SlotUIFactory {

    static private readonly SlotUICollection slotUICollection;

    static SlotUIFactory() {
        slotUICollection = Resources.Load<SlotUICollection>(nameof(SlotUICollection));
    }

    static public SlotUI Create(Card slot) {
        return slot switch {
            CardSprite cardSprite => SlotUI.Create(slotUICollection.spriteSlotUIPrefab, cardSprite),
            CardWord cardWord => SlotUI.Create(slotUICollection.wordSlotUIPrefab, cardWord),
            _ => throw new System.Exception()
        };
    }
}
