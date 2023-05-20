using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager Instance { get; private set; }
    public UnityEvent OnStarsChangeEvent = new();

    private const string FIRST_SCORE_KEY = "FirstScore";
    private const string SECOND_SCORE_KEY = "SecondScore";
    private const string THIRD_SCORE_KEY = "ThirdScore";

    private const string SPEED_UP_UPGRADE_KEY = "SpeedUpUpgrade";
    private const string FUEL_CONSUMPTION_UPGRADE_KEY = "FuelConsumptionUpgrade";
    private const string ENEMY_SPEED_UPGRADE_KEY = "ObstacleSpeedUpgrade";
    private const string DOUBLE_LIFE_UPGRADE_KEY = "DoubleLifeUpgrade";

    private const string SPEED_UP_PRICE_KEY = "SpeedUpPrice";
    private const string FUEL_CONSUMPTION_PRICE_KEY = "FuelConsumptionPrice";
    private const string OBSTACLE_SPEED_PRICE_KEY = "ObstacleSpeedPrice";
    private const string DOUBLE_LIFE_PRICE_KEY = "DoubleLifePrice";

    private readonly Dictionary<UpgradeManager.Upgrade, string> _upgradeKeys = new()
    {
        { UpgradeManager.Upgrade.SPEED_UP, SPEED_UP_UPGRADE_KEY },
        { UpgradeManager.Upgrade.FUEL_CONSUMPTION, FUEL_CONSUMPTION_UPGRADE_KEY },
        { UpgradeManager.Upgrade.OBSTACLE_SPEED, ENEMY_SPEED_UPGRADE_KEY },
        { UpgradeManager.Upgrade.DOUBLE_LIFE, DOUBLE_LIFE_UPGRADE_KEY }
    };

    private readonly Dictionary<UpgradeManager.Upgrade, string> _priceKeys = new()
    {
        { UpgradeManager.Upgrade.SPEED_UP, SPEED_UP_PRICE_KEY },
        { UpgradeManager.Upgrade.FUEL_CONSUMPTION, FUEL_CONSUMPTION_PRICE_KEY },
        { UpgradeManager.Upgrade.OBSTACLE_SPEED, OBSTACLE_SPEED_PRICE_KEY },
        { UpgradeManager.Upgrade.DOUBLE_LIFE, DOUBLE_LIFE_PRICE_KEY }
    };

    public void SaveScore(int score)
    {
        List<int> scores = LoadScores();
        scores.Add(score);

        // Sort array in descending order
        scores.Sort();
        scores.Reverse();

        PlayerPrefs.SetInt(FIRST_SCORE_KEY, scores[0]);
        PlayerPrefs.SetInt(SECOND_SCORE_KEY, scores[1]);
        PlayerPrefs.SetInt(THIRD_SCORE_KEY, scores[2]);

        PlayerPrefs.Save();
    }

    public List<int> LoadScores()
    {
        List<int> scores = new List<int>();

        if (PlayerPrefs.HasKey(FIRST_SCORE_KEY))
        {
            scores.Add(PlayerPrefs.GetInt(FIRST_SCORE_KEY));
        }
        else
        {
            scores.Add(0);
        }

        if (PlayerPrefs.HasKey(SECOND_SCORE_KEY))
        {
            scores.Add(PlayerPrefs.GetInt(SECOND_SCORE_KEY));
        }
        else
        {
            scores.Add(0);
        }

        if (PlayerPrefs.HasKey(THIRD_SCORE_KEY))
        {
            scores.Add(PlayerPrefs.GetInt(THIRD_SCORE_KEY));
        }
        else
        {
            scores.Add(0);
        }

        return scores;
    }

    public void AddStars(int stars)
    {
        int loadedStars = LoadStars();
        loadedStars += stars;
        SaveStars(loadedStars);
    }

    public void SaveStars(int stars)
    {
        PlayerPrefs.SetInt("Stars", stars);
        PlayerPrefs.Save();
        OnStarsChangeEvent.Invoke();
    }

    public int LoadStars()
    {
        if (PlayerPrefs.HasKey("Stars"))
        {
            return PlayerPrefs.GetInt("Stars");
        }
        else
        {
            return 0;
        }
    }

    public void SaveUpgrade(UpgradeManager.Upgrade upgrade, float value)
    {
        if (!_upgradeKeys.TryGetValue(upgrade, out string key))
        {
            throw new ArgumentException($"Invalid upgrade: {upgrade}");
        }

        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    public float LoadUpgrade(UpgradeManager.Upgrade upgrade)
    {
        if (!_upgradeKeys.TryGetValue(upgrade, out string key))
        {
            throw new ArgumentException($"Invalid upgrade: {upgrade}");
        }

        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }

        return 0.0f;
    }

    public void SavePrice(UpgradeManager.Upgrade upgrade, int value)
    {
        if (!_priceKeys.TryGetValue(upgrade, out string key))
        {
            throw new ArgumentException($"Invalid upgrade: {upgrade}");
        }

        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public int LoadPrice(UpgradeManager.Upgrade upgrade)
    {
        if (!_priceKeys.TryGetValue(upgrade, out string key))
        {
            throw new ArgumentException($"Invalid price: {upgrade}");
        }

        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key);
        }

        if (key == DOUBLE_LIFE_PRICE_KEY)
        {
            return 10000;
        }
        else
        {
            return 10;
        }
    }

    private void Awake()
    {
        InitializeInstance();
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
}
