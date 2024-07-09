using UnityEngine;

public class CPUShipController : MonoBehaviour
{
    public Transform target; // The target the CPU ship will move towards (e.g., the ball)
    public Transform ball; // The ball the CPU ship will try to shoot
    public Transform redGoal; // The red goal area where the CPU will aim to shoot the ball

    public float rotationSpeed = 200f;
    public float thrustPower = 5f;
    public float brakePower = 2f;
    public float maxSpeed = 10f;
    public float handbrakeThreshold = 0.1f;
    public float maxAngularVelocity = 100f;
    public float angularDamping = 1f;
    public float stoppingDistance = 2f; // Distance at which the ship stops moving towards the target
    public float shootingDistance = 3f; // Distance at which the ship will attempt to shoot the ball
    public float shootingForce = 10f; // Force applied to shoot the ball

    private Rigidbody2D rb;
    private Transform shipTransform;

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        shipTransform = rb.transform;
    }

    private void Update()
    {
        if (target != null)
        {
            MoveTowardsTarget();
        }

        if (ball != null && redGoal != null)
        {
            TryShootBall();
        }

        LimitAngularVelocity();
        ApplyAngularDamping();
    }

    private void MoveTowardsTarget()
    {
        Vector2 directionToTarget = (Vector2)target.position - rb.position;
        float distanceToTarget = directionToTarget.magnitude;

        // Rotate towards target
        float angleToTarget = Vector2.SignedAngle(shipTransform.up, directionToTarget);
        float rotationInput = Mathf.Clamp(angleToTarget / 180f, -1f, 1f);
        float rotation = rotationInput * rotationSpeed * Time.deltaTime;
        shipTransform.Rotate(Vector3.forward, -rotation);

        // Move towards target if not within stopping distance
        if (distanceToTarget > stoppingDistance)
        {
            Vector2 thrustDirection = shipTransform.up * thrustPower * Time.deltaTime;
            rb.AddForce(thrustDirection);
        }
        else
        {
            // Apply braking force to stop near the target
            Vector2 brakeDirection = -rb.velocity.normalized * brakePower * Time.deltaTime;
            rb.AddForce(brakeDirection);

            // Optionally stop the ship completely when very close to the target
            if (rb.velocity.magnitude <= maxSpeed * handbrakeThreshold)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0;
            }
        }
    }

    private void TryShootBall()
    {
        float distanceToBall = Vector2.Distance(rb.position, ball.position);

        if (distanceToBall <= shootingDistance)
        {
            Vector2 directionToGoal = (redGoal.position - ball.position).normalized;
            ball.GetComponent<Rigidbody2D>().AddForce(directionToGoal * shootingForce, ForceMode2D.Impulse);
        }
    }

    private void LimitAngularVelocity()
    {
        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxAngularVelocity, maxAngularVelocity);
    }

    private void ApplyAngularDamping()
    {
        rb.angularVelocity *= (1 - angularDamping * Time.deltaTime);
    }
}
