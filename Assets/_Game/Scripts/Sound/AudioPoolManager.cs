using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;
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
        private Dictionary<int, SoundData> id2ActiveAudio;
        private Dictionary<int, Tween> id2PlayTween;
        private ObjectPool<AudioSource> audioSourcePool;
        private int id = 0;
        private int highPriorityCount = 0;
        private int maxPoolSize;
        private int maxHighPriorityReserveCount;

        private void Awake() {
            maxPoolSize = 16;
            maxHighPriorityReserveCount = 2;

            id2ActiveAudio = new();
            id2PlayTween = new();
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
            if (id2PlayTween.TryGetValue(id, out Tween tween)) {
                tween.Kill();
                id2PlayTween.Remove(id);
            }

            bool isFound = id2ActiveAudio.TryGetValue(id, out SoundData sound);

            if (isFound && fadeDuration <= 0f) {
                this.StopCOR(ref sound.playCOR);
                ClearSoundData(sound);
                onComplete?.Invoke();
            } else if (isFound && fadeDuration > 0f) {
                this.StopCOR(ref sound.playCOR);
                sound.audioSource.DOFade(0f, fadeDuration).OnComplete(() => { ClearSoundData(sound); onComplete?.Invoke(); });
            }
        }

        private void DeactiveAndActiveInternal(
            int id,
            AudioClip newAudioClip,
            float fadeOutDuration,
            float fadeInDuration,
            Action onStopComplete,
            Action onPlayComplete) {

            if (id2PlayTween.TryGetValue(id, out Tween tween)) {
                tween.Kill();
                id2PlayTween.Remove(id);
            }

            bool isFound = id2ActiveAudio.TryGetValue(id, out SoundData sound);

            if (!isFound) { return; }

            if (fadeOutDuration <= 0f) {
                this.StopCOR(ref sound.playCOR);
                sound.audioSource.Stop();
                sound.audioSource.clip = newAudioClip;
                sound.playCOR = StartCoroutine(PlaySoundCOR(sound, fadeInDuration, onPlayComplete));
                id2ActiveAudio[id] = sound;
                onStopComplete?.Invoke();
                return;
            }

            if (fadeOutDuration > 0f) {
                this.StopCOR(ref sound.playCOR);
                sound.audioSource
                    .DOFade(0f, fadeOutDuration)
                    .OnComplete(() => {
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
                Tween tween = sound.audioSource.DOFade(sound.volume, fadeDuration);
                tween.OnComplete(() => { id2PlayTween.Remove(sound.id); onComplete?.Invoke(); });
                id2PlayTween.Add(sound.id, tween);
            }

            if (sound.audioSource.loop) { yield break; }

            float timeToDeactive = sound.audioSource.clip.length;
            yield return new WaitForSeconds(timeToDeactive);

            if (id2ActiveAudio.ContainsKey(sound.id)) {
                ClearSoundData(sound);
            }
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