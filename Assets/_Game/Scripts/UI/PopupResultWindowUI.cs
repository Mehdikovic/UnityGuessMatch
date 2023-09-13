using GameManagement;
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

    private UIElementTag[] ui;

    private void Awake() {
        UIUtil.HideWindowImmediate(loseRect);
        UIUtil.HideWindowImmediate(winRect);

        ui = GetComponentsInChildren<UIElementTag>();

        HideComponents();

        GameManager.OnGameStop += GameManager_OnGameStop;

        restartButton.onClick.AddListener(() => {
            UIUtil.UIElementsActivation(false, ui);
            Player.Instance.SaveState();
            LoadSceneManager.Instance.Restart();
        });

        homeButton.onClick.AddListener(() => {
            UIUtil.UIElementsActivation(false, ui);
            Player.Instance.SaveState();
            LoadSceneManager.Instance.LoadScene("LoadScene");
        });
    }

    private void OnDestroy() {
        GameManager.OnGameStop -= GameManager_OnGameStop;
    }

    private void GameManager_OnGameStop(bool result) {
        if (result) {
            UIUtil.ShowWindow(winRect, () => ShowComponents());
        } else {
            UIUtil.ShowWindow(loseRect, () => ShowComponents());
        }

        int allTimeScore = Player.Instance.GetAllCorrectGuesses();
        allTimeScoreText.text = $"All time score: {allTimeScore}";
    }

    private void HideComponents() {
        gameobjects.SafeForEach(c => c.SetActive(false));
    }

    private void ShowComponents() {
        gameobjects.SafeForEach(c => c.SetActive(true));
    }
}
