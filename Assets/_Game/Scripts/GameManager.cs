using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager> {
    static public event Action OnGameStart;
    static public event Action<bool> OnGameStop;
    static public event Action OnTick;


    [SerializeField] private Timer timer;

    [SerializeField] private TimeSegment timeToEnd;

    [SerializeField] private int count = 8;
    [SerializeField] private int columnCount = 4;
    [SerializeField] private float width = 350;
    [SerializeField] private float height = 350;
    [SerializeField] private float xSpace = 50;
    [SerializeField] private float ySpace = 50;

    private int endTick = 0;
    private int currentTick = 0;

    public int GetCount() => count;
    public int GetColumnCount() => columnCount;
    public float GetWidth() => width;
    public float GetHeight() => height;
    public float GetXSpace() => xSpace;
    public float GetYSpace() => ySpace;

    public int GetTick() => currentTick;

    protected override void OnAwakeAfter() {
        timer.OnTick += Timer_OnTick;
        endTick = timeToEnd.ToTick();
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
