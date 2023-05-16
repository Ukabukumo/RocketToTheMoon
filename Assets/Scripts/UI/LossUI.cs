using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using TMPro;


public class LossUI : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector] public UnityEvent OnScreenTouch = new();

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _coinsText;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnScreenTouch.Invoke();
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateScoreText();
        UpdateCoinsText();
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
}
