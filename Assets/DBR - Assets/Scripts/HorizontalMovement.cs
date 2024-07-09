using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
    [Header("Gameplay")]

    public GameplaySystem gameplaySystem;


    [Header("Player's Component")]

    public GameObject character;
    private CharacterStates states;
    private InputDetector inputDetector;
    private CharacterAtributes characterAtributes;


    [SerializeField]
    private Jump jump;

    [SerializeField]
    public Pathfinding2 pathfinder;

    [SerializeField]
    private GameObject playerBase;

    [HideInInspector]
    public Rigidbody2D rb2DBase;

    [Header("Horizontal Movement Settings")]

    [Tooltip("Horizontal Speed Multiplier Factor")]
    public float walkingSpeed;
    public float runningSpeed;
    public bool canRun;
    public bool alwaysRunning;

    public float timeToTopSpeed;


    public float hSpeedFactor = 1f;


    [Header("Horizontal Movement States")]

    
    public bool onGround;
    public string orientation;

    [Header("Horizontal Movement Actions")]
    public bool isIdle;
    public bool isWalking;
    public bool isRunning;
    public bool changingSpeedSoft;
    public bool changingSpeedHard;

    public bool autoWalk; 

    public bool changingDirections;
    public bool changingDirectionHard;
    public float directionChangeDuration;

    public float movingCounter;
    public float negativeDirectionCounter;
    public float positiveDirectionCounter;

    public bool movingByInput;
    public bool movingByClick;

    [Header("Top Down Elements")]
    public GameObject baseShadow;


    [Header("Indicators")]
    public float hVector;
    public float vVector;

    

    public Vector2 movementSpeedVector;





    [Tooltip("Time in seconds that it actually takes to start walking after pressing Horizontal Input")]
    public float walkingLateness;
    private float walkingLatenessCounter;

    [Header("Modifiers")]

    public float hSpeedModifier;


    public void Start()
    {
        inputDetector = GetComponent<InputDetector>();
        jump = GetComponent<Jump>();
        gameplaySystem = GetComponent<GameplaySystem>();
        characterAtributes = GetComponent<CharacterAtributes>();
        rb2DBase = playerBase.GetComponent<Rigidbody2D>();
        states = GetComponent<CharacterStates>();


        orientation = "Right";

        if (gameplaySystem.gameplay == GameplaySystem.GameplayType.Platformer)
        {
           
        }

        else if (gameplaySystem.gameplay == GameplaySystem.GameplayType.TopDown)
        {
           
        }


    }

    public void Update()
    {
        hVector = inputDetector.hVector;
        vVector = inputDetector.vVector;



        #region RigidBody2D Controller
        
       if (gameplaySystem.gameplay == GameplaySystem.GameplayType.Platformer)
       {
           
           rb2DBase.velocity = new Vector2(movementSpeedVector.x * hSpeedFactor + hSpeedModifier, rb2DBase.velocity.y);
       }

       if (gameplaySystem.gameplay == GameplaySystem.GameplayType.TopDown)
       {
           rb2DBase.velocity = movementSpeedVector;
           rb2DBase.velocity = new Vector2(movementSpeedVector.x * hSpeedFactor, movementSpeedVector.y); 
       }
       

        #endregion

        #region 1- Platformer Controls
        if (gameplaySystem.gameplay == GameplaySystem.GameplayType.Platformer)
        {
            MovingHorizontally(walkingSpeed);

            #region 1-1 Platformer Actions

            #region 1.1- Idle
            if (states.onGround && movementSpeedVector.x == 0 && rb2DBase.velocity.y == 0)
            {

                states.currentAction = "Idle";
                isIdle = true;
            }

            else
            {
                isIdle = false;
            }
            #endregion



            #region 1.2 - Walking

            if (states.onGround && movementSpeedVector.x != 0 && inputDetector.pressingHorizontalAxis && (!inputDetector.pressingRun && !alwaysRunning)) // && Mathf.Abs(rb2DBase.velocity.x) == walkingSpeed)
            {
                isWalking = true;
                states.currentAction = "Walking";
            }

            else
            {
                isWalking = false;
            }

            #endregion



            #region 1.3 - Running

            if (canRun)
            {
                if (states.onGround && movementSpeedVector.x != 0 && inputDetector.pressingHorizontalAxis && (inputDetector.pressingRun || alwaysRunning))
                {
                    isRunning = true;
                    states.currentAction = "Running";
                }

                else
                {
                    isRunning = false;
                }
            }

            #endregion



            #endregion

            #region 1.2 - Platformer States


            #region 1.2.1 - OnGround
            if (states.touchingGround && !states.onLadder)
            {
                onGround = true;
            }

            else
            {
                onGround = false;
            }
            #endregion

            #region 2.2 - Changing Direction

            if (hVector < 0 && (isWalking || isRunning))
            {
                negativeDirectionCounter += Time.deltaTime;
            }

            else
            {
                negativeDirectionCounter = 0f;
            }

            if (hVector > 0 && (isWalking || isRunning))
            {
                positiveDirectionCounter += Time.deltaTime;
            }

            else
            {
                positiveDirectionCounter = 0f;
            }

            if (hVector != 0 && (isWalking || isRunning))
            {
                movingCounter += Time.deltaTime;
            }

            else { movingCounter = 0f; }

            #endregion




            #endregion

            #region 1-3 Modifiers

            if (hVector != 0)
            {
                walkingLatenessCounter -= Time.deltaTime;

            }

            else
            {
                walkingLatenessCounter = walkingLateness;
            }





            #region Horizontal Movement Modifiers

            //if (onGround) { 

            //}


            #endregion

            #endregion

        }
        #endregion // Final de Actions Platformer 

        #region 2- TopDown Controls



        if (gameplaySystem.gameplay == GameplaySystem.GameplayType.TopDown)
        {

            MovingHorizontallyTopDown(walkingSpeed); 
            #region 2.1 Top Down Actions







            #region 2.1.3 - Top Down Running

            if (canRun)
            {
                if (states.onGround && movementSpeedVector != new Vector2(0f, 0f) && inputDetector.pressingHorizontalAxis && (inputDetector.pressingRun || alwaysRunning))
                {
                    isRunning = true;
                    states.currentAction = "Running";
                }

                else
                {
                    isRunning = false;
                }


            }
            #endregion


            #endregion

            #region 2.2 Top Down States

            #region 2.2.1 - Top Down Idle Idle
            if (states.currentAction == "Idle")
            {

                
                isIdle = true;
            }

            else

            {
                isIdle = false;

            }

            #endregion


            #region 2.2.1 On Ground

            if (character.transform.position.y == baseShadow.transform.position.y && !states.onLadder)
            {

                onGround = true;
            }

            else
            {
                onGround = false;
            }

            #endregion


            #region 2.1.2 - Top Down Walking


            if (movementSpeedVector != new Vector2(0f, 0f) && (inputDetector.pressingHorizontalAxis || inputDetector.pressingVerticalAxis) && ((!inputDetector.pressingRun && !alwaysRunning) || !canRun) && !movingByClick)
            {
                
                isWalking = true;
                states.currentAction = "Walking";
                movingByInput = true;
            }

            else if (pathfinder != null && pathfinder.isWalking)
            {
                
                states.currentAction = "Walking";
                isWalking = pathfinder.isWalking;
                movingByClick = true;
            }

            else
            {
                
                movingByInput = false;
                movingByClick = false;
                isWalking = false; 
            }




            /*
            if (states.onGround && movementSpeedVector != new Vector2(0f, 0f) && (inputDetector.pressingHorizontalAxis || inputDetector.pressingVerticalAxis) && ((!inputDetector.pressingRun && !alwaysRunning) || !canRun) && !movingByClick) // && Mathf.Abs(rb2DBase.velocity.x) == walkingSpeed)
            {
                Debug.Log("esta es la wea que está weviando");
                isWalking = true;
                states.currentAction = "Walking";
                movingByInput = true;


            }

            if (pathfinder != null)
            {
                if (pathfinder.isWalking)
                {


                    Debug.Log("Caminando desde el pathfinder!!!");
                    states.currentAction = "Walking";
                    isWalking = pathfinder.isWalking;
                    movingByClick = true;
                }

                else
                {
                    movingByClick = false;
                    isWalking = false;
                }


            }

            else
            {
                if (!movingByClick && !movingByInput) 
                { 
                Debug.Log("YO digo que no está caminando"); 
                movingByInput = false;
                movingByClick = false;
                isWalking = false;
                }


            }
            */



                #endregion



                #endregion



        }

        #endregion
    }

    #region 1- Platformer Methods

    public void MovingHorizontally(float speed)
    {
        if (walkingLatenessCounter <= 0 && hVector != 0)
        {
            Accelerating(speed);

            movementSpeedVector = new Vector2(hVector * speed * hSpeedFactor, rb2DBase.velocity.y);
        }

        else

        {
            movementSpeedVector = new Vector2(0f, rb2DBase.velocity.y);
            //speedFactor = 0.1f;
        }

    }


    public void MovingHorizontallyTopDown(float speed)
    {
        if (walkingLatenessCounter <= 0 && (hVector != 0 || vVector != 0))
        {
            Accelerating(speed);
            Vector2 combinedVector = new Vector2(hVector, 0f) + new Vector2(0f, vVector);

            movementSpeedVector = combinedVector.normalized * speed * hSpeedFactor;

            //movementSpeedVector = new Vector2(hVector * speed * speedFactor, rb2DBase.velocity.y);
        }

        else

        {
            movementSpeedVector = new Vector2(0f, 0f);
            //speedFactor = 0.1f;
        }

    }


    public void Accelerating(float speed)
    {


        if (hSpeedFactor < 1f && (positiveDirectionCounter >= 0.1f || negativeDirectionCounter >= 0.1f) && timeToTopSpeed != 0f)
        {
            hSpeedFactor = movingCounter / timeToTopSpeed;
        }

        else if (hSpeedFactor < 1f && (positiveDirectionCounter < 0.1f || negativeDirectionCounter < 0.1f) && timeToTopSpeed != 0f)
        {
            hSpeedFactor = 0.1f;
        }

        else if ((positiveDirectionCounter > timeToTopSpeed || negativeDirectionCounter > timeToTopSpeed))
        {
            hSpeedFactor = 1f;
        }

    }

    public void OrientingHorizontally(float direction)
    {
        if (direction > 0f)
        {
            orientation = "Right";
        }

        else if (direction < 0f)
        {
            orientation = "Left";
        }

        


    }

    #endregion //Platformer Methods

    public IEnumerator AutoPositioning(Vector2 targetPosition, float speed)
    {
        autoWalk = true; 
        // Calculate the distance to the target
        float distance = Vector2.Distance(transform.position, targetPosition);

        // While the character is not at the target position
        while (distance > 0.1f) // You can adjust the threshold as needed
        {
            // Calculate the direction towards the target
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

            // Calculate the new position
            Vector2 newPosition = (Vector2)transform.position + direction * speed * Time.deltaTime;

            // Move the character
            rb2DBase.MovePosition(newPosition);

            // Update the distance
            distance = Vector2.Distance(transform.position, targetPosition);

            // Wait for the next frame
            yield return null;
        }

        // Ensure the character is exactly at the target position
        rb2DBase.MovePosition(targetPosition);

        autoWalk = false;

       

    }

    public void TestWalk(Transform goal)
    {
        Vector2 targetPosition = goal.position;
        StartCoroutine(AutoPositioning(targetPosition, walkingSpeed));  
    }
}

