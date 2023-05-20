using UnityEngine;

public class WarningSign : MonoBehaviour
{
    [SerializeField] private GameObject _warningSignPrefab;
    [SerializeField] private float offset = 0.5f;

    private GameObject _warningSign;

    private void Start()
    {
        Vector3 signPosition = transform.position;
        Vector3 obstaclePosition = Camera.main.WorldToScreenPoint(signPosition);

        if (IsBeyondScreen(obstaclePosition))
        {
            signPosition = CalculateSignPosition(obstaclePosition);
            _warningSign = Instantiate(_warningSignPrefab, signPosition, Quaternion.identity);
            _warningSign.transform.rotation = LookAtObstacle();
        }
    }

    private void Update()
    {
        if (_warningSign != null)
        {
            Vector3 obstaclePosition = Camera.main.WorldToScreenPoint(transform.position);
            _warningSign.transform.position = CalculateSignPosition(obstaclePosition);
            _warningSign.transform.rotation = LookAtObstacle();
        }
    }

    private void OnBecameVisible()
    {
        if (_warningSign != null)
        {
            Destroy(_warningSign);
        }
    }

    private void OnDestroy()
    {
        if (_warningSign != null)
        {
            Destroy(_warningSign);
        }
    }

    private bool IsBeyondScreen(Vector3 obstaclePosition)
    {
        if (obstaclePosition.x < 0.0f || obstaclePosition.x > Screen.width ||
            obstaclePosition.y < 0.0f || obstaclePosition.y > Screen.height)
        {
            return true;
        }

        return false;
    }

    private Vector3 CalculateSignPosition(Vector3 obstaclePosition)
    {
        Vector3 signPosition = Camera.main.ScreenToWorldPoint(obstaclePosition);
        Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new(Screen.width, Screen.height));

        if (obstaclePosition.x > Screen.width)
        {
            signPosition.x = screenPosition.x - offset;
        }
        else if (obstaclePosition.x < 0.0f)
        {
            signPosition.x = Camera.main.ScreenToWorldPoint(new(0.0f, 0.0f)).x + offset;
        }

        if (obstaclePosition.y > Screen.height)
        {
            signPosition.y = screenPosition.y - offset;
        }
        else if (obstaclePosition.y < 0.0f)
        {
            signPosition.y = Camera.main.ScreenToWorldPoint(new(0.0f, 0.0f)).y + offset;
        }

        return signPosition;
    }

    private Quaternion LookAtObstacle()
    {
        if (_warningSign == null)
        {
            return Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }

        Vector3 direction = transform.position - _warningSign.transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        return rotation;
    }
}
