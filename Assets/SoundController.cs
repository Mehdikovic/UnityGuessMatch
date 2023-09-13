using Sound;
using UnityEngine;

public class SoundController : MonoBehaviour {
    [SerializeField] private AudioClip winnerClip;
    [SerializeField] private AudioClip failureClip;

    [Space]
    [SerializeField] private AudioClip pairMatchClip;

    private readonly int playerMixerID = SoundManager.NameToHash("Player");

    private int winId = -1;

    private void Awake() {
        Player.OnAnyMatchStateBefore += Player_OnAnyMatchStateBefore;
        GameManager.OnGameStop += GameManager_OnGameStop;
    }

    private void OnDestroy() {
        Player.OnAnyMatchStateBefore -= Player_OnAnyMatchStateBefore;
        GameManager.OnGameStop -= GameManager_OnGameStop;

        DisableSounds();
    }

    private void Player_OnAnyMatchStateBefore(object sender, Player.OnAnyMatchStateChangeEventArgs args) {
        if (args.result != Player.Result.Successful) { return; }
        SoundManager.Instance.PlaySound(playerMixerID, pairMatchClip, 1f);
    }

    private void GameManager_OnGameStop(bool isWon) {
        AudioClip clip = isWon ? winnerClip : failureClip;
        winId = SoundManager.Instance.PlaySound(playerMixerID, clip, 1f);
    }

    private void DisableSounds() {
        if (winId > -1) {
            SoundManager.Instance.StopSound(winId, .2f);
        }
    }
}
