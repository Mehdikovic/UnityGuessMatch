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
        timer.StartTimer();
        OnGameStart?.Invoke();
    }

    public void EndGame(bool isWinnder) {
        Debug.Log("Win: " + isWinnder);
        timer.Stop();
        OnGameStop?.Invoke(isWinnder);
    }

#if UNITY_EDITOR
    private void OnValidate() {
        if (timer == null) { timer = GetComponentInChildren<Timer>(); }
    }
#endif
}
