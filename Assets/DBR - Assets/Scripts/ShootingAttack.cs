using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAttack : MonoBehaviour
{
    public bool shootingEnabled; 
    public bool isShooting;
    public bool canShoot;

    public Transform shootingPoint;
    public float shootingTimer; 

    public float secondsPerRound;
    public float secondsToLowerGun; 

    private InputDetector inputDetector;
    private CharacterStates states; 

    public GameObject bulletPrefab; 


    // Start is called before the first frame update
    void Start()
    {
        canShoot = true; 
        states = GetComponent<CharacterStates>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isShooting)
        {
            shootingTimer += Time.deltaTime;
        }

        if (shootingTimer > secondsPerRound + secondsToLowerGun)
        {
            isShooting = false; 
        }
    }

    public void Shooting(string xDirection)
    {
        if (shootingEnabled) { 
        if (canShoot)
        {
            StartCoroutine(ShootForDuration(secondsPerRound));
            GameObject proyectil = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);  // Creating the Bullet
            BulletMovement bulletmovement = proyectil.GetComponent<BulletMovement>(); // Getting into the bullet  movement script 
            bulletmovement.xDirection = xDirection;  //Setting the direction of the bullet
            


            
        }
        }
    }

    public IEnumerator ShootForDuration(float duration)
    {
        shootingTimer = 0f;
        isShooting = true; 
        
        
       
        isShooting = true;
        canShoot = false; 

        yield return new WaitForSeconds(duration);
        canShoot = true;

        yield return new WaitForSeconds(secondsToLowerGun);

        
       
    }
}
