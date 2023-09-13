using ResourceItem;
using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneMainWindowUI : WindowUI {
    [Header("Card Count UI")]
    [SerializeField] private CardCountUI cardCountUITemplate;
    [SerializeField] private Transform cardCountContainer;

    [Header("Card Type UI")]
    [SerializeField] private List<CardTypeUI> cardTypeList;

    [Header("UI")]
    [SerializeField] private Button startButton;

    private Dictionary<int, LevelDataSO> id2LevelSO;
    private HashSet<CardCountUI> cardUISet;
    private List<LevelDataSO> sortedLevelSOList;

    private CardCountUI selectedCardUI;

    private void Start() {
        id2LevelSO = DatabaseUtility.Build<LevelDataSO>("LevelDataSO/");
        cardUISet = new();

        cardCountUITemplate.gameObject.SetActive(false);

        foreach (Transform t in cardCountContainer) {
            if (t.gameObject == cardCountUITemplate.gameObject) { continue; }
            Destroy(t.gameObject);
        }

        sortedLevelSOList = id2LevelSO.Values.OrderBy(l => l.count).ToList();

        foreach (LevelDataSO levelSO in GetLevelSOEnumerable()) {
            CardCountUI cardUI = Instantiate(cardCountUITemplate, cardCountContainer);
            cardUI.transform.ResetLocalTransform();
            cardUI.Initialize(this, levelSO);
            cardUI.gameObject.SetActive(true);
            cardUISet.Add(cardUI);
        }

        foreach (CardTypeUI cardUI in cardTypeList) {
            cardUI.Initialize(this);
        }

        this.ImmediateShow();

        SelectCardUI(cardUISet.First());
        SelectCardTypeUI(cardTypeList.First());

        startButton.onClick.AddListener(() => {
            PlayerPrefs.SetInt(SaveID.CardConfigID, selectedCardUI.GetLevelSO().Id);

            foreach (var card in cardTypeList) {
                PlayerPrefs.SetInt(card.GetSaveID(), card.GetActive() ? 1 : 0);
            }

            PlayerPrefs.Save();

            SceneManager.LoadScene("MainScene");
        });
    }


    public void SelectCardUI(CardCountUI selectedCardUI) {
        foreach (CardCountUI cardUI in cardUISet) {
            cardUI.HideOutline();
        }

        selectedCardUI.ShowOutline();
        this.selectedCardUI = selectedCardUI;
    }


    public void SelectCardTypeUI(CardTypeUI cardTypeUI) {
        if (cardTypeUI.GetActive() && !IsOnlyActiveOne(cardTypeUI)) {
            cardTypeUI.Hide();
        } else {
            cardTypeUI.Show();
        }

        foreach (var i in cardTypeList) {
            if (i.GetActive()) {
                Debug.Log("Active ID: " + i.GetSaveID());
            }
        }
    }

    private bool IsOnlyActiveOne(CardTypeUI selectedCardTypeUI) {
        int activeCount = 0;
        CardTypeUI lastActiveOne = null;

        foreach (CardTypeUI cardUI in cardTypeList) {
            if (cardUI.GetActive()) {
                lastActiveOne = cardUI;
                activeCount++;
            }
        }

        if (activeCount > 1) {
            return false;
        }

        return selectedCardTypeUI == lastActiveOne;
    }

    private IEnumerable<LevelDataSO> GetLevelSOEnumerable() => sortedLevelSOList;
}
