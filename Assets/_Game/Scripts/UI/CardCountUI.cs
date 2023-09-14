using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardCountUI : MonoBehaviour {
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Outline outline;

    private LevelDataSO levelSO;

    public LevelDataSO GetLevelSO() => levelSO;

    public void Initialize(LoadSceneMainWindowUI mainWindowUI, LevelDataSO levelSO) {
        this.levelSO = levelSO;
        text.text = levelSO.count.ToString();
        HideOutline();

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(() => {
            mainWindowUI.SelectCardCountUI(this);
        });
    }

    public void ShowOutline() {
        outline.enabled = true;
    }

    public void HideOutline() {
        outline.enabled = false;
    }
}
