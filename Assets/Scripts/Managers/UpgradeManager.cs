using UnityEngine;
using System;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    private const float PRICE_COEFFICIENT = 0.3f;
    private const float MAX_SPEED_UP_UPGRADE = 25.0f;
    private const float MAX_FUEL_CONSUMPTION_UPGRADE = 25.0f;
    private const float MAX_OBSTACLE_SPEED_UPGRADE = 25.0f;
    private const float MAX_DOUBLE_LIFE_UPGRADE = 1.0f;

    private readonly Dictionary<Upgrade, float> _maxUpgradeValues = new Dictionary<Upgrade, float>
    {
        { Upgrade.SPEED_UP, MAX_SPEED_UP_UPGRADE },
        { Upgrade.FUEL_CONSUMPTION, MAX_FUEL_CONSUMPTION_UPGRADE },
        { Upgrade.OBSTACLE_SPEED, MAX_OBSTACLE_SPEED_UPGRADE },
        { Upgrade.DOUBLE_LIFE, MAX_DOUBLE_LIFE_UPGRADE }
    };

    public enum Upgrade
    {
        SPEED_UP,
        FUEL_CONSUMPTION,
        OBSTACLE_SPEED,
        DOUBLE_LIFE,
        COUNT
    }

    public bool SkillUpgradeable(Upgrade upgrade)
    {
        return (CanBuy(upgrade) && CanUpgrade(upgrade));
    }

    public bool CanBuy(Upgrade upgrade)
    {
        int starsAmount = SaveDataManager.Instance.LoadStars();
        int upgradePrice = SaveDataManager.Instance.LoadPrice(upgrade);

        return (upgradePrice <= starsAmount);
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
        SaveDataManager.Instance.AddStars(-price);
    }

    private int UpdatePrice(int price)
    {
        return (int)Mathf.Ceil(price + price * PRICE_COEFFICIENT);
    }
}
