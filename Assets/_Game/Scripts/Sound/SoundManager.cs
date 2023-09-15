using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Sound {
    [RequireComponent(typeof(AudioPoolManager))]
    [DisallowMultipleComponent]
    public class SoundManager : MonoSingleton<SoundManager> {
        static public int NameToHash(string mixerName) => mixerName.GetHashCode();
        static public int NameToHash(AudioMixerGroup mixerGroup) => mixerGroup.name.GetHashCode();

        [SerializeField] private AudioMixer audioMixer;
        private Dictionary<int, AudioMixerGroup> mixerHashName2Mixer;
        private Dictionary<int, Coroutine> mixerHashName2VolumeCoroutine;

        private AudioPoolManager audioPool;

        public AudioPoolManager Pool => audioPool;

        protected override void OnAwakeAfter() {
            mixerHashName2Mixer = new();
            mixerHashName2VolumeCoroutine = new();
            audioPool = GetComponent<AudioPoolManager>();

            foreach (AudioMixerGroup group in audioMixer.FindMatchingGroups("")) {
                mixerHashName2Mixer.Add(group.name.GetHashCode(), group);
            }
        }

        public IEnumerable<AudioMixerGroup> MixerEnumerable() => mixerHashName2Mixer.Values;
        public AudioMixerGroup GetMixer(string name) => GetMixer(name.GetHashCode());
        public AudioMixerGroup GetMixer(int hashName) => mixerHashName2Mixer[hashName];

        public int PlaySound(
            int mixerGroupHashName,
            AudioClip clip,
            float volume,
            float spatial = 0f,
            float fadeDuration = 0f,
            bool loop = false,
            Vector3 position = new(),
            int priority = 128,
            bool useHighPriorityReserverdPool = false,
            Action onComplete = null) {

            AudioMixerGroup mixer = mixerHashName2Mixer[mixerGroupHashName];
            if (useHighPriorityReserverdPool) {
                return audioPool.PlayReservedPriority(clip, mixer, volume, spatial, loop, priority, fadeDuration, position, onComplete);
            } else {
                return audioPool.Play(clip, mixer, volume, spatial, loop, priority, fadeDuration, position, onComplete);
            }
        }

        public int PlaySound(
            string mixerGroupName,
            AudioClip clip,
            float volume,
            float spatial = 0f,
            float fadeDuration = 0f,
            bool loop = false,
            Vector3 position = new(),
            int priority = 128,
            bool useHighPriorityReserverdPool = false,
            Action onComplete = null) {

            return PlaySound(mixerGroupName.GetHashCode(), clip, volume, spatial, fadeDuration, loop, position, priority, useHighPriorityReserverdPool, onComplete);
        }

        public int PlaySound(
            AudioMixerGroup mixer,
            AudioClip clip,
            float volume,
            float spatial = 0f,
            float fadeDuration = 0f,
            bool loop = false,
            Vector3 position = new(),
            int priority = 128,
            bool useHighPriorityReserverdPool = false,
            Action onComplete = null) {

            if (useHighPriorityReserverdPool) {
                return audioPool.PlayReservedPriority(clip, mixer, volume, spatial, loop, priority, fadeDuration, position, onComplete);
            } else {
                return audioPool.Play(clip, mixer, volume, spatial, loop, priority, fadeDuration, position, onComplete);
            }
        }

        public void ReuseSound(
            int id,
            AudioClip newClip,
            float fadeOutDuration = 0f,
            float fadeInDuration = 0f,
            Action onStopComplete = null,
            Action onPlayComplete = null) {
            audioPool.Reuse(id, newClip, fadeOutDuration, fadeInDuration, onStopComplete, onPlayComplete);
        }

        public void StopSound(int id, float fadeDuration = 0f, Action onComplete = null) {
            audioPool.Stop(id, fadeDuration, onComplete);
        }

        public void SetMixerVolume(string mixerGroupName, float volume, float fadeTime = 0f) {
            int hashedName = mixerGroupName.GetHashCode();
            SetMixerVolumeInternal(hashedName, mixerGroupName, volume, fadeTime);
        }

        public void SetMixerVolume(AudioMixerGroup mixerGroup, float volume, float fadeTime = 0f) {
            SetMixerVolume(mixerGroup.name, volume, fadeTime);
        }

        public void SetMixerVolume(int mixerGroupHashName, float volume, float fadeTime = 0f) {
            string mixerGroupName = GetMixer(mixerGroupHashName).name;
            SetMixerVolumeInternal(mixerGroupHashName, mixerGroupName, volume, fadeTime);
        }

        private void SetMixerVolumeInternal(int mixerGroupHashName, string mixerGroupName, float volume, float fadeTime) {
            float logVolume = Util.ConvertToDecibel(volume);

            if (fadeTime <= 0f) {
                audioMixer.SetFloat(mixerGroupName, logVolume);
                return;
            }

            if (mixerHashName2VolumeCoroutine.TryGetValue(mixerGroupHashName, out Coroutine coroutine)) {
                coroutine.Kill(this);
                mixerHashName2VolumeCoroutine.Remove(mixerGroupHashName);
            }

            Coroutine mixerVolumeCoroutine = StartCoroutine(SetMixerVolumeCOR(logVolume, fadeTime, mixerGroupName));
            mixerHashName2VolumeCoroutine.Add(mixerGroupHashName, mixerVolumeCoroutine);
        }

        private IEnumerator SetMixerVolumeCOR(float target, float timerMax, string mixerGroupName) {
            float timer = 0f;
            float current = audioMixer.GetFloat(mixerGroupName, out current) ? current : 0;

            if (current == target) { yield break; }

            while (timer <= timerMax) {
                float value = Mathf.Lerp(current, target, timer / timerMax);
                audioMixer.SetFloat(mixerGroupName, value);
                timer += audioPool.GetDeltaTime();
                yield return null;
            }
            audioMixer.SetFloat(mixerGroupName, target);
        }
    }
}