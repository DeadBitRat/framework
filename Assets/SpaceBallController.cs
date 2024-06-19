using UnityEngine;

public class SpaceBallController : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float acceleration = 5f;
    public float deceleration = 2f;
    public float kickForce = 10f;
    public float minKickInterval = 0.5f;

    private Rigidbody2D rb;
    private float lastKickTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Limit ball speed
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }

    public void Kick(Vector2 direction)
    {
        // Ensure the kick is not too frequent
        if (Time.time - lastKickTime < minKickInterval)
            return;

        // Apply force in the specified direction
        rb.AddForce(direction * kickForce, ForceMode2D.Impulse);

        // Update last kick time
        lastKickTime = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Bounce off walls
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector2 reflection = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
            rb.velocity = reflection;
        }
    }

    private void FixedUpdate()
    {
        // Decelerate the ball gradually when not being kicked
        if (rb.velocity.magnitude > 0)
        {
            rb.velocity -= rb.velocity.normalized * deceleration * Time.fixedDeltaTime;
        }
    }
}