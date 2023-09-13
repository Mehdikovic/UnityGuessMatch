using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager> {
    static event Action OnGameStart;
    [SerializeField] private Timer timer;

    [SerializeField] private int count = 8;
    [SerializeField] private int columnCount = 4;
    [SerializeField] private float width = 350;
    [SerializeField] private float height = 350;
    [SerializeField] private float xSpace = 50;
    [SerializeField] private float ySpace = 50;


    public int GetCount() => count;
    public int GetColumnCount() => columnCount;
    public float GetWidth() => width;
    public float GetHeight() => height;
    public float GetXSpace() => xSpace;
    public float GetYSpace() => ySpace;

    static public void StartGame() {
        OnGameStart?.Invoke();
    }

#if UNITY_EDITOR
    private void OnValidate() {
        if (timer == null) { timer = GetComponentInChildren<Timer>(); }
    }
#endif
}
