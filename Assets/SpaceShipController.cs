using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
    public enum GameMode { SinglePlayer, LocalMultiplayer }
    public GameMode currentGameMode;
    [HideInInspector]
    public string gameMode; 

    public float rotationSpeed = 200f;
    public float thrustPower = 5f;
    public float brakePower = 2f;
    public float maxSpeed = 10f;
    public float handbrakeThreshold = 0.1f;
    public float maxAngularVelocity = 100f;
    public float angularDamping = 1f;

    [HideInInspector]
    public Rigidbody2D rb;
    private Transform shipTransform;
    private float rotationInput;
    private float thrustInput;
    private bool isHandbraking;

    public int playerID = 1; // 1 for Player 1, 2 for Player 2 in local multiplayer

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        shipTransform = rb.transform;
    }

    private void Update()
    {
        HandleInput();
        HandleRotation();
        HandleMovement();
        LimitAngularVelocity();
        ApplyAngularDamping();
    }

    private void HandleInput()
    {
        if (currentGameMode == GameMode.SinglePlayer)
        {
            gameMode = "SinglePlayer"; 
            rotationInput = Input.GetAxis("Horizontal");
            thrustInput = Input.GetAxis("Vertical");
            isHandbraking = Input.GetKey(KeyCode.Space);
        }
        else if (currentGameMode == GameMode.LocalMultiplayer)
        {
            gameMode = "LocalMultiplayer";
            if (playerID == 2)
            {
                rotationInput = Input.GetAxis("Horizontal");
                thrustInput = Input.GetAxis("Vertical");
                isHandbraking = Input.GetKey(KeyCode.Space);
            }
            else if (playerID == 1)
            {
                rotationInput = Input.GetAxis("Horizontal2");
                thrustInput = Input.GetAxis("Vertical2");
                isHandbraking = Input.GetKey(KeyCode.RightShift);
            }
        }
    }

    private void HandleRotation()
    {
        if (rotationInput != 0)
        {
            float rotation = rotationInput * rotationSpeed * Time.deltaTime;
            shipTransform.Rotate(Vector3.forward, -rotation);
        }
    }

    private void HandleMovement()
    {
        if (thrustInput > 0)
        {
            Vector2 thrustDirection = shipTransform.up * thrustPower * Time.deltaTime;
            rb.AddForce(thrustDirection);
        }
        else if (thrustInput < 0)
        {
            Vector2 brakeDirection = -rb.velocity.normalized * brakePower * Time.deltaTime;
            rb.AddForce(brakeDirection);
        }

        if (isHandbraking && rb.velocity.magnitude <= maxSpeed * handbrakeThreshold)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
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
