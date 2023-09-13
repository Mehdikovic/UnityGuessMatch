using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardCountUI : MonoBehaviour {
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image border;

    private LevelDataSO levelSO;

    public LevelDataSO GetLevelSO() => levelSO;

    public void Initialize(LoadSceneMainWindowUI mainWindowUI, LevelDataSO levelSO) {
        this.levelSO = levelSO;
        text.text = levelSO.count.ToString();
        HideOutline();

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() => {
            mainWindowUI.SelectCardUI(this);
        });
    }

    public void ShowOutline() {
        border.gameObject.SetActive(true);
    }

    public void HideOutline() {
        border.gameObject.SetActive(false);
    }
}
