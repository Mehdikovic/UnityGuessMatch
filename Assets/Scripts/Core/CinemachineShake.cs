using Cinemachine;
using UnityEngine;

public class CinemachineShake : MonoSingleton<CinemachineShake> {
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private CinemachineBasicMultiChannelPerlin perlinNoise;

    private float timerMax;
    private float timer;
    private float startedIntencity;

    protected override void OnAwakeAfter() {
        perlinNoise = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start() {
        perlinNoise.m_AmplitudeGain = 0f;
        startedIntencity = 0f;
    }

    private void Update() {
        if (timer < timerMax) {
            float amplitude = Mathf.Lerp(startedIntencity, 0f, timer / timerMax);
            perlinNoise.m_AmplitudeGain = amplitude;
        }

        if (timer > timerMax) {
            perlinNoise.m_AmplitudeGain = 0f;
        }

        timer += Time.deltaTime;
    }

    public void Shake(float intencity, float time) {
        timerMax = time;
        timer = 0;
        startedIntencity = intencity;
        perlinNoise.m_AmplitudeGain = intencity;
    }
}
