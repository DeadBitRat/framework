using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZombieWalkBehavior : MonoBehaviour
{
    public GameObject zombieBase; 
    public ZombieVictimDetector detector;

    public float interval = 2f; // Time interval between movements
    public Vector2 movementRange = new Vector2(2f, 2f); // Range for random movement
    public float speed; 

    private Rigidbody2D rb;

    public bool movingRandomly;
    public bool lurking = false;
    public bool walking;

    public bool chasingVictim;
    public Vector2 victimLocation;
    public float chaseSpeedFactor; 

    private float randomX; 
    private float randomY;

    void Start()
    {
        rb = zombieBase.GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        if (movingRandomly && !detector.detectedVictim)
        {
            
            if (!lurking && !walking) 
            {
                StartCoroutine(MoveRandomly());   

            }

            else if (lurking)
            {
                StartCoroutine(Lurk());
            }



            if (walking)
            {
                Vector2 direction = new Vector2(randomX, randomY);
                rb.velocity = direction.normalized * speed;
                
            }

            else
            {
                rb.velocity = new Vector2(0f, 0f);
            }

            
        }

        if (detector.detectedVictim)
        {
            StopAllCoroutines();
            victimLocation = detector.detectedVictim.transform.position; 
            Vector2 zombiePosition = new Vector2(zombieBase.transform.position.x, zombieBase.transform.position.y);
            // Calculate the direction vector from the Rigidbody2D to the baseTransform
            Vector2 direction = (victimLocation - zombiePosition).normalized;
            Debug.Log("La ubicación de la victima es: " + victimLocation); 
            Debug.Log("La ubicación del Zombie es: " + zombiePosition);
            Debug.Log("Direction: " + direction);
          

            rb.velocity = direction *  speed * chaseSpeedFactor;
            
        }

        else
        {
            //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y); 
        }


    }

    IEnumerator MoveRandomly()
    {
        
        walking = true; 
        ChooseRandomPoint(); 
        
        yield return new WaitForSeconds(interval);
        walking = false;
        lurking = true;
        
    }

    IEnumerator Lurk()
    {
        lurking = true; 
        yield return new WaitForSeconds(interval);
        lurking = false;
    }
   

    public void ChooseRandomPoint()
    {
        // Generate random velocities within the specified range
        randomX = Random.Range(-movementRange.x, movementRange.x);
        randomY = Random.Range(-movementRange.y, movementRange.y);
    }



}
