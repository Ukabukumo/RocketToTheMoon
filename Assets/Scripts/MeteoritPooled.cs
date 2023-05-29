using UnityEngine;

public class MeteoritPooled : MonoBehaviour
{
    [SerializeField] private Vector2 _borders = new(10.0f, 10.0f);

    public void ReturnToPool()
    {
        MeteoritPool.Instance.ReturnToPool(this);
    }

    private void Update()
    {
        CheckBorders();
    }

    private void CheckBorders()
    {
        Vector3 objectPosition = transform.position;
        Vector3 cameraPosition = Camera.main.transform.position;

        if (Mathf.Abs(objectPosition.x) > _borders.x ||
            cameraPosition.y - objectPosition.y > _borders.y)
        {
            ReturnToPool();
        }
    }
}
