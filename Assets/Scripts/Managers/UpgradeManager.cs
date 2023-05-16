using UnityEngine;
using System;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    private float _priceCoefficient = 0.3f;
    private const float MAX_SPEED_UP_UPGRADE = 25.0f;
    private const float MAX_FUEL_CONSUMPTION_UPGRADE = 25.0f;
    private const float MAX_ENEMY_SPEED_UPGRADE = 25.0f;
    private const float MAX_DOUBLE_LIFE_UPGRADE = 1.0f;

    private readonly Dictionary<Upgrade, float> _maxUpgradeValues = new Dictionary<Upgrade, float>
    {
        { Upgrade.SPEED_UP, MAX_SPEED_UP_UPGRADE },
        { Upgrade.FUEL_CONSUMPTION, MAX_FUEL_CONSUMPTION_UPGRADE },
        { Upgrade.ENEMY_SPEED, MAX_ENEMY_SPEED_UPGRADE },
        { Upgrade.DOUBLE_LIFE, MAX_DOUBLE_LIFE_UPGRADE }
    };

    public enum Upgrade
    {
        SPEED_UP,
        FUEL_CONSUMPTION,
        ENEMY_SPEED,
        DOUBLE_LIFE,
        COUNT
    }

    public bool SkillUpgradeable(Upgrade upgrade)
    {
        int coinsAmount = SaveDataManager.Instance.LoadCoins();
        float upgradeValue = SaveDataManager.Instance.LoadUpgrade(upgrade);
        int upgradePrice = SaveDataManager.Instance.LoadPrice(upgrade);

        if (!_maxUpgradeValues.TryGetValue(upgrade, out float maxUpgradeValue))
        {
            throw new ArgumentException($"Invalid upgrade: {upgrade}");
        }

        return ((upgradePrice <= coinsAmount) && (upgradeValue < maxUpgradeValue));
    }

    public bool CanBuy(Upgrade upgrade)
    {
        int coinsAmount = SaveDataManager.Instance.LoadCoins();
        int upgradePrice = SaveDataManager.Instance.LoadPrice(upgrade);

        return (upgradePrice <= coinsAmount);
    }

    public bool CanUpgrade(Upgrade upgrade)
    {
        float upgradeValue = SaveDataManager.Instance.LoadUpgrade(upgrade);

        if (!_maxUpgradeValues.TryGetValue(upgrade, out float maxUpgradeValue))
        {
            throw new ArgumentException($"Invalid upgrade: {upgrade}");
        }

        return (upgradeValue < maxUpgradeValue);
    }

    private void Awake()
    {
        InitializeInstance();
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

    private void InitializeListeners()
    {
        GameManager.Instance.OnUpgradeSkill.AddListener((Upgrade upgrade) => UpgradeSkill(upgrade));
    }

    private float UpgradeSkill(Upgrade upgrade)
    {
        float upgradeValue = SaveDataManager.Instance.LoadUpgrade(upgrade);
        int upgradePrice = SaveDataManager.Instance.LoadPrice(upgrade);

        upgradeValue += 1.0f;
        BuyUpgrade(upgradePrice);
        upgradePrice = UpdatePrice(upgradePrice);

        SaveDataManager.Instance.SaveUpgrade(upgrade, upgradeValue);
        SaveDataManager.Instance.SavePrice(upgrade, upgradePrice);

        return upgradeValue;
    }

    private void BuyUpgrade(int price)
    {
        SaveDataManager.Instance.AddCoins(-price);
    }

    private int UpdatePrice(int price)
    {
        return (int)Mathf.Ceil(price + price * _priceCoefficient);
    }
}
