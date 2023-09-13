using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoSingleton<Player> {
    static public event System.Action<List<Card>> OnAnyCardListReady;
    static public event UEventHandler<OnAnyMatchStateChangeEventArgs> OnAnyMatchStateBefore;
    static public event UEventHandler<OnAnyMatchStateChangeEventArgs> OnAnyMatchStateAfter;

    [field: SerializeField] public WordCollectionSO CharacterListSO { get; private set; }
    [field: SerializeField] public WordCollectionSO WordListSO { get; private set; }
    [field: SerializeField] public SpriteListSO SpriteListSO { get; private set; }

    private Card firstSelectedCard;
    private Card secondSelectedCard;

    public enum GameState {
        None,
        FirstSelected,
        SecondSelected,
    }

    private GameState gameState = GameState.None;

    public GameState GetState() => gameState;

    public bool HasFirstCard() => firstSelectedCard != null;
    public bool HasSecondCard() => secondSelectedCard != null;

    public Card GetFirstCard() => firstSelectedCard;
    public Card GetSecondCard() => secondSelectedCard;

    private void Start() {
        int length = GameManager.Instance.GetCount() / 2;
        int id = 0;
        List<Card> cards = new();

        bool useSprites = PlayerPrefs.GetInt(SaveID.LoadSprites, 1) > 0;
        bool useWords = PlayerPrefs.GetInt(SaveID.LoadWords, 0) > 0;
        bool useChars = PlayerPrefs.GetInt(SaveID.LoadCharacter, 0) > 0;

        if (useSprites) {
            SpriteListSO.sprites.SafeForEach(sprite => {
                cards.Add(new CardSprite(++id, sprite));
            });
        }

        if (useWords) {
            WordListSO.words.SafeForEach(str => {
                cards.Add(new CardWord(++id, str));
            });
        }

        if (useChars) {
            CharacterListSO.words.SafeForEach(chr => {
                cards.Add(new CardWord(++id, chr));
            });
        }


        cards = Util.Shuffle(cards, Random.Range(0, int.MaxValue));
        cards = cards.Take(length).ToList();
        OnAnyCardListReady?.Invoke(cards);
    }

    public void Select(Card slot) {
        if (gameState == GameState.None && firstSelectedCard == null) {
            firstSelectedCard = slot;
            gameState = GameState.FirstSelected;
            return;
        }

        if (gameState == GameState.FirstSelected && firstSelectedCard != null) {
            if (ReferenceEquals(slot, firstSelectedCard)) {
                firstSelectedCard = null;
                gameState = GameState.None;
                return;
            }
        }

        if (gameState == GameState.FirstSelected && secondSelectedCard == null) {
            secondSelectedCard = slot;
            gameState = GameState.SecondSelected;
            StartCoroutine(CheckCOR());
        }
    }

    private IEnumerator CheckCOR() {
        int firstIndex = firstSelectedCard.GetIndex();
        int secondIndex = secondSelectedCard.GetIndex();

        int firstID = firstSelectedCard.GetID();
        int secondID = secondSelectedCard.GetID();

        OnAnyMatchStateBefore?.Invoke(this, new OnAnyMatchStateChangeEventArgs {
            result = firstID == secondID ? Result.Successful : Result.Failed,
            firstCardIndex = firstIndex,
            secondCardIndex = secondIndex
        });

        firstSelectedCard = null;
        secondSelectedCard = null;
        gameState = GameState.None;

        yield return new WaitForSeconds(.4f);

        OnAnyMatchStateAfter?.Invoke(this, new OnAnyMatchStateChangeEventArgs {
            result = firstID == secondID ? Result.Successful : Result.Failed,
            firstCardIndex = firstIndex,
            secondCardIndex = secondIndex
        });
    }

    public enum Result { Failed, Successful }

    public struct OnAnyMatchStateChangeEventArgs {
        public Result result;
        public int firstCardIndex;
        public int secondCardIndex;
    }
}
