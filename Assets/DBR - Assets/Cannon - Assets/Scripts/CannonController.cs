using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public bool cannonActivated;
    public GameObject cannonUser;
    public bool canShoot;
    public float fireRate;

    public GameObject bullet;
    public Transform shootingPoint;

    public Animator anim;

    public bool userDetected;
    public GameObject detectedUser;

    public int numberOfShoots;
    public int shootsLeft;

    public string cannonMode;

    public MathAttackSystem mathAttackSystem;

    public int sortorderOfUser;

    private AudioSource audioSource;
    public AudioClip[] shotSound;
    public AudioClip openingSound;
    public AudioClip closingSound; 



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        canShoot = false;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Activated", cannonActivated);
        anim.SetBool("Can Shoot", canShoot);


        if (Input.GetButtonDown("Function") && userDetected && !cannonActivated)
        {
            
            cannonActivated = true;
            cannonUser = detectedUser;



            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 11; 
            cannonUser.GetComponentInParent<InputDetector>().inputActivated = false;

        }


        if (Input.GetButtonDown("Function") && cannonUser != null && cannonActivated && canShoot)
        {
            
            cannonUser.GetComponentInParent<InputDetector>().inputActivated = true;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 9;
            cannonUser = null;
            canShoot = false;
            cannonActivated = false;


        }


        if (cannonActivated)
        {
            

            if (cannonMode == "normal")
            { 

            if (Input.GetButtonDown("Fire1") && canShoot)
            {
                StartCoroutine(regularShooting(numberOfShoots));
            }

            }


            // CODIGO PARA EL MODO MATH ATTACK AQUI !!!!!!!!!!!!


            if (cannonMode == "MathAttack")

            {
                

                
                    if ( (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && canShoot && mathAttackSystem.isExerciseOver)
                    {
                        
                        mathAttackSystem.GenerateExercise();

                    
                    }

                    
                
            }

            // FIN PARA EL CODIGO PARA EL MODO MATH ATTACK AQUI !!!!!!!!!!!!

        }

        else
        {
            cannonUser.GetComponentInParent<InputDetector>().inputActivated = true;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 9;
            cannonUser = null;
            canShoot = false;
        }
    }


    public void SettingCannonReady()
    {
        canShoot = true;
    }

    public IEnumerator regularShooting(int tiros)
    {
        shootsLeft = tiros;
        canShoot = false; // Prevent shooting during this coroutine

        


        while (shootsLeft > 0)
        {
            GameObject bulletShooted = Instantiate(bullet, shootingPoint.position, shootingPoint.rotation);
            if (gameObject.transform.localScale.x > 0)
            {
                bulletShooted.GetComponent<BulletMovement>().xDirection = "Left";
            }

            else
            {
                bulletShooted.GetComponent<BulletMovement>().xDirection = "Right";
            }
            shootsLeft -= 1;

            yield return new WaitForSeconds(fireRate);

        }

        cannonActivated = false; // Allow shooting again after waiting for 'fireRate' seconds
        yield return null;
    }


    public IEnumerator MathAttackShooting(int tiros)
    {
        shootsLeft = tiros;
        canShoot = false; // Prevent shooting during this coroutine

        yield return new WaitForSeconds(2f);


        while (shootsLeft > 0)
        {

            anim.Play("Cannon-Shooting", -1, 0f);
            PlayRandomShootingSound();
            GameObject bulletShooted = Instantiate(bullet, shootingPoint.position, shootingPoint.rotation);
            if (gameObject.transform.localScale.x > 0)
            {
                bulletShooted.GetComponent<BulletMovement>().xDirection = "Left";
            }

            else
            {
                bulletShooted.GetComponent<BulletMovement>().xDirection = "Right";
            }
            shootsLeft -= 1;

            yield return new WaitForSeconds(fireRate);

        }

        StartCoroutine(mathAttackSystem.WaitingForResults());
        
        //cannonActivated = false; // Allow shooting again after waiting for 'fireRate' seconds

        yield return null;
    }


    void PlayRandomShootingSound()
    {
        // Pick a random sound effect from the array
        AudioClip soundEffect = shotSound[Random.Range(0, shotSound.Length)];

        // Play the selected sound effect without interrupting the previous one
        audioSource.PlayOneShot(soundEffect);
    }

    public void openingSoundEffect()
    {
        audioSource.clip = openingSound;
        audioSource.Play();
    }

    public void closingSoundEffect() 
    {
        audioSource.clip = closingSound;
        audioSource.Play();
    }


    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")

        {
            userDetected = true;
            detectedUser = collision.gameObject;

        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")

        {
            userDetected = false;
            detectedUser = null;

        }
    }

  

}
