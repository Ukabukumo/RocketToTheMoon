using UnityEngine;

public class BorderDetection : MonoBehaviour
{
    [SerializeField] private Vector2 _borders = new Vector2(10.0f, 10.0f);
    [SerializeField] private GameObject _cameraObject;

    private void Update()
    {
        CheckBorderCrossing();
    }

    private void CheckBorderCrossing()
    {
        Vector3 position = transform.position;

        if (Mathf.Abs(transform.position.x) > _borders.x ||
            _cameraObject.transform.position.y - transform.position.y > _borders.y)
        {
            Destroy(gameObject);
        }
    }
}
