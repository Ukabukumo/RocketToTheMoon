using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

public class GameUI : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnPauseButtonClick = new();

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _starsText;
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
        GameManager.Instance.OnStarsChange.AddListener(() => UpdateStarsText());
        GameManager.Instance.OnFuelChange.AddListener(() => UpdateFuelBar());
        _pauseButton.onClick.AddListener(OnPauseButtonClick.Invoke);
    }

    private void UpdateUI()
    {
        UpdateScoreText();
        UpdateStarsText();
        UpdateFuelBar();
    }

    private void UpdateScoreText()
    {
        int score = (int)Mathf.Ceil(GameManager.Instance.Score);
        _scoreText.text = Convert.ToString(score);
    }

    private void UpdateStarsText()
    {
        int stars = GameManager.Instance.Stars;
        _starsText.text = Convert.ToString(stars);
    }

    private void UpdateFuelBar()
    {
        float fuel = GameManager.Instance.GetFuel();
        _fuelBar.value = fuel;
    }
}
