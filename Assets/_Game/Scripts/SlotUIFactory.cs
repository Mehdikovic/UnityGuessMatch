using UnityEngine;

public class SlotUIFactory {

    static private readonly SlotUICollection slotUICollection;

    static SlotUIFactory() {
        slotUICollection = Resources.Load<SlotUICollection>(nameof(SlotUICollection));
    }

    static public SlotUI Create(BaseSlot slot) {
        return slot switch {
            SpriteSlot spriteSlot => SlotUI.Create(slotUICollection.spriteSlotUIPrefab, spriteSlot),
            WordSlot wordSlot => SlotUI.Create(slotUICollection.wordSlotUIPrefab, wordSlot),
            _ => throw new System.Exception()
        };
    }
}
