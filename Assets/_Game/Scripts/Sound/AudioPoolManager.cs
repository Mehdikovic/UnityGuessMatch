using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Audio;

namespace Sound {
    public struct SoundData {
        public int id;
        public float volume;
        public bool highPriority;
        public AudioSource audioSource;
        public Coroutine playCOR;
    }

    [DisallowMultipleComponent]
    public class AudioPoolManager : MonoBehaviour {
        [field: SerializeField] public bool RunInPause { get; set; } = true;

        private Dictionary<int, SoundData> id2ActiveAudio;
        private Dictionary<int, Coroutine> id2VolumeFadeCoroutine;
        private ObjectPool<AudioSource> audioSourcePool;
        private int id = 0;
        private int highPriorityCount = 0;
        private int maxPoolSize;
        private int maxHighPriorityReserveCount;

        private void Awake() {
            maxPoolSize = 16;
            maxHighPriorityReserveCount = 2;

            id2ActiveAudio = new();
            id2VolumeFadeCoroutine = new();
            int index = 0;

            AudioSource Create() {
                GameObject go = new($"Audio Source_{++index}");
                go.transform.parent = transform;
                return go.AddComponent<AudioSource>();
            }

            audioSourcePool = new ObjectPool<AudioSource>(
                createFunc: Create,
                actionOnGet: a => a.gameObject.SetActive(true),
                actionOnRelease: a => a.gameObject.SetActive(false),
                actionOnDestroy: a => Destroy(a.gameObject),
                maxSize: maxPoolSize,
                collectionCheck: false
            );

            AudioSource[] sources = new AudioSource[maxPoolSize];

            for (int i = 0; i < maxPoolSize; i++) {
                sources[i] = audioSourcePool.Get();
            }

            for (int i = 0; i < maxPoolSize; i++) {
                audioSourcePool.Release(sources[i]);
            }
        }

        public bool IsAudioActive(int soundId) {
            return id2ActiveAudio.ContainsKey(soundId);
        }

        public bool TryGetAudioSource(int soundId, out AudioSource audioSource) {
            if (id2ActiveAudio.TryGetValue(soundId, out SoundData sound)) {
                audioSource = sound.audioSource;
                return true;
            }
            audioSource = null; return false;
        }

        public int Play(
            AudioClip clip,
            AudioMixerGroup mixerGroup,
            float volume,
            float spatial,
            bool loop,
            int priority,
            float fadeDuration,
            Vector3 position,
            Action onComplete) {
            if (audioSourcePool.CountActive >= maxPoolSize) { return 0; }

            int normalUsed = audioSourcePool.CountActive - highPriorityCount;
            int allowedNormalUse = maxPoolSize - Mathf.Max(maxHighPriorityReserveCount, highPriorityCount);
            if (normalUsed >= allowedNormalUse) { return 0; }

            return ActiveInternal(clip, mixerGroup, volume, spatial, loop, priority, fadeDuration, position, false, onComplete);
        }

        public int PlayReservedPriority(
            AudioClip clip,
            AudioMixerGroup mixerGroup,
            float volume,
            float spatial,
            bool loop,
            int priority,
            float fadeDuration,
            Vector3 position,
            Action onComplete) {
            if (audioSourcePool.CountActive >= maxPoolSize) { return 0; }

            highPriorityCount++;
            return ActiveInternal(clip, mixerGroup, volume, spatial, loop, priority, fadeDuration, position, true, onComplete);
        }

        public void Stop(int id, float fadeDuration, Action onComplete) {
            DeactiveInternal(id, fadeDuration, onComplete);
        }

        public void Reuse(
            int existingId,
            AudioClip newClip,
            float fadeOutDuration,
            float fadeInDuration,
            Action onStopComplete,
            Action onPlayComplete) {
            DeactiveAndActiveInternal(existingId, newClip, fadeOutDuration, fadeInDuration, onStopComplete, onPlayComplete);
        }

        public float GetDeltaTime() {
            return RunInPause ? Time.unscaledDeltaTime : Time.deltaTime;
        }

        // INTERNAL

        private int ActiveInternal(
            AudioClip clip,
            AudioMixerGroup mixerGroup,
            float volume,
            float spatial,
            bool loop,
            int priority,
            float fadeDuration,
            Vector3 position,
            bool isHighPriority,
            Action onComplete) {
            AudioSource audioSource = audioSourcePool.Get();

            SoundData sound = new() {
                id = ++id,
                volume = volume,
                audioSource = audioSource,
                highPriority = isHighPriority,
            };

            audioSource.clip = clip;
            audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.loop = loop;
            audioSource.priority = priority;
            audioSource.spatialBlend = spatial;
            audioSource.transform.position = position;
            sound.playCOR = StartCoroutine(PlaySoundCOR(sound, fadeDuration, onComplete));
            id2ActiveAudio.Add(id, sound);
            return id;
        }

        private void DeactiveInternal(int id, float fadeDuration, Action onComplete) {
            if (id2VolumeFadeCoroutine.TryGetValue(id, out Coroutine fadeCoroutine)) {
                fadeCoroutine.Kill(this);
                id2VolumeFadeCoroutine.Remove(id);
            }

            bool isFound = id2ActiveAudio.TryGetValue(id, out SoundData sound);

            if (isFound && fadeDuration <= 0f) {
                sound.playCOR.Kill(this);
                ClearSoundData(sound);
                onComplete?.Invoke();
            } else if (isFound && fadeDuration > 0f) {
                sound.playCOR.Kill(this);
                DOFadeVolume(sound.audioSource, 0f, fadeDuration, () => {
                    ClearSoundData(sound);
                    onComplete?.Invoke();
                });
            }
        }

        private void DeactiveAndActiveInternal(
            int id,
            AudioClip newAudioClip,
            float fadeOutDuration,
            float fadeInDuration,
            Action onStopComplete,
            Action onPlayComplete) {

            if (id2VolumeFadeCoroutine.TryGetValue(id, out Coroutine fadeCoroutine)) {
                fadeCoroutine.Kill(this);
                id2VolumeFadeCoroutine.Remove(id);
            }

            bool isFound = id2ActiveAudio.TryGetValue(id, out SoundData sound);

            if (!isFound) { return; }

            if (fadeOutDuration <= 0f) {
                sound.playCOR.Kill(this);
                sound.audioSource.Stop();
                sound.audioSource.clip = newAudioClip;
                sound.playCOR = StartCoroutine(PlaySoundCOR(sound, fadeInDuration, onPlayComplete));
                id2ActiveAudio[id] = sound;
                onStopComplete?.Invoke();
                return;
            }

            if (fadeOutDuration > 0f) {
                sound.playCOR.Kill(this);
                DOFadeVolume(sound.audioSource, 0f, fadeOutDuration, () => {
                    sound.audioSource.Stop();
                    sound.audioSource.clip = newAudioClip;
                    sound.playCOR = StartCoroutine(PlaySoundCOR(sound, fadeInDuration, onPlayComplete));
                    id2ActiveAudio[id] = sound;
                    onStopComplete?.Invoke();
                });
            }
        }

        private IEnumerator PlaySoundCOR(SoundData sound, float fadeDuration, Action onComplete) {
            if (fadeDuration <= 0f) {
                sound.audioSource.volume = sound.volume;
                sound.audioSource.Play();
                onComplete?.Invoke();
            } else {
                sound.audioSource.volume = 0f;
                sound.audioSource.Play();

                Coroutine fadeCOR = DOFadeVolume(sound.audioSource, sound.volume, fadeDuration, () => {
                    id2VolumeFadeCoroutine.Remove(sound.id);
                    onComplete?.Invoke();
                });

                id2VolumeFadeCoroutine.Add(sound.id, fadeCOR);
            }

            if (sound.audioSource.loop) { yield break; }

            float timeToDeactive = sound.audioSource.clip.length;
            yield return new WaitForSeconds(timeToDeactive);

            if (id2ActiveAudio.ContainsKey(sound.id)) {
                ClearSoundData(sound);
            }
        }

        private Coroutine DOFadeVolume(AudioSource audioSource, float target, float duration, Action onComplete) {
            return StartCoroutine(VolumeFadeCoroutine(audioSource, target, duration, onComplete));
        }

        private IEnumerator VolumeFadeCoroutine(AudioSource audioSource, float target, float duration, Action onComplete) {
            float timer = 0f;
            float current = audioSource.volume;

            while (timer <= duration) {
                float value = Mathf.Lerp(current, target, timer / duration);
                audioSource.volume = value;
                timer += GetDeltaTime();
                yield return null;
            }

            audioSource.volume = target;
            onComplete?.Invoke();
        }

        private void ClearSoundData(SoundData sound) {
            sound.audioSource.Stop();
            sound.audioSource.loop = false;
            sound.audioSource.priority = 128;
            sound.audioSource.outputAudioMixerGroup = null;
            audioSourcePool.Release(sound.audioSource);
            id2ActiveAudio.Remove(sound.id);
            if (sound.highPriority) { highPriorityCount--; }
        }
    }
}