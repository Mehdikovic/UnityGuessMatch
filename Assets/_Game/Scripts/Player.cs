using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoSingleton<Player> {
    static public event System.Action<List<BaseSlot>> OnAnySlotListReady;
    static public event UEventHandler<OnAnyMatchStateChangeEventArgs> OnAnyMatchStateBefore;
    static public event UEventHandler<OnAnyMatchStateChangeEventArgs> OnAnyMatchStateAfter;

    [field: SerializeField] public WordCollectionSO CharacterListSO { get; private set; }
    [field: SerializeField] public WordCollectionSO WordListSO { get; private set; }
    [field: SerializeField] public SpriteListSO SpriteListSO { get; private set; }

    private BaseSlot firstSlotSelected;
    private BaseSlot secondSlotSelected;

    public enum GameState {
        None,
        FirstSelected,
        SecondSelected,
    }

    private GameState gameState = GameState.None;

    public GameState GetState() => gameState;

    public bool HasFirstSlot() => firstSlotSelected != null;
    public bool HasSecondSlot() => secondSlotSelected != null;

    public BaseSlot GetFirstSlot() => firstSlotSelected;
    public BaseSlot GetSecondSlot() => secondSlotSelected;

    private void Start() {
        int length = 4;
        int id = 0;
        List<BaseSlot> slots = new();

        bool useSprites = PlayerPrefs.GetInt(SaveID.LoadSprites, 1) > 0;
        bool useWords = PlayerPrefs.GetInt(SaveID.LoadWords, 0) > 0;
        bool useChars = PlayerPrefs.GetInt(SaveID.LoadCharacter, 0) > 0;

        if (useSprites) {
            SpriteListSO.sprites.SafeForEach(sprite => {
                slots.Add(new SpriteSlot(++id, sprite));
            });
        }

        if (useWords) {
            WordListSO.words.SafeForEach(str => {
                slots.Add(new WordSlot(++id, str));
            });
        }

        if (useChars) {
            CharacterListSO.words.SafeForEach(chr => {
                slots.Add(new WordSlot(++id, chr));
            });
        }


        slots = Util.Shuffle(slots, Random.Range(0, int.MaxValue));
        slots = slots.Take(length).ToList();
        OnAnySlotListReady?.Invoke(slots);
    }

    public void Select(BaseSlot slot) {
        if (gameState == GameState.None && firstSlotSelected == null) {
            firstSlotSelected = slot;
            gameState = GameState.FirstSelected;
            return;
        }

        if (gameState == GameState.FirstSelected && firstSlotSelected != null) {
            if (ReferenceEquals(slot, firstSlotSelected)) {
                firstSlotSelected = null;
                gameState = GameState.None;
                return;
            }
        }

        if (gameState == GameState.FirstSelected && secondSlotSelected == null) {
            secondSlotSelected = slot;
            gameState = GameState.SecondSelected;
            StartCoroutine(CheckCOR());
        }
    }

    private IEnumerator CheckCOR() {
        int firstIndex = firstSlotSelected.GetIndex();
        int secondIndex = secondSlotSelected.GetIndex();

        int firstID = firstSlotSelected.GetID();
        int secondID = secondSlotSelected.GetID();

        OnAnyMatchStateBefore?.Invoke(this, new OnAnyMatchStateChangeEventArgs {
            result = firstID == secondID ? Result.Successful : Result.Failed,
            firstSlotIndex = firstIndex,
            secondSlotIndex = secondIndex
        });

        firstSlotSelected = null;
        secondSlotSelected = null;
        gameState = GameState.None;

        yield return new WaitForSeconds(.4f);

        OnAnyMatchStateAfter?.Invoke(this, new OnAnyMatchStateChangeEventArgs {
            result = firstID == secondID ? Result.Successful : Result.Failed,
            firstSlotIndex = firstIndex,
            secondSlotIndex = secondIndex
        });
    }

    public enum Result { Failed, Successful }

    public struct OnAnyMatchStateChangeEventArgs {
        public Result result;
        public int firstSlotIndex;
        public int secondSlotIndex;
    }
}
