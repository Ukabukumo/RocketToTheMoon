using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class FuelConsumption : MonoBehaviour
{
    public UnityEvent OnFuelRunOutEvent = new();

    [SerializeField] private float _maxFuel = 100.0f;
    [SerializeField] private float _fuelConsumption = 1.0f;
    [SerializeField] private ControlArm controlArm;

    private float _currentFuel;
    private float _fuelConsumptionUpgrade;
    private Coroutine _consumption;

    public float GetFuel()
    {
        float normalizedFuel = _currentFuel / _maxFuel;
        return normalizedFuel;
    }

    public void AddFuel(float fuelPercentage)
    {
        _currentFuel += _maxFuel * fuelPercentage;
        _currentFuel = _currentFuel > _maxFuel ? _maxFuel : _currentFuel;
    }

    private void Awake()
    {
        InitializeListeners();
    }

    private void InitializeListeners()
    {
        GameManager.Instance.OnStartGame.AddListener(() => StartConsumption());
        GameManager.Instance.OnLossGame.AddListener(() => StopConsumption());
        GameManager.Instance.OnRevive.AddListener(() => RestartConsumption());
    }

    private void StartConsumption()
    {
        _currentFuel = _maxFuel;
        _fuelConsumptionUpgrade = _fuelConsumption * SaveDataManager.Instance.LoadUpgrade(UpgradeManager.Upgrade.FUEL_CONSUMPTION) / 100.0f;
        _consumption = StartCoroutine(Consumption());
        GameManager.Instance.OnFuelChange.Invoke();
    }

    private void StopConsumption()
    {
        StopCoroutine(_consumption);
    }

    private void RestartConsumption()
    {
        StopConsumption();
        StartConsumption();
    }

    private IEnumerator Consumption()
    {
        while (_currentFuel > 0.0f)
        {
            if (controlArm.IsControlling())
            {
                _currentFuel -= (_fuelConsumption - _fuelConsumptionUpgrade) * Time.deltaTime;
                GameManager.Instance.OnFuelChange.Invoke();
            }

            yield return null;
        }

        OnFuelRunOutEvent.Invoke();
    }
}
