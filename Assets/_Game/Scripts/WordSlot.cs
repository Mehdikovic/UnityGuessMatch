using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSlot : BaseSlot {
    private string word;

    public override bool IsSameSlot(BaseSlot other) {
        return other is WordSlot otherSlot && word.StringEquals(otherSlot.word);
    }

    public string GetWord() => word;
}
