using UnityEngine;

public class SmoothCameraMover : MonoBehaviour
{
    public Transform targetPosition; // The position to move the camera to
    public float duration = 2.0f;    // The duration of the movement

    private Vector3 startPosition;
    private float elapsedTime = 0f;
    private bool isMoving = false;

    void Update()
    {
        if (isMoving)
        {
            Time.timeScale = 0f; 
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.position = Vector3.Lerp(startPosition, targetPosition.position, t);

            if (t >= 1f)
            {
                Time.timeScale = 1f;
                isMoving = false;
            }
        }
    }

    public void StartMoving()
    {
        startPosition = transform.position;
        elapsedTime = 0f;
        isMoving = true;
    }

    public void SetTargetPosition(Transform newTargetPosition)
    {
        targetPosition = newTargetPosition;
    }
}
