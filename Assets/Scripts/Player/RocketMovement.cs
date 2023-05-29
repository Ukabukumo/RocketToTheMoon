using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    [SerializeField] private ControlArm _controlArm;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _flame;

    private const float ROTATION_LIMIT = 60.0f;
    private const float ROTATION_SPEED = 3.0f;
    private const float ROCKET_SPEED_UP = 650.0f;
    private const float MAX_ROCKET_SPEED = 5.0f;

    private Rigidbody2D _rb;
    private float _speedUpUpgrade = 0.0f;
    private float _currentSpeed;
    private Vector3 _previousPosition = Vector3.zero;

    public float GetCurrentSpeed()
    {
        return _currentSpeed;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        InitializeListeners();
    }

    private void FixedUpdate()
    {
        if (_controlArm.isActiveAndEnabled &&
            _controlArm.IsControlling())
        {
            Movement();
            ActivateFlame();
            AudioManager.Instance.PlayRocketSound();
        }
        else
        {
            DeactivateFlame();
            AudioManager.Instance.StopRocketSound();
        }

        CalculateCurrentSpeed();
    }

    private void InitializeListeners()
    {
        GameManager.Instance.OnStartGame.AddListener(() => InitializeMovement());
        GameManager.Instance.OnRevive.AddListener(() => OnRevive());
        GameManager.Instance.OnRestartGame.AddListener(() => OnRestartGame());
    }

    private void InitializeMovement()
    {
        _speedUpUpgrade = ROCKET_SPEED_UP * SaveDataManager.Instance.LoadUpgrade(UpgradeManager.Upgrade.SPEED_UP) / 100.0f;
    }

    private void Movement()
    {
        float force = (ROCKET_SPEED_UP + _speedUpUpgrade) * _rb.mass;
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, ROTATION_LIMIT * _controlArm.GetArmValue());
        rotation = Quaternion.Lerp(transform.rotation, rotation, ROTATION_SPEED * Time.fixedDeltaTime);
        Vector3 direction = Vector3.up;

        // Rotate rocket
        transform.rotation = rotation;

        // Move rocket
        _rb.AddForce(rotation * direction * force * Time.fixedDeltaTime);

        // Limit rocket speed
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, MAX_ROCKET_SPEED);
    }

    private void ActivateFlame()
    {
        _flame.SetActive(true);
    }

    private void DeactivateFlame()
    {
        _flame.SetActive(false);
    }

    private void StopMovement()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = 0.0f;
        DeactivateFlame();
    }

    private void CalculateCurrentSpeed()
    {
        // Calculate distance passed per 1 second
        Vector3 positionDifference = transform.position - _previousPosition;
        float yAxisDifference = positionDifference.y;
        yAxisDifference = yAxisDifference < 0.0f ? 0.0f : yAxisDifference;
        _currentSpeed = yAxisDifference / Time.fixedDeltaTime;
        _previousPosition = transform.position;
    }

    private void OnRevive()
    {
        StopMovement();

        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 rocketRespawnPosition = new(cameraPosition.x, cameraPosition.y, transform.position.z);
        transform.position = rocketRespawnPosition;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    private void OnRestartGame()
    {
        StopMovement();

        transform.position = _spawnPoint.position;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
}
