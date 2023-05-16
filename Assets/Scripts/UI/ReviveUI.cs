using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ReviveUI : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnMenuButtonClick = new();
    [HideInInspector] public UnityEvent OnReviveForLifeButtonClick = new();
    [HideInInspector] public UnityEvent OnReviveForAdButtonClick = new();

    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _reviveForLifeButton;
    [SerializeField] private Button _reviveForAdButton;

    private void Awake()
    {
        InitializeListeners();
    }

    private void OnEnable()
    {
        InitializeUI();
    }

    private void InitializeListeners()
    {
        _menuButton.onClick.AddListener(OnMenuButtonClick.Invoke);
        _reviveForLifeButton.onClick.AddListener(OnReviveForLifeButtonClick.Invoke);
        _reviveForAdButton.onClick.AddListener(OnReviveForAdButtonClick.Invoke);
    }

    private void InitializeUI()
    {
        _reviveForLifeButton.gameObject.SetActive(false);
        _reviveForAdButton.gameObject.SetActive(false);

        if (GameManager.Instance.HasDoubleLife)
        {
            _reviveForLifeButton.gameObject.SetActive(true);
        }
        else if (GameManager.Instance.HasAd)
        {
            _reviveForAdButton.gameObject.SetActive(true);
        }
    }
}
