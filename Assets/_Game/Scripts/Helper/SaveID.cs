using UnityEngine;

public class SaveID : MonoBehaviour {
    static public string LoadSprites { get; private set; } = nameof(LoadSprites);
    static public string LoadWords { get; private set; } = nameof(LoadWords);
    static public string LoadCharacter { get; private set; } = nameof(LoadCharacter);
    static public string CardConfigID { get; private set; } = nameof(CardConfigID);
    static public string AllTimeScore { get; private set; } = nameof(AllTimeScore);
    static public string GameDifficulty { get; private set; } = nameof(GameDifficulty);
    static public string Music { get; private set; } = nameof(Music);
    static public string Sound { get; private set; } = nameof(Sound);
}
