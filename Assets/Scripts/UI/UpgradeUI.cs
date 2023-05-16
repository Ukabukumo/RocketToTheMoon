using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnMenuButtonClick = new();
    [HideInInspector] public UnityEvent<UpgradeManager.Upgrade> OnUpgradeButtonClick = new();

    [SerializeField] private Button _menuButton;

    [Header("Upgrade Values Texts")]
    [SerializeField] private TextMeshProUGUI _speedUpValueText;
    [SerializeField] private TextMeshProUGUI _fuelConsumptionValueText;
    [SerializeField] private TextMeshProUGUI _enemySpeedValueText;
    [SerializeField] private TextMeshProUGUI _doubleLifeValueText;

    [Header("Upgrade Prices Texts")]
    [SerializeField] private TextMeshProUGUI _speedUpPriceText;
    [SerializeField] private TextMeshProUGUI _fuelConsumptionPriceText;
    [SerializeField] private TextMeshProUGUI _enemySpeedPriceText;
    [SerializeField] private TextMeshProUGUI _doubleLifePriceText;

    [Header("Upgrade Buttons")]
    [SerializeField] private Button _speedUpUpgradeButton;
    [SerializeField] private Button _fuelConsumptionUpgradeButton;
    [SerializeField] private Button _enemySpeedUpgradeButton;
    [SerializeField] private Button _doubleLifeUpgradeButton;

    private Dictionary<UpgradeManager.Upgrade, 
        (TextMeshProUGUI valueText, TextMeshProUGUI priceText, Button upgradeButton)> _upgradeUIElements;

    private void Awake()
    {
        InitializeUI();
        InitializeListeners();
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    private void InitializeUI()
    {
        _upgradeUIElements = new Dictionary<UpgradeManager.Upgrade,
            (TextMeshProUGUI, TextMeshProUGUI, Button)>
        {
            { UpgradeManager.Upgrade.SPEED_UP,
                (_speedUpValueText, _speedUpPriceText, _speedUpUpgradeButton) },
            { UpgradeManager.Upgrade.FUEL_CONSUMPTION,
                (_fuelConsumptionValueText, _fuelConsumptionPriceText, _fuelConsumptionUpgradeButton) },
            { UpgradeManager.Upgrade.ENEMY_SPEED,
                (_enemySpeedValueText, _enemySpeedPriceText, _enemySpeedUpgradeButton) },
            { UpgradeManager.Upgrade.DOUBLE_LIFE,
                (_doubleLifeValueText, _doubleLifePriceText, _doubleLifeUpgradeButton) }
        };
    }

    private void InitializeListeners()
    {
        _menuButton.onClick.AddListener(OnMenuButtonClick.Invoke);
        _speedUpUpgradeButton.onClick.AddListener(() => 
            OnUpgradeButtonClicked(UpgradeManager.Upgrade.SPEED_UP));
        _fuelConsumptionUpgradeButton.onClick.AddListener(() => 
            OnUpgradeButtonClicked(UpgradeManager.Upgrade.FUEL_CONSUMPTION));
        _enemySpeedUpgradeButton.onClick.AddListener(() => 
            OnUpgradeButtonClicked(UpgradeManager.Upgrade.ENEMY_SPEED));
        _doubleLifeUpgradeButton.onClick.AddListener(() => 
            OnUpgradeButtonClicked(UpgradeManager.Upgrade.DOUBLE_LIFE));
    }

    private void OnUpgradeButtonClicked(UpgradeManager.Upgrade upgrade)
    {
        OnUpgradeButtonClick.Invoke(upgrade);
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < (int)UpgradeManager.Upgrade.COUNT; ++i)
        {
            UpgradeManager.Upgrade upgrade = (UpgradeManager.Upgrade)i;

            if (!_upgradeUIElements.TryGetValue(upgrade, out var elements))
            {
                throw new ArgumentException($"Invalid upgrade: {upgrade}");
            }

            bool canBuy = UpgradeManager.Instance.CanBuy(upgrade);
            bool canUpgrade = UpgradeManager.Instance.CanUpgrade(upgrade);

            elements.upgradeButton.interactable = canBuy;
            elements.priceText.gameObject.SetActive(canUpgrade);
            elements.upgradeButton.gameObject.SetActive(canUpgrade);

            float upgradeValue = SaveDataManager.Instance.LoadUpgrade(upgrade);
            string valueText;

            if (upgrade == UpgradeManager.Upgrade.DOUBLE_LIFE)
            {
                valueText = (upgradeValue == 1.0f ? "ON" : "OFF");
            }
            else
            {
                valueText = Convert.ToString(upgradeValue) + "%";
            }

            elements.valueText.text = valueText;
            elements.priceText.text = Convert.ToString(SaveDataManager.Instance.LoadPrice(upgrade));
        }
    }
}
