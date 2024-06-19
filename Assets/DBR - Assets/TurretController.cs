using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public bool detectingEnemy; 
    public TurretDetector detector;

    public Animator animator;
    public AnimationClip openingAnimation; 
    public AnimationClip closingAnimation;

    public GameObject bulletPrefab;
    public Transform shootingPoint;

    public float shootingCadence;

    public float preparingTime;
    public float shootsPerRound;

    public bool readyToShoot;
    public bool gunOpened; 

    public bool playingAnimation; 

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        detectingEnemy = detector.detectingPlayer; 

        if (detectingEnemy)
        {
            // Asigna la animación a la variable del Animator.

            if (playingAnimation == false && gunOpened == false) { 
                animator.Play("Turret1-Opening");

            }
            if (readyToShoot && gunOpened)
            {
                StartCoroutine(TakingAShoot());
            }

        }

        else
        {
           if (playingAnimation == false)
            {
                animator.Play("Turret1-Closing");
            }
            
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            playingAnimation = true; 
        }
        else
        {
            
            playingAnimation = false; 
        }


    }


    public void Shooting()
    {
        readyToShoot = false;
        
        GameObject proyectil = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);  // Creating the Bullet
        BulletMovement bulletmovement = proyectil.GetComponent<BulletMovement>(); // Getting into the bullet  movement script 

        if (gameObject.transform.localScale.x == -1)
        {
            bulletmovement.xDirection = "Right";  //Setting the direction of the bullet
        }

        else
        {
            bulletmovement.xDirection = "Left";  //Setting the direction of the bullet
        }
       
    }


    IEnumerator TakingAShoot()
    {
        Debug.Log("Disparading desde la Corrutina");
        animator.Play("Turret1-Shooting");

        yield return new WaitForSeconds(shootingCadence); // Wait for shootingCadence seconds
        readyToShoot = true;

    }

    public void SettingUpCannon()
    {
        readyToShoot = true;
        gunOpened = true; 
    }

    public void SettingDownCannon()
    {
        readyToShoot = false;
        gunOpened = false;
    }


}
