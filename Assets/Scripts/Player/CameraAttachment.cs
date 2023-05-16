using UnityEngine;

public class CameraAttachment : MonoBehaviour
{
    [SerializeField] private Camera cameraObject;

    private void LateUpdate()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        if (transform.position.y > cameraObject.transform.position.y)
        {
            Vector3 newCameraPosition = 
                new Vector3(cameraObject.transform.position.x, transform.position.y, cameraObject.transform.position.z);
            cameraObject.transform.position = newCameraPosition;
        }
    }
}
