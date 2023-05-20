using UnityEngine;
using UnityEngine.Events;

public class BeyondScreenDetection : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnScreenBottomReached = new();

    [SerializeField] private Camera _cameraObject;
    [SerializeField] private float borderOffset = 0.6f;

    private void OnBecameInvisible()
    {
        if (_cameraObject == null)
        {
            return;
        }

         // Offset from screen border
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
            transform.position = new Vector3(-transform.position.x - borderOffset, transform.position.y, transform.position.z);
        }
        // Crossing right border
        else if (transform.position.x > 0.0f)
        {
            // Move to left border with offset
            transform.position = new Vector3(-transform.position.x + borderOffset, transform.position.y, transform.position.z);
        }
    }
}
