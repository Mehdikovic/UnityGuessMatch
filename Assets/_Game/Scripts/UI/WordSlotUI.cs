using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordSlotUI : SlotUI {
    [SerializeField] private TextMeshProUGUI textMeshUI;

    public override void Setup(Card card) {
        base.Setup(card);

        if (card is CardWord cardWord) {
            textMeshUI.text = cardWord.GetWord();
        }
    }
}
