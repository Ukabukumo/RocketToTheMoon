using UnityEngine;

public class MeteoritMovement : MonoBehaviour
{
    [SerializeField] private GameObject _flamePrefab;
    [SerializeField] private float _flameOffset = 1.0f;
    [SerializeField] private float _flameScaleCoefficient = 3.0f;
    [SerializeField] private float _deadZone = 0.3f;
    [SerializeField] private float _meteoritSpeed = 2.0f;
    [SerializeField] private float _rotationLimit = 80.0f;

    private Quaternion _rotation;
    private Vector3 _direction;
    private float _rotationCoefficient;
    private float _obstacleSpeedUpgrade;

    private void Start()
    {
        InitializeMovement();
        InitializeFlame();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void InitializeMovement()
    {
        _rotationCoefficient = Random.Range(-1.0f, 1.0f);

        if (transform.position.x < 0.0f)
        {
            _rotationCoefficient = Random.Range(_deadZone, 1.0f);
        }
        else if (transform.position.x > 0.0f)
        {
            _rotationCoefficient = Random.Range(-1.0f, -_deadZone);
        }

        _rotation = Quaternion.Euler(0.0f, 0.0f, _rotationLimit * _rotationCoefficient);
        _direction = Vector3.down;
        UpgradeManager.Upgrade upgrade = UpgradeManager.Upgrade.OBSTACLE_SPEED;
        _obstacleSpeedUpgrade = _meteoritSpeed * SaveDataManager.Instance.LoadUpgrade(upgrade) / 100.0f;
    }

    private void InitializeFlame()
    {
        Vector3 movementDirection = _rotation * _direction;
        Vector3 oppositePosition = transform.position - movementDirection * _flameOffset;

        GameObject flameObject = Instantiate(_flamePrefab, oppositePosition, _rotation, transform);
        flameObject.transform.localScale *= _flameScaleCoefficient;
        
        // Change vision level
        flameObject.transform.localPosition += new Vector3(0.0f, 0.0f, 1.0f);
    }

    private void Movement()
    {
        transform.Translate(_rotation * _direction * (_meteoritSpeed - _obstacleSpeedUpgrade) * Time.fixedDeltaTime);
    }
}
