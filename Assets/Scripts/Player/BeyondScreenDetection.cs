using UnityEngine;
using UnityEngine.Events;

public class BeyondScreenDetection : MonoBehaviour
{
    public UnityEvent OnScreenBottomReached = new();

    [SerializeField] private Camera _cameraObject;

    private void OnBecameInvisible()
    {
        if (_cameraObject == null)
        {
            return;
        }

        float borderOffset = 0.1f; // Offset from screen border

        Vector3 screenPosition = _cameraObject.WorldToScreenPoint(transform.position); // This object position in screen coordinates

        // Crossing bottom border
        if (screenPosition.y < 0.0f)
        {
            OnScreenBottomReached.Invoke();
        }

        // Crossing left border
        if (transform.position.x < 0.0f)
        {
            // Move to right border with offset
            transform.position = new Vector2(-transform.position.x - borderOffset, transform.position.y);
        }
        // Crossing right border
        else if (transform.position.x > 0.0f)
        {
            // Move to left border with offset
            transform.position = new Vector2(-transform.position.x + borderOffset, transform.position.y);
        }
    }
}
