using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    public SmoothCameraMover cameraMover;
    public Transform newTarget;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Just an example trigger
        {
            cameraMover.SetTargetPosition(newTarget);
            cameraMover.StartMoving();
        }
    }
}
