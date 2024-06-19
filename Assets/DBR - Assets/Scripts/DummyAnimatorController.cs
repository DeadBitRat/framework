using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAnimatorController : MonoBehaviour
{
    public Animator anim; 
    public HealthSystem healthSystem;

    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();   
        healthSystem = GetComponent<HealthSystem>();
    }

    // Update is called once per frame
    void Update()
    {
      

        anim.SetBool("Damaged", healthSystem.isGettingHurt);
    }

 
}
