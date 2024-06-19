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

    private InputDetector inputDetector;
    public GameObject character;
    private Jump jump;
    [SerializeField]
    public Pathfinding2 pathfinder;

    [SerializeField]
    private GameObject playerBase;

    [HideInInspector]
    public Rigidbody2D rb2DBase;


    [Header("Character Atributes")]

    private CharacterAtributes characterAtributes;

    [Header("Horizontal Movement States")]

    private CharacterStates states;
    public bool onGround;
    public string orientation;

    [Header("Horizontal Movement Actions")]
    public bool isIdle;
    public bool isWalking;
    public bool isRunning;
    public bool changingSpeedSoft;
    public bool changingSpeedHard;

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


    [Tooltip("Horizontal Vector, Goes gradually from 0 to 1 or from 0 to -1")]
    public float hVector;
    public float vVector;

    [Tooltip("Horizontal Speed Multiplier Factor")]
    public float walkingSpeed;
    public float runningSpeed;

    public Vector2 movementSpeedVector;

    public float timeToTopSpeed;


    public float speedFactor;

    public bool canRun;
    public bool alwaysRunning;


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
    }

    public void Update()
    {
        hVector = inputDetector.hVector;
        vVector = inputDetector.vVector;

        

        #region RigidBody2D Controller
        if (gameplaySystem.gameplay == GameplaySystem.GameplayType.Platformer)
        {
            Debug.Log("EStamos en modo platformer! "); 
            rb2DBase.velocity = new Vector2(movementSpeedVector.x + hSpeedModifier, rb2DBase.velocity.y);
        }

        if (gameplaySystem.gameplay == GameplaySystem.GameplayType.TopDown)
        {
            rb2DBase.velocity = movementSpeedVector;
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

            movementSpeedVector = new Vector2(hVector * speed * speedFactor, rb2DBase.velocity.y);
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

            movementSpeedVector = combinedVector.normalized * speed * speedFactor;

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


        if (speedFactor < 1f && (positiveDirectionCounter >= 0.1f || negativeDirectionCounter >= 0.1f) && timeToTopSpeed != 0f)
        {
            speedFactor = movingCounter / timeToTopSpeed;
        }

        else if (speedFactor < 1f && (positiveDirectionCounter < 0.1f || negativeDirectionCounter < 0.1f) && timeToTopSpeed != 0f)
        {
            speedFactor = 0.1f;
        }

        else if ((positiveDirectionCounter > timeToTopSpeed || negativeDirectionCounter > timeToTopSpeed))
        {
            speedFactor = 1f;
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

}

