using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDetector : MonoBehaviour
{
    [Header("Gameobject Components")]
    private CharacterStates states;



    [Header("Movement Components")]
    private float _hVector;
    private float _vVector;

    public float hVector;
    public float vVector;

    public GameplaySystem gameplaySystem;

    [HideInInspector]
    public Rigidbody2D rb;

    public GameObject playerBase;

   
    public HorizontalMovement horizontalMovement;

   
    public LadderMovement ladderMovement;
    
    
    private Jump jump;

    [Header("Attack Components")]

    public ShootingAttack shootingAttack;

    [Header("Machines Operating")]

    public MachinesController machineController = null; 


    [Header("Components States")]

    public bool inputActivated;
    public bool horizontalInputActivated; 
    public bool jumpInputActivated;

    [Header("Input States")]

    public bool pressingHorizontalAxis;
    public bool pressingVerticalAxis; 
    public bool pressingJump;
    public bool pressingRun;

    public bool isFirePressed;
    public bool wasFirePressed;
    public bool wasFireReleased;
    public bool currentFireAxisValue;
    public bool wasFirePressedLastFrame;
    
    public bool functionPressed;
    public bool doubleClickToFunctionActive; 
    private DoubleClickToFunction doubleClick2F; 



    // Start is called before the first frame update
    void Start()
    {
        gameplaySystem = GetComponent<GameplaySystem>();
        horizontalMovement = GetComponent<HorizontalMovement>();
        ladderMovement = GetComponent<LadderMovement>();

        jump = GetComponent<Jump>();
        states = GetComponent<CharacterStates>();
        rb = playerBase.GetComponent<Rigidbody2D>();

        if (doubleClickToFunctionActive) { 
       
            doubleClick2F = FindObjectOfType<DoubleClickToFunction>();
      
        }

    }

    // Update is called once per frame
    void Update()
    {
        hVector = _hVector;
        vVector = _vVector;

        #region General Input

        if (Input.GetButtonDown("Function")  )
        {
            
            functionPressed = true; 
        }

        else if (doubleClick2F != null)
        {
            functionPressed = doubleClick2F.functionClicked; 
        }

        else
        {
            functionPressed = false; 
        }

        #endregion

            #region 1- Platformer

        if (gameplaySystem.gameplay == GameplaySystem.GameplayType.Platformer && inputActivated)
        {
            #region Movements

            #region 1.1- Horizontal Movement Input

            if (horizontalInputActivated)
            {

                #region 1.1.1- Horizontal Vector
                _hVector = Input.GetAxisRaw("Horizontal");
                _vVector = Input.GetAxisRaw("Vertical");

                #endregion

                #region 1.1.1.1- Orientation Input

                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    float direction = Input.GetAxisRaw("Horizontal");
                    horizontalMovement.OrientingHorizontally(direction); 
                }
                #endregion

                #region 1.1.1.2- Walking Input
                if ((Input.GetAxis("Running") == 0 || !horizontalMovement.canRun) && horizontalMovement.alwaysRunning == false)
                {
                    horizontalMovement.MovingHorizontally(horizontalMovement.walkingSpeed);
                }
                #endregion

                #region 1.1.1.3- Running Input

                if ((Input.GetAxis("Running") != 0  || horizontalMovement.alwaysRunning) && horizontalMovement.canRun)

                {
                    horizontalMovement.MovingHorizontally(horizontalMovement.runningSpeed);
                }





                #endregion

            }
            #endregion

            #region 1.2- Jump Input

            //||| JUMP CONTROLS |||

            if (jumpInputActivated)
            {
                
            
            if (Input.GetButtonDown("Jump"))
            {
                jump.jumpBufferCount = jump.jumpBufferLenght;
            }

            else
            {
                jump.jumpBufferCount -= Time.deltaTime;
            }


            if (jump.jumpBufferCount >= 0 && jump.coyoteCounter > 0 && jump.jumpCounter >= 1)
            {
                
                jump.Jumping();
            }

            if (!Input.GetButton("Jump") && states.verticalVelocity > 0 && !states.onLadder)//&& !states.onGround && states.velocity.y > 0)
            {
                jump.jumpReleased = true; 

            }

            else
            {
                jump.jumpReleased = false;
            }



                // Jumping Down Through One Way Platforms




                // ||| END OF JUMP CONTROLS |||

            }

            #endregion

            #region 1.3- Ladder Input

            #region 1.3.1- Getting On Ladder Input


            // "Se presiona hacia arriba o hacia abajo cuando no se está ni al fondo, en la cima de la escalera, ni saltando; Para subirse a la escalera en el aire. 
            if (Input.GetAxisRaw("Vertical") != 0 && !states.atBottomOfLadder && !states.onTopOfLadder && !states.onGround && states.inFrontOfLadder)
            {

                ladderMovement.GettingOnLadder();
            }

            // Para subirse a escalera desde arriba: "Se presiona hacia abajo cuando está arriba"

            if (Input.GetAxisRaw("Vertical") < 0 && states.onTopOfLadder && states.inFrontOfLadder)
            {

                ladderMovement.GettingOnLadder();
            }

            // Para subirse a escalera desde abajo. "Se presiona hacia arriba cuando está abajo"

            if (Input.GetAxisRaw("Vertical") > 0 && states.atBottomOfLadder && states.inFrontOfLadder)
            {

                ladderMovement.GettingOnLadder();
            }


            #endregion

            #region 1.3.2- Getting Off Ladder

            if (states.onLadder)
            {
                //

                if (Input.GetKey(KeyCode.Space))
                {
                    ladderMovement.GettingOffLadder();
                }
                // Para Bajarse de la escalera por abajo
                if (states.atBottomOfLadder && Input.GetAxisRaw("Vertical") < 0)
                {
                    ladderMovement.GettingOffLadder();
                }

                // Para bajarse de la escalera por arriba

                if (states.onTopOfLadder && Input.GetAxisRaw("Vertical") > 0)
                {
                    ladderMovement.GettingOffByTheHatch();
                    ladderMovement.GettingOffLadder();
                }


            }



            #endregion

            #endregion

            #endregion

            #region Attacks!

            if(Input.GetAxisRaw("Fire1") == 1)
            {
                
                
            }

            else
            {
                
            }

            currentFireAxisValue = Input.GetAxisRaw("Fire1") != 0;


            // Check if the axis key is pressed during this frame
            if (currentFireAxisValue && !wasFirePressedLastFrame)
            {
                
                shootingAttack.Shooting(states.orientation);
                // You can add your custom logic here
            }

            // Check if the axis key is released during this frame
            if (!currentFireAxisValue && wasFirePressedLastFrame)
            {
                
                // You can add your custom logic here
            }

            // Check if the axis key is being held down
            if (currentFireAxisValue)
            {
                
                // You can add your custom logic here
            }

            // Update the state for the next frame
            wasFirePressedLastFrame = currentFireAxisValue;




            #endregion


        }

        #endregion

        #region 2- TopDown

        if (gameplaySystem.gameplay == GameplaySystem.GameplayType.TopDown && inputActivated)
        {
            #region 2.1.1- Horizontal Vector
            hVector = Input.GetAxisRaw("Horizontal");
            vVector = Input.GetAxisRaw("Vertical");

            #endregion

            
            #region 2.1.1.1- Orientation Input

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                float direction = Input.GetAxisRaw("Horizontal");
                horizontalMovement.OrientingHorizontally(direction);
            }
            #endregion
            


            #region 2.1.1.2- Walking Input
            if ((Input.GetAxis("Running") == 0 || !horizontalMovement.canRun) && horizontalMovement.alwaysRunning == false)
            {
                horizontalMovement.MovingHorizontallyTopDown(horizontalMovement.walkingSpeed);
            }
            #endregion

            #region 1.1.1.3- Running Input

            if ((Input.GetAxis("Running") != 0 || horizontalMovement.alwaysRunning) && horizontalMovement.canRun)

            {
                horizontalMovement.MovingHorizontallyTopDown(horizontalMovement.runningSpeed);
            }

            #endregion
        }

        #endregion 



        #region Machine Operating


        if (machineController.machineActivatorPanel != null && machineController.machineActivatorPanel.machineActivator != null)
        {
            if (Input.GetButtonDown("Function") && states.isIdle && !machineController.machineActivatorPanel.machineActivator.activated)
            {
                // Do something when the F key is pressed

                machineController.machineActivatorPanel.machineActivator.activated = true;
                inputActivated = false;

                Debug.Log("Máquina encendida"); 
            }

            else if (Input.GetButtonDown("Function") && states.isIdle && machineController.machineActivatorPanel.machineActivator.activated)
            {
                machineController.machineActivatorPanel.machineActivator.activated = false;
                inputActivated = true;
                Debug.Log("Máquina apagada");
            }
        }


        #endregion

        #region Input States

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            pressingHorizontalAxis = true; 
        }

        else
        {
            pressingHorizontalAxis = false; 
        }


        if (Input.GetAxisRaw("Vertical") != 0)
        {
            pressingVerticalAxis = true;
        }

        else
        {
            pressingVerticalAxis = false;
        }

        if (Input.GetAxisRaw("Running") != 0)
        {
            pressingRun = true; 
        }

        else

        {
            pressingRun = false; 
        }

        if (Input.GetButton("Jump"))
        {
            pressingJump = true; 
        }

        else
        {
            pressingJump = false;   
        }


        #endregion

    }

    public void SwitchInput()
    {
        inputActivated = !inputActivated; 
    }

    public void SwitchInputOn()
    {
        inputActivated = true;
    }

    public void SwitchInputOff()
    {
        inputActivated = false;
    }


    public void SwitchHorizontalInputOn()
    {
        horizontalInputActivated = true;
    }

    public void SwitchHorizontalInputOff()
    {
        horizontalInputActivated = false;
    }

}

