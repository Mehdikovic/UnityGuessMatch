using Sound;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicManager : MonoBehaviour {
    [SerializeField] private AudioClip backgroundMusicClip;

    private int soundID = -1;

    private void Start() {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        StartCoroutine(PlaySoundCOR());
    }

    private void OnDestroy() {
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(Scene _, Scene __) {
        PlaySound(.5f);
    }

    private void PlaySound(float fade) {
        if (SceneManager.GetActiveScene().name.StringEquals("LoadScene")) {
            soundID = SoundManager.Instance.PlaySound("Music", backgroundMusicClip, .7f, loop: true, fadeDuration: fade);
        } else if (soundID > 0) {
            SoundManager.Instance.StopSound(soundID, fadeDuration: fade + 1f);
        }
    }

    private IEnumerator PlaySoundCOR() {
        yield return new WaitForSeconds(.5f);
        PlaySound(2f);
    }
}
