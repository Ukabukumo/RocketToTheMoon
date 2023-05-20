using UnityEngine;
using UnityEngine.Events;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        MENU,
        UPGRADE,
        GAME,
        PAUSE,
        REVIVE,
        LOSS
    }

    [HideInInspector] public UnityEvent OnStartGame = new();
    [HideInInspector] public UnityEvent OnPauseGame = new();
    [HideInInspector] public UnityEvent OnUnpauseGame = new();
    [HideInInspector] public UnityEvent OnRevivePause = new();
    [HideInInspector] public UnityEvent OnRevive = new();
    [HideInInspector] public UnityEvent OnLossGame = new();
    [HideInInspector] public UnityEvent OnRestartGame = new();
    [HideInInspector] public UnityEvent OnUpgradeMenu = new();
    [HideInInspector] public UnityEvent<UpgradeManager.Upgrade> OnUpgradeSkill = new();
    [HideInInspector] public UnityEvent OnScoreChange = new();
    [HideInInspector] public UnityEvent OnStarsChange = new();
    [HideInInspector] public UnityEvent OnFuelChange = new();

    public GameState CurrentGameState { get; private set; }
    public bool HasDoubleLife { get; private set; }
    public bool HasAd { get; private set; }
    public float Score { get; private set; }
    public int Stars { get; private set; }

    [SerializeField] private Transform _rocketSpawnPoint;
    [SerializeField] private GameObject _rocketObject;
    [SerializeField] private GameObject _cameraObject;

    private const float SCORE_COEFFICIENT = 100.0f;
    private const float FUEL_PERCENTAGE = 15.0f;

    private FuelConsumption _rocketFuelConsumption;
    private CollisionDetection _rocketCollisionDetection;
    private BeyondScreenDetection _rocketBeyondScreenDetection;

    public float GetFuel()
    {
        return _rocketObject.GetComponent<FuelConsumption>().GetFuel();
    }

    private void Awake()
    {
        InitializeInstance();
        InitializeManager();
    }

    private void Start()
    {
        InitializeListeners();
        RestartGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveDataManager.Instance.AddStars(10000);
        }

        Scoring();
    }

    private void OnDisable()
    {
        DeinitializeListeners();
    }

    private void OnDestroy()
    {
        DeinitializeListeners();
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
        if (!_rocketObject.TryGetComponent(out FuelConsumption fuelConsumption))
        {
            throw new MissingComponentException("FuelConsumption component is missing from RocketObject");
        }
        if (!_rocketObject.TryGetComponent(out CollisionDetection collisionDetection))
        {
            throw new MissingComponentException("CollisionDetection component is missing from RocketObject");
        }
        if (!_rocketObject.TryGetComponent(out BeyondScreenDetection beyondScreenDetection))
        {
            throw new MissingComponentException("BeyondScreenDetection component is missing from RocketObject");
        }

        _rocketFuelConsumption = fuelConsumption;
        _rocketCollisionDetection = collisionDetection;
        _rocketBeyondScreenDetection = beyondScreenDetection;
    }

    private void StartGame()
    {
        CurrentGameState = GameState.GAME;
        Score = 0.0f;
        Stars = 0;
        HasDoubleLife = SaveDataManager.Instance.LoadUpgrade(UpgradeManager.Upgrade.DOUBLE_LIFE) == 1.0f ? true : false;
        HasAd = true;
        OnStartGame.Invoke();
    }

    private void PauseGame()
    {
        CurrentGameState = GameState.PAUSE;
        Time.timeScale = 0.0f;
        OnPauseGame.Invoke();
    }

    private void UnpauseGame()
    {
        CurrentGameState = GameState.GAME;
        Time.timeScale = 1.0f;
        OnUnpauseGame.Invoke();
    }

    private void LossGame()
    {
        if (HasDoubleLife)
        {
            RevivePause();
            HasDoubleLife = false;
            return;
        }
        else if (HasAd)
        {
            RevivePause();
            HasAd = false;
            return;
        }

        if (CurrentGameState == GameState.LOSS)
        {
            return;
        }

        CurrentGameState = GameState.LOSS;
        SaveDataManager.Instance.SaveScore(Convert.ToInt32(Score));
        SaveDataManager.Instance.AddStars(Stars);
        OnLossGame.Invoke();
    }

    private void RevivePause()
    {
        Time.timeScale = 0.0f;
        CurrentGameState = GameState.REVIVE;
        OnRevivePause.Invoke();
    }

    private void Revive()
    {
        CurrentGameState = GameState.GAME;
        OnRevive.Invoke();
    }

    private void RestartGame()
    {
        CurrentGameState = GameState.MENU;
        _cameraObject.transform.position = new Vector3(0.0f, 0.0f, _cameraObject.transform.position.z);
        OnRestartGame.Invoke();
    }

    private void GoToMenu()
    {
        switch (CurrentGameState)
        {
            case GameState.PAUSE:
                UnpauseGame();
                HasDoubleLife = false;
                HasAd = false;
                LossGame();
                break;
            case GameState.UPGRADE:
                LoadManager.Instance.ExecuteAfterLoad(RestartGame);
                break;
            case GameState.REVIVE:
                UnpauseGame();
                HasDoubleLife = false;
                HasAd = false;
                LossGame();
                break;
        }
    }

    private void GoToUpgradeMenu()
    {
        CurrentGameState = GameState.UPGRADE;
        OnUpgradeMenu.Invoke();
    }

    private void Scoring()
    {
        if (CurrentGameState == GameState.GAME)
        {
            Score = _cameraObject.transform.position.y * SCORE_COEFFICIENT;
            OnScoreChange.Invoke();
        }
    }

    private void CollectStar()
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.STAR);
        ++Stars;
        OnStarsChange.Invoke();
    }

    private void CollectFuel()
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.FUEL);
        _rocketFuelConsumption.AddFuel(FUEL_PERCENTAGE);
        OnFuelChange.Invoke();
    }

    private void InitializeListeners()
    {
        _rocketFuelConsumption.OnFuelRunOutEvent.AddListener(() => LossGame());
        _rocketCollisionDetection.OnObstacleCollisionEvent.AddListener(() => LossGame());
        _rocketCollisionDetection.OnStarCollisionEvent.AddListener(() => CollectStar());
        _rocketCollisionDetection.OnFuelCollisionEvent.AddListener(() => CollectFuel());
        _rocketBeyondScreenDetection.OnScreenBottomReached.AddListener(() => LossGame());

        MenuUI menuUI = UIManager.Instance.GetMenuUI();
        menuUI.OnUpgradeMenuButtonClick.AddListener(() => GoToUpgradeMenu());
        menuUI.OnScreenTouch.AddListener(() => StartGame());

        UpgradeUI upgradeUI = UIManager.Instance.GetUpgradeUI();
        upgradeUI.OnUpgradeButtonClick.AddListener((UpgradeManager.Upgrade upgrade) => OnUpgradeSkill.Invoke(upgrade));
        upgradeUI.OnMenuButtonClick.AddListener(() => GoToMenu());

        GameUI gameUI = UIManager.Instance.GetGameUI();
        gameUI.OnPauseButtonClick.AddListener(() => PauseGame());

        PauseUI pauseUI = UIManager.Instance.GetPauseUI();
        pauseUI.OnMenuButtonClick.AddListener(() => GoToMenu());
        pauseUI.OnReturnButtonClick.AddListener(() => UnpauseGame());

        ReviveUI reviveUI = UIManager.Instance.GetReviveUI();
        reviveUI.OnMenuButtonClick.AddListener(() => GoToMenu());
        reviveUI.OnReviveForLifeButtonClick.AddListener(delegate { Revive(); PauseGame(); });
        reviveUI.OnReviveForAdButtonClick.AddListener(delegate { Revive(); PauseGame(); });

        LossUI lossUI = UIManager.Instance.GetLossUI();
        lossUI.OnScreenTouch.AddListener(() => LoadManager.Instance.ExecuteAfterLoad(RestartGame));
    }

    private void DeinitializeListeners()
    {
        _rocketFuelConsumption.OnFuelRunOutEvent.RemoveAllListeners();
        _rocketCollisionDetection.OnObstacleCollisionEvent.RemoveAllListeners();
        _rocketCollisionDetection.OnStarCollisionEvent.RemoveAllListeners();
        _rocketBeyondScreenDetection.OnScreenBottomReached.RemoveAllListeners();
    }
}
