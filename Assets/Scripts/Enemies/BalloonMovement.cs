using UnityEngine;

public class BalloonMovement : MonoBehaviour
{
    private float _balloonSpeed = 2.0f;
    private float _rotationLimit = 80.0f;
    private Quaternion _rotation;
    private Vector3 _direction;
    private float _rotationCoefficient;
    private float _enemySpeedUpgrade;

    private void Start()
    {
        InitializeMovement();
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
            _rotationCoefficient = Random.Range(0.3f, 1.0f);
        }
        else if (transform.position.x > 0.0f)
        {
            _rotationCoefficient = Random.Range(-1.0f, -0.3f);
        }

        _rotation = Quaternion.Euler(0.0f, 0.0f, _rotationLimit * _rotationCoefficient);
        _direction = Vector3.down;
        UpgradeManager.Upgrade upgrade = UpgradeManager.Upgrade.ENEMY_SPEED;
        _enemySpeedUpgrade = _balloonSpeed * SaveDataManager.Instance.LoadUpgrade(upgrade) / 100.0f;
    }

    private void Movement()
    {
        transform.Translate(_rotation * _direction * (_balloonSpeed - _enemySpeedUpgrade) * Time.fixedDeltaTime);
    }
}
