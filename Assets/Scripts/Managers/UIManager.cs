using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Screens UI")]
    [SerializeField] private MenuUI _menuUI;
    [SerializeField] private UpgradeUI _upgradeUI;
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private PauseUI _pauseUI;
    [SerializeField] private ReviveUI _reviveUI;
    [SerializeField] private LossUI _lossUI;

    private Dictionary<GameManager.GameState, GameObject> _screensUI;

    public MenuUI GetMenuUI()
    {
        return _menuUI;
    }

    public UpgradeUI GetUpgradeUI()
    {
        return _upgradeUI;
    }

    public GameUI GetGameUI()
    {
        return _gameUI;
    }

    public PauseUI GetPauseUI()
    {
        return _pauseUI;
    }

    public ReviveUI GetReviveUI()
    {
        return _reviveUI;
    }

    public LossUI GetLossUI()
    {
        return _lossUI;
    }

    private void Awake()
    {
        InitializeInstance();
        InitializeManager();
        InitializeListeners();
    }

    private void InitializeInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void InitializeManager()
    {
        _screensUI = new()
        {
            { GameManager.GameState.MENU, _menuUI.gameObject },
            { GameManager.GameState.UPGRADE, _upgradeUI.gameObject },
            { GameManager.GameState.GAME, _gameUI.gameObject },
            { GameManager.GameState.PAUSE, _pauseUI.gameObject },
            { GameManager.GameState.REVIVE, _reviveUI.gameObject },
            { GameManager.GameState.LOSS, _lossUI.gameObject }
        };
    }

    private void InitializeListeners()
    {
        GameManager.Instance.OnStartGame.AddListener(() => SwitchUI());
        GameManager.Instance.OnPauseGame.AddListener(() => SwitchUI());
        GameManager.Instance.OnUnpauseGame.AddListener(() => SwitchUI());
        GameManager.Instance.OnRevivePause.AddListener(() => SwitchUI());
        GameManager.Instance.OnRevive.AddListener(() => SwitchUI());
        GameManager.Instance.OnLossGame.AddListener(() => SwitchUI());
        GameManager.Instance.OnRestartGame.AddListener(() => SwitchUI());
        GameManager.Instance.OnUpgradeMenu.AddListener(() => SwitchUI());
    }

    private void SwitchUI()
    {
        GameManager.GameState gameState = GameManager.Instance.CurrentGameState;

        if (!_screensUI.TryGetValue(gameState, out GameObject currentUI))
        {
            throw new ArgumentException($"Invalid game state: {gameState}");
        }

        foreach (var ui in _screensUI)
        {
            ui.Value.SetActive(false);
        }

        currentUI.SetActive(true);
    }
}
