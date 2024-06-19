using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private HorizontalMovement horizontalMovement;
    private InputDetector inputDetector;

    public float horizontalAirSpeed;
    public float hAirSpeedFactor; 

    public bool sameAsOnGround; 

    public float verticalJumpSpeed;

    public float walkingJumpForce;
    public float runningJumpForce; 

    [SerializeField]
    private GameObject playerBase;

    [SerializeField]
    private GameObject character;

   
    
    public CharacterStates states;

    [Header("States")]
    public bool inTheAir;
    public bool jumpReleased; 

    [Header("Actions")]
    public bool isJumping;
    public bool isSquatting; 

    public bool isRising;
    public bool isReachingPeak;
    public bool isFalling;

    private Rigidbody2D rb2DBase;

    public bool canJump; 
    public bool canDoubleJump;
    public bool canPowerJump;
    public bool canPowerBackwardsJump;
    public bool canSlideJump; 


    //These advanced 2D platformer techniques will be considered: 
    // Variable jump height
    // Apex modifiers
    // Jump Buffering
    // Coyote time
    // Clamped fall speed
    // Ledge detection

    public float coyoteTime;
    
    
    public float coyoteCounter;

    
    public float jumpCounter;

    public float jumpBufferLenght;

    
    public float jumpBufferCount;

    public float squattingTime;

    



    // Start is called before the first frame update
    void Start()
    {
        rb2DBase = playerBase.GetComponent<Rigidbody2D>();
        horizontalMovement = GetComponent<HorizontalMovement>(); 
        states = GetComponent<CharacterStates>();
        inputDetector = GetComponent<InputDetector>();  
    }

    // Update is called once per frame
    void Update()
    {
        #region Jump Counter

        if (states.onGround && rb2DBase.velocity.y == 0 && !isSquatting)
        {

            if (canDoubleJump)
            {
                jumpCounter = 2;
            }

            else
            {
                jumpCounter = 1;
            }


        }

        #endregion

        #region Coyote Time    

        // Coyote Time: Time the character can still jump after leaving a platform. 

        if (states.onGround && jumpCounter >= 1)
        {
            coyoteCounter = coyoteTime; 
        }

        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        #endregion

        #region Jump Buffer

        // Code is in input detector script... sadly. 


        #endregion

        #region Jump Force

        if (horizontalMovement.isRunning)
        {
            verticalJumpSpeed = runningJumpForce; 
        }

        else
        {
            verticalJumpSpeed = walkingJumpForce; 
        }

        #endregion

        #region Jump Actions

        if (jumpCounter == 0 && !states.onLadder || isSquatting)
        {
            isJumping = true;
            states.currentAction = "Jumping";
            
        }

        else
        {
            isJumping = false;
            
        }

        if (isJumping && rb2DBase.velocity.y > 0.1f)
        {
            
            isRising = true;
            states.currentAction = "Jumping-Rising";
        }

        else
        {
           isRising = false; 
        }

        if (isJumping && rb2DBase.velocity.y < 1f && rb2DBase.velocity.y > -1f  && inTheAir && states.currentAction == "Jumping-Rising")
        {
            isReachingPeak = true;
            states.currentAction = "JumpPeak";
                   }

        else
        {
            isReachingPeak = false;
        }

        if (inTheAir && rb2DBase.velocity.y < -0.1f )
        {
            isFalling = true;
            states.currentAction = "Falling";
        }

        else
        {
            isFalling= false;
        }




        #endregion

        #region Jump States

        if (states.onGround == false && states.onLadder == false)
        {
            inTheAir = true;
            MovingHorizontallyInTheAir();
        }

        else
        {
            inTheAir = false;
        }

        #endregion

        #region Physics Conditions

        if (jumpReleased)
        {
            ReleasingJump(); 
           
        }


        #region About Horizontal Movement in the Air
      
    

                #endregion

            #endregion
    }

    #region Jump Method
    public void Jumping()
    {
        isSquatting = true;
        jumpCounter = jumpCounter - 1f;
        StartCoroutine(Squatting());
        

      
    }

    public void MovingHorizontallyInTheAir()
    {
        
        if (sameAsOnGround)
        {
            rb2DBase.velocity = new Vector2(horizontalMovement.movementSpeedVector.x * hAirSpeedFactor, rb2DBase.velocity.y); //old version of horizontal movement
        }

      

        else
        {
            rb2DBase.velocity = new Vector2(hAirSpeedFactor * horizontalAirSpeed * horizontalMovement.hVector, rb2DBase.velocity.y); //old version of horizontal movement
        }
        
    }

    public void powerJumping()
    {

    }

    #endregion

    #region Releasing Jump Method
    public void ReleasingJump()
    {
        
        if (rb2DBase.velocity.y >= 0f)
        {
            
            rb2DBase.velocity = new Vector2(rb2DBase.velocity.x, rb2DBase.velocity.y * 0.25f);
        }
        
    }
    #endregion

    public void Landing()
    {

    }

    public IEnumerator Squatting()
    {
        yield return new WaitForSeconds(squattingTime);

        

        rb2DBase.velocity = new Vector2(rb2DBase.velocity.x, verticalJumpSpeed);

        isJumping = true; 
        if (jumpReleased )
        {
            ReleasingJump(); 
        }
        isSquatting = false;
        //jumpReleased = false; 
    }


}

