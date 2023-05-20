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
    [SerializeField] private TextMeshProUGUI _starsText;
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
        SaveDataManager.Instance.OnStarsChangeEvent.AddListener(() => OnStarsChange());
    }

    private void OnStarsChange()
    {
        int stars = SaveDataManager.Instance.LoadStars();
        _starsText.text = Convert.ToString(stars);
    }

    private void UpdateUI()
    {
        UpdateScoreTexts();
        UpdateStarsText();
    }

    private void UpdateScoreTexts()
    {
        List<int> scores = SaveDataManager.Instance.LoadScores();

        for (int i = 0; i < _scoreTexts.Length; ++i)
        {
            _scoreTexts[i].text = Convert.ToString(scores[i]);
        }
    }

    private void UpdateStarsText()
    {
        int stars = SaveDataManager.Instance.LoadStars();
        _starsText.text = Convert.ToString(stars);
    }
}
