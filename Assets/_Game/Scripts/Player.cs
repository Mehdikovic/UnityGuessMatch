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

        SpriteListSO.sprites.SafeForEach(sp => {
            slots.Add(new SpriteSlot(sp));
        });

        slots = Util.Shuffle(slots, Random.Range(0, int.MaxValue));

        slots = slots.Take(length).ToList();

        OnAnySlotListReady?.Invoke(slots);
    }
}
