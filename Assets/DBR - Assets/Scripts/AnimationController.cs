
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[ExecuteInEditMode]
public class AnimationController : MonoBehaviour
{   public Animator animator;
    public RuntimeAnimatorController runTimeAnimatorController;

    public CharacterStates states;
    public string[] stateAvailables;

    public InputDetector inputDetector;
    
    public Transform tf;

    public bool animationPaused;

    public int ladderAnimationCounter;

    public SpriteRenderer shooterArmSpriteRenderer;

    public Sprite spritesForArmMask;
    public Sprite spritesForTalkingHead; 
    public SpriteMask spriteMask;

    [Header("Animations")]
    public AnimationClip idleAnimation;
    public AnimationClip walkingAnimation;
    public AnimationClip talkingAnimation; 
    


    // Start is called before the first frame update
    void Start()
    {
        SetAnimatorController(); 
       
    }

    // Update is called once per frame
    void Update()
    {
        #region Orientation

        if (states.orientation == "Left")
        {
            tf.localScale = new Vector2(-1f, 1f);
        }

        if (states.orientation == "Right")
        {
            tf.localScale = new Vector2(1f, 1f);
        }

        #endregion

        #region Movement Animations


        animator.SetBool("Idle", states.isIdle);

        if (states.isIdle)
        {
            if (idleAnimation != null)
            {
                animator.Play(idleAnimation.name);
            }
        }

        animator.SetBool("Walking", states.isWalking);

        if (states.isWalking)
        {
            if (walkingAnimation != null)
            {
                animator.Play(walkingAnimation.name); 
                    }
        }

        animator.SetBool("Running", states.isRunning);



        animator.SetBool("Jumping", states.isJumping);


        
        if (states.currentAction == "JumpPeak")
        {
           //animator.SetTrigger("JumpPeak");
        }

        animator.SetBool("Squatting", states.isSquatting); 

        animator.SetBool("Rising", states.isRising);

        animator.SetBool("Falling", states.isFalling);

        animator.SetBool("ClimbingLadder", states.isClimbingLadder);



        #endregion

        #region Attack Animations


        animator.SetBool("Shooting", states.isShooting);

        #region Shooting Animation Handling

        spriteMask.sprite = spritesForArmMask;

        if (Application.isPlaying)
        {


            if (states.isShooting)
            {
                spriteMask.enabled = true;
                shooterArmSpriteRenderer.enabled = true;
                
            }

            else
            {
                spriteMask.enabled = false;
                shooterArmSpriteRenderer.enabled = false;
                
            }
        }

        #endregion



        #endregion


        #region States

        animator.SetBool("OnGround", states.onGround);

        animator.SetBool("OnLadder", states.onLadder);

        if (states.onLadder)
        {


            if (states.isClimbingLadder)
            {
                UnpausingLadderAnimation();
            }

            else
            {
                if (ladderAnimationCounter == 1)
                {
                    PausingLadderAnimation();
                }
            }

        }
        else
        {
            if (ladderAnimationCounter == 0)
            {
                UnpausingLadderAnimation();
            }
        }



        #endregion

        #region Acting!

        animator.SetBool("Talking", states.isTalking); 

        
        if (states.isTalking && !states.isActing)
        {
            animator.Play(talkingAnimation.name); 
        }
        
        


        #endregion

        #region Physics

        animator.SetFloat("VerticalVelocity", states.verticalVelocity);


        #endregion





    }

    #region Input Voids

    public void SwitchInputOffFromAnimator()
    {
        inputDetector.SwitchInputOff();
    }

    public void SwitchInputOnFromAnimator()
    {
        inputDetector.SwitchInputOn();
    }

    public void SwitchHorizontalInputOffFromAnimator()
    {
        inputDetector.SwitchHorizontalInputOff();
    }

    public void SwitchHorizontalInputOnFromAnimator()
    {
        inputDetector.SwitchHorizontalInputOn();
    }

    #endregion

    #region Ladder Animation Voids

    public void PausingLadderAnimation()
    {
        
        animator.speed = 0;
        ladderAnimationCounter = 0;
    }

    public void UnpausingLadderAnimation()
    {
        ladderAnimationCounter = 1;
        animator.speed = 1; 
    }

    #endregion

    public void SetAnimatorController()
    {
        if (runTimeAnimatorController != null) { 
        animator.runtimeAnimatorController = runTimeAnimatorController;
        }
    }


    bool AnimatorHasParameter(Animator animator, string parameterName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == parameterName)
            {
                return true;
            }
        }
        return false;
    }
}
