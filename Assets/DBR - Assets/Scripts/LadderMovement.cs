using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    public float ladderClimbingSpeed; 

    private Rigidbody2D rb2DBase;
    private InputDetector inputDetector;
    private CharacterStates states;
    private Transform tr;
    private HorizontalMovement horizontalMovement; 

    public GameObject detectedLadder;
    public GameObject registeredLadder; 

    public GameObject playerBase; 
    public Collider2D baseCollider;

    public float vVector; 

    // Start is called before the first frame update
    void Start()
    {

        rb2DBase = playerBase.GetComponent<Rigidbody2D>();
        states = GetComponent<CharacterStates>();
        tr = playerBase.GetComponent<Transform>();
        inputDetector = GetComponent<InputDetector>();
        baseCollider = playerBase.GetComponent<Collider2D>();
        horizontalMovement = GetComponent<HorizontalMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        #region Identifying Detected Ladder

        detectedLadder = states.groundDetector.detectedLadder;
        registeredLadder = detectedLadder; 


        #endregion

        #region Ladder Movement

        vVector = inputDetector.vVector; 

        if (states.onLadder)
        {


            ClimbingLadder(); 
            if (rb2DBase.velocity.y != 0)
            {
                states.isClimbingLadder = true; 
            }

            else
            {
                states.isClimbingLadder = false;
            }

                    

        }

        else
        {
            states.isClimbingLadder = false;
            


        }

        #endregion


        #region Ladder Conditions

        if (!states.inFrontOfLadder)
        {
            states.onLadder = false; 
        }

        #endregion


    }

    public void ClimbingLadder()
    {
        rb2DBase.velocity = new Vector2(0f, vVector * ladderClimbingSpeed); //old version of horizontal movement
    }

    public void GettingOnLadder()
    {
        rb2DBase.gravityScale = 0f;
        states.onLadder = true;
        registeredLadder = detectedLadder;

        if (registeredLadder != null)
        {
            tr.position = new Vector2(registeredLadder.GetComponent<Ladder>().ladderCenter, tr.position.y);

            Physics2D.IgnoreCollision(baseCollider, registeredLadder.GetComponent<Ladder>().hatch.GetComponent<BoxCollider2D>(), true);
        }
    }

    public void GettingOffLadder()
    {
        
        rb2DBase.gravityScale = 1f;
        
        Physics2D.IgnoreCollision(baseCollider, registeredLadder.GetComponent<Ladder>().hatch.GetComponent<BoxCollider2D>(), false);
        registeredLadder = null;
        states.onLadder = false;

    }

    public void GettingOffByTheHatch()
    {
        tr.position = new Vector2(registeredLadder.GetComponent<Ladder>().ladderCenter, registeredLadder.transform.position.y + registeredLadder.GetComponent<Ladder>().ladderHeight*0.16f);
        rb2DBase.velocity = new Vector2(rb2DBase.velocity.x, 0f); 
    }
}
