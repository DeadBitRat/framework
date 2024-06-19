using UnityEngine;

public class ShipShootingController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootingPoint;
    public float projectileSpeed = 10f;
    public float fireRate = 0.5f;
    public float speedBulletShipEffect;

    private float nextFireTime;

    private SpaceShipController shipController;

    private void Start()
    {
        shipController = GetComponent<SpaceShipController>();
    }


    private void Update()
    {
        // Check if the fire button is pressed and if the ship can fire
        if (shipController.gameMode == "SinglePlayer")
        {


            if (Input.GetButton("Fire1") && Time.time > nextFireTime)
            {
                FireProjectile();
            }
        }

        if (shipController.gameMode == "LocalMultiplayer")
        {
            if(shipController.playerID == 1)
            {
                if (Input.GetButton("Function") && Time.time > nextFireTime)
                {
                    FireProjectile();
                }
            }
            else if (shipController.playerID == 2)
            {
                if (Input.GetButton("Fire2") && Time.time > nextFireTime)
                {
                    FireProjectile();
                }
            }
        }

    }


    private void FireProjectile()
    {
        // Instantiate the projectile at the shooting point
        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, shootingPoint.rotation);

        // Get the Rigidbody2D component of the projectile and set its velocity
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        Rigidbody2D rbShip = shipController.rb;

        if (rb != null)
        {
            rb.velocity = shootingPoint.up * (projectileSpeed);
        }

        // Set the next fire time
        nextFireTime = Time.time + fireRate;
    }

}
