using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Collections/WordCollection")]
public class WordCollectionSO : ScriptableObject {
    public WordSlot slotPrefab;
    public WordSlotUI slotUIPrefab;

    public List<string> words;
}
