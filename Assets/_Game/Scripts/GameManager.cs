using Core;
using ResourceItem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager> {
    static public event Action OnGameStart;
    static public event Action<bool> OnGameStop;
    static public event Action OnTick;

    [SerializeField] private Timer timer;
    [SerializeField] private LevelDataSO defaultLevelDataSO;

    private int endTick = 0;
    private int currentTick = 0;
    private LevelDataSO levelDataSO;
    private GameMode gameDifficulty;

    public int GetCount() => levelDataSO.count;
    public LevelDataSO GetLevelDataSO() => levelDataSO;
    public int GetTick() => currentTick;

    private Dictionary<int, LevelDataSO> id2LevelSO;

    protected override void OnAwakeAfter() {
        id2LevelSO = DatabaseUtility.Build<LevelDataSO>("LevelDataSO/");
        int idToLoad = PlayerPrefs.GetInt(SaveID.CardConfigID, 0);
        levelDataSO = idToLoad == 0 ? defaultLevelDataSO : id2LevelSO[idToLoad];

        timer.OnTick += Timer_OnTick;
        endTick = levelDataSO.timeToEnd.ToTick();
        currentTick = endTick;

        gameDifficulty = (GameMode) PlayerPrefs.GetInt(SaveID.GameDifficulty, 1);
    }

    private void Timer_OnTick() {
        currentTick--;
        OnTick?.Invoke();

        if (currentTick <= 0) {
            currentTick = 0;
            timer.Stop();
            EndGame(false);
        }
    }

    public void StartGame() {
        timer.SetTimerTick(GetTimerTick());
        timer.SetSingleShot(false);
        timer.StartTimer();
        OnGameStart?.Invoke();
    }

    public void EndGame(bool isWinnder) {
        timer.Stop();
        OnGameStop?.Invoke(isWinnder);
    }

    private float GetTimerTick() {
        return gameDifficulty switch {
            GameMode.Easy => 1.3f,
            GameMode.Normal => 1f,
            GameMode.Hard => .9f,
            GameMode.VeryHard => .6f,
            _ => 1f
        };
    }

#if UNITY_EDITOR
    private void OnValidate() {
        if (timer == null) { timer = GetComponentInChildren<Timer>(); }
    }
#endif
}
