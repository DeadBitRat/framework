using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStates : MonoBehaviour
{
    // CHARACTER STATES GATHERS ALL THE STATES FROM THE ACTION SCRIPTS, BUT DOESNT DEFINE OR CONTROLS THEM. 

    // Components


    private GameplaySystem gameplaySystem;
    public GroundDetector groundDetector;
    private HorizontalMovement horizontalMovement;
    private InputDetector inputDetector;
    private Jump jump;
    private ShootingAttack shootingAttack;

    [SerializeField]
    private DialogueActorManager dialogue; 


    // Actions
    public string currentAction;

    public bool isIdle;
    public bool isActionIdle;
    public bool isLongIdle; 
    public bool isWalking;
    public bool isRunning;

    public bool isJumping;
    public bool isSquatting; 
    public bool isRising; 
    public bool isFalling; 

    public bool isClimbingLadder;

    public bool isShooting;

    public bool isTalking; 

    // States

    public string orientation;
    public bool touchingGround; 
    public bool onGround;
    public bool inTheAir;

    public bool onLadder; 
    public bool inFrontOfLadder;
    public bool onTopOfLadder;
    public bool atBottomOfLadder; 


    // Physics

    public Vector2 velocity;
    public float verticalVelocity; 


 



    // Start is called before the first frame update
    void Start()
    {
        gameplaySystem = GetComponent<GameplaySystem>();
        horizontalMovement = GetComponent<HorizontalMovement>();
        jump = GetComponent<Jump>();
        inputDetector = GetComponent<InputDetector>();  
        shootingAttack = GetComponent<ShootingAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        isIdle = !isActionIdle && !isLongIdle && !isWalking && !isRunning &&
                !isJumping && !isSquatting && !isRising && !isFalling &&
                !isClimbingLadder && !isShooting && !isTalking;



        // Actions
        
        
        isWalking = horizontalMovement.isWalking; 
        isRunning = horizontalMovement.isRunning;
        isJumping = jump.isJumping;
        isSquatting = jump.isSquatting;
        isRising = jump.isRising; 
        isFalling = jump.isFalling;

        isShooting = shootingAttack.isShooting;


        // Acting!

        isTalking = dialogue.isTalking; 

        // States: 


        onGround = horizontalMovement.onGround;  // From: Ground Detector
        touchingGround = gameplaySystem.groundDetector.touchingGround;
        inTheAir = jump.inTheAir; // From: Jump 
        orientation = horizontalMovement.orientation; 
        
        
        inFrontOfLadder = gameplaySystem.groundDetector.detectingFrontOfLadder; // From: Ground Detector
        onTopOfLadder = gameplaySystem.groundDetector.detectingTopOfLadder; // From: Ground Detector
        atBottomOfLadder = gameplaySystem.groundDetector.detectingBottomOfLadder;  // From: Ground Detector



        // Physics 
        velocity = gameplaySystem.playerBase.GetComponent<Rigidbody2D>().velocity;
        verticalVelocity = velocity.y; 

    }
}
