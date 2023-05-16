using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PauseUI : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnReturnButtonClick = new();
    [HideInInspector] public UnityEvent OnMenuButtonClick = new();

    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _menuButton;

    private void Awake()
    {
        InitializeListeners();
    }

    private void InitializeListeners()
    {
        _returnButton.onClick.AddListener(OnReturnButtonClick.Invoke);
        _menuButton.onClick.AddListener(OnMenuButtonClick.Invoke);
    }
}
