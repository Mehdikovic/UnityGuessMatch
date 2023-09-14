using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardTypeUI : MonoBehaviour {
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Outline outline;

    [SerializeField] private string saveID = "";

    public bool GetActive() => outline.enabled;
    public string GetSaveID() => saveID;

    public void Initialize(LoadSceneMainWindowUI mainWindowUI) {
        Hide();

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() => {
            mainWindowUI.SelectCardTypeUI(this);
        });
    }

    public void Show() {
        outline.enabled = true;
    }

    public void Hide() {
        outline.enabled = false;
    }
}
