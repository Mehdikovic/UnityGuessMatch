using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordSlotUI : SlotUI {
    [SerializeField] private TextMeshProUGUI textMeshUI;

    public override void Setup(BaseSlot slot) {
        base.Setup(slot);

        if (slot is WordSlot wordSlot) {
            textMeshUI.text = wordSlot.GetWord();
        }
    }
}
