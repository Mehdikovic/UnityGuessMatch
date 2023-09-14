using GameManagement;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class PopupResultWindowUI : MonoBehaviour {
    [Header("")]
    [SerializeField] private RectTransform winRect;
    [SerializeField] private RectTransform loseRect;

    [Header("UI Elements")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button homeButton;

    [SerializeField] private TextMeshProUGUI allTimeScoreText;

    [SerializeField] private GameObject[] gameobjects;

    private List<Behaviour> ui;

    private void Awake() {
        UIUtil.HideWindowImmediate(loseRect);
        UIUtil.HideWindowImmediate(winRect);

        ui = new() {
            restartButton,
            homeButton,
        };

        HideGameObjects();

        GameManager.OnGameStop += GameManager_OnGameStop;

        restartButton.onClick.AddListener(() => {
            Player.Instance.SaveState();
            
            UIUtil.UIElementsActivation(false, ui);
            SceneController.Instance.Force().Restart();
        });

        homeButton.onClick.AddListener(() => {
            Player.Instance.SaveState();

            UIUtil.UIElementsActivation(false, ui);
            SceneController.Instance.Force().LoadScene("LoadScene");
        });
    }

    private void OnDestroy() {
        GameManager.OnGameStop -= GameManager_OnGameStop;
    }

    private void GameManager_OnGameStop(bool result) {
        if (result) {
            UIUtil.ShowWindow(winRect, () => ShowGameObjects());
        } else {
            UIUtil.ShowWindow(loseRect, () => ShowGameObjects());
        }

        int allTimeScore = Player.Instance.GetAllCorrectGuesses();
        allTimeScoreText.text = $"All time score: {allTimeScore}";
    }

    private void HideGameObjects() {
        gameobjects.SafeForEach(c => c.SetActive(false));
    }

    private void ShowGameObjects() {
        gameobjects.SafeForEach(c => c.SetActive(true));
    }
}
