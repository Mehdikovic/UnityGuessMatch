using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoSingleton<Player> {
    [field: SerializeField] public WordCollectionSO CharacterListSO { get; private set; }
    [field: SerializeField] public WordCollectionSO WordListSO { get; private set; }
    [field: SerializeField] public SpriteListSO SpriteListSO { get; private set; }

    static public event System.Action<List<BaseSlot>> OnAnySlotListReady;

    private void Start() {
        int length = 4;
        List<BaseSlot> slots = new();

        bool useSprites = PlayerPrefs.GetInt(SaveID.LoadSprites, 1) > 0;
        bool useWords = PlayerPrefs.GetInt(SaveID.LoadWords, 1) > 0;
        bool useChars = PlayerPrefs.GetInt(SaveID.LoadCharacter, 1) > 0;

        if (useSprites) {
            SpriteListSO.sprites.SafeForEach(sprite => {
                slots.Add(new SpriteSlot(sprite));
            });
        }

        if (useWords) {
            WordListSO.words.SafeForEach(str => {
                slots.Add(new WordSlot(str));
            });
        }

        if (useChars) {
            CharacterListSO.words.SafeForEach(chr => {
                slots.Add(new WordSlot(chr));
            });
        }


        slots = Util.Shuffle(slots, Random.Range(0, int.MaxValue));
        slots = slots.Take(length).ToList();
        OnAnySlotListReady?.Invoke(slots);
    }
}
