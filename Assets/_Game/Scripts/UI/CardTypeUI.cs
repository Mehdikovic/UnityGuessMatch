using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardTypeUI : MonoBehaviour {
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image border;

    [SerializeField] private string saveID = "";

    private bool isActive;

    public bool GetActive() => isActive;
    public string GetSaveID() => saveID;

    public void Initialize(LoadSceneMainWindowUI mainWindowUI) {
        Hide();

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() => {
            mainWindowUI.SelectCardTypeUI(this);
        });
    }

    public void Show() {
        isActive = true;
        border.gameObject.SetActive(true);
    }

    public void Hide() {
        isActive = false;
        border.gameObject.SetActive(false);
    }
}
