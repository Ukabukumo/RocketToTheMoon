using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using TMPro;

public class MenuUI : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector] public UnityEvent OnUpgradeMenuButtonClick = new();
    [HideInInspector] public UnityEvent OnScreenTouch = new();

    [SerializeField] private TextMeshProUGUI[] _scoreTexts;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private Button _upgradeMenuButton;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnScreenTouch.Invoke();
    }

    private void Awake()
    {
        InitializeListeners();
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    private void InitializeListeners()
    {
        _upgradeMenuButton.onClick.AddListener(OnUpgradeMenuButtonClick.Invoke);
        SaveDataManager.Instance.OnCoinsChangeEvent.AddListener(() => OnCoinsChange());
    }

    private void OnCoinsChange()
    {
        int coins = SaveDataManager.Instance.LoadCoins();
        _coinsText.text = Convert.ToString(coins);
    }

    private void UpdateUI()
    {
        UpdateScoreTexts();
        UpdateCoinsText();
    }

    private void UpdateScoreTexts()
    {
        List<int> scores = SaveDataManager.Instance.LoadScores();

        for (int i = 0; i < _scoreTexts.Length; ++i)
        {
            _scoreTexts[i].text = Convert.ToString(scores[i]);
        }
    }

    private void UpdateCoinsText()
    {
        int coins = SaveDataManager.Instance.LoadCoins();
        _coinsText.text = Convert.ToString(coins);
    }
}
