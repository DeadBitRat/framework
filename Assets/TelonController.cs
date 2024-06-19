using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.Arm;

public class TelonController : MonoBehaviour
{
    public AnimationClip fadeIn;
    public AnimationClip fadeOut; 

    private Animator animator; 

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn(int seconds)
    {
        animator.Play(fadeIn.name); 

    }

    public void FadeOut(int seconds)
    {
        animator.Play(fadeOut.name); 
    }



    
}
