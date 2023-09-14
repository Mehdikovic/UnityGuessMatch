using GameManagement;
using Sound;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class PauseWindowUI : WindowUI {
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button closeButton;

    [Space]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    private void Start() {
        AddBehaviour(resumeButton, restartButton, homeButton, closeButton, musicSlider, soundSlider);

        SetupButtons();
        SetupSliders();
    }

    private void SetupButtons() {
        resumeButton.onClick.AddListener(() => {
            DisableUIElements();
            this.TransitionHide();
        });

        restartButton.onClick.AddListener(() => {
            DisableUIElements();
            SceneController.Instance.Force().Restart();
        });

        homeButton.onClick.AddListener(() => {
            DisableUIElements();
            SceneController.Instance.Force().LoadScene("LoadScene");
        });

        closeButton.onClick.AddListener(() => {
            if (!Application.isEditor) {
                DisableUIElements();
            }
            Application.Quit();
        });
    }

    protected override void EndShowCallback() {
        EnableUIElements();
    }

    protected override void EndHideCallback() {
        GameManager.Unpause();
    }

    private void SetupSliders() {
        musicSlider.value = PlayerPrefs.GetFloat(SaveID.Music, 1f);
        soundSlider.value = PlayerPrefs.GetFloat(SaveID.Sound, 1f);

        int musicHash = SoundManager.NameToHash("Music");
        int soundHash = SoundManager.NameToHash("Player");

        SoundManager.Instance.SetMixerVolume(musicHash, musicSlider.value);
        SoundManager.Instance.SetMixerVolume(soundHash, soundSlider.value);

        musicSlider.onValueChanged.AddListener((value) => {
            PlayerPrefs.SetFloat(SaveID.Music, value);
            SoundManager.Instance.SetMixerVolume(musicHash, value);
            PlayerPrefs.Save();
        });

        soundSlider.onValueChanged.AddListener((value) => {
            PlayerPrefs.SetFloat(SaveID.Sound, value);
            SoundManager.Instance.SetMixerVolume(soundHash, value);
            PlayerPrefs.Save();
        });
    }
}
