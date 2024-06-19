using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        // Get the main camera's transform
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Get the rotation of the camera
        Vector3 targetPosition = cameraTransform.position;
        targetPosition.y = transform.position.y; // Keep the y position of the text the same

        // Look at the camera's position
        transform.LookAt(targetPosition);

        // Optionally, you can make the text always face the camera directly
        // transform.rotation = Quaternion.LookRotation(cameraTransform.forward);
    }
}
