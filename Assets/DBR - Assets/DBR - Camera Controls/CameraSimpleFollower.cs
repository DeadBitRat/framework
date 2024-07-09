using UnityEngine;

public class SmoothCameraFollower : MonoBehaviour
{
    public Transform target; // The target for the camera to follow
    public float smoothSpeed = 0.125f; // Speed of the camera movement
    public Vector3 offset; // Offset between the camera and the target

    public bool hasLimits; // Whether the camera has movement limits
    public float leftLimit; // Left boundary
    public float rightLimit; // Right boundary
    public float upperLimit; // Upper boundary
    public float bottomLimit; // Bottom boundary

    private Vector3 velocity = Vector3.zero; // SmoothDamp velocity

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target not assigned. Please assign a target Transform in the inspector.");
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Desired position of the camera
            Vector3 desiredPosition = target.position + offset;
            

            // Smoothly move the camera towards the desired position
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            

            // Clamp the camera's position to stay within the defined limits
            if (hasLimits)
            {
                smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, leftLimit, rightLimit);
                smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, bottomLimit, upperLimit);
               
            }

            // Set the camera's position
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
        else
        {
            Debug.LogWarning("Target is null. Please assign a target Transform in the inspector.");
        }
    }
}
