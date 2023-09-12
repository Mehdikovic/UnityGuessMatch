using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Collections/SpriteCollection")]
public class SpriteListSO : ScriptableObject {
    public SpriteSlotUI slotUIPrefab;

    public List<Sprite> sprites;
}
