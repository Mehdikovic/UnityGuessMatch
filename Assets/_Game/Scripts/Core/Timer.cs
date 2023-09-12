using System;
using System.Collections;
using UnityEngine;

namespace Core {
    public class Timer : MonoBehaviour {
        [Header("Timer Setting")]
        [Tooltip("In seconds")]
        [SerializeField] private float timerTick;
        [Tooltip("Tick only one time")]
        [SerializeField] private bool singleShot;

        [Space]
        [Tooltip("Stop ticking while game paused")]
        [SerializeField] private bool stopTickOnGamePause;

        [Space]
        [SerializeField] private bool startOnAwake;
        [SerializeField] private bool startOnStart;

        public event Action OnTick;

        private WaitForSecondsRealtime waitForSecond;
        private Coroutine timerCOR;

        private void Awake() {
            if (startOnAwake) { StartTimer(); }
        }

        private void Start() {
            if (startOnStart && !startOnAwake) { StartTimer(); }
        }

        public void StartTimer() {
            this.StopCOR(ref timerCOR);
            waitForSecond = new(timerTick);
            timerCOR = StartCoroutine(TimerAsyc());
        }

        public void StartTimer(float newTimerTick) {
            this.StopCOR(ref timerCOR);
            waitForSecond = new(newTimerTick);
            timerCOR = StartCoroutine(TimerAsyc());
        }

        private IEnumerator TimerAsyc() {
            bool isSingleShot = singleShot;

            while (true) {
                if (stopTickOnGamePause && Time.timeScale == 0f) {
                    yield return null;
                } else {
                    yield return waitForSecond;
                    OnTick?.Invoke();
                    if (isSingleShot) { yield break; }
                }
            }
        }
    }
}