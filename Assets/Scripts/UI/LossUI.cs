using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using TMPro;


public class LossUI : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector] public UnityEvent OnScreenTouch = new();

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _starsText;

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
        UpdateStarsText();
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
}
