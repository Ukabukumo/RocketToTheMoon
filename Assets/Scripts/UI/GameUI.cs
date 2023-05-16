using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

public class GameUI : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnPauseButtonClick = new();

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private Slider _fuelBar;
    [SerializeField] private Button _pauseButton;

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
        GameManager.Instance.OnScoreChange.AddListener(() => UpdateScoreText());
        GameManager.Instance.OnCoinsChange.AddListener(() => UpdateCoinsText());
        GameManager.Instance.OnFuelChange.AddListener(() => UpdateFuelBar());
        _pauseButton.onClick.AddListener(OnPauseButtonClick.Invoke);
    }

    private void UpdateUI()
    {
        UpdateScoreText();
        UpdateCoinsText();
        UpdateFuelBar();
    }

    private void UpdateScoreText()
    {
        int score = (int)Mathf.Ceil(GameManager.Instance.Score);
        _scoreText.text = Convert.ToString(score);
    }

    private void UpdateCoinsText()
    {
        int coins = GameManager.Instance.Coins;
        _coinsText.text = Convert.ToString(coins);
    }

    private void UpdateFuelBar()
    {
        float fuel = GameManager.Instance.GetFuel();
        _fuelBar.value = fuel;
    }
}
