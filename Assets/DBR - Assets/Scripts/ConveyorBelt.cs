using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float conveyorSpeed;
    public GameObject characterStanding; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            

            characterStanding = collision.gameObject;

            if (collision.gameObject.GetComponent<PhysicsModifiers>() != null)
            {
                Debug.Log("Agregándole velocidad");
                PhysicsModifiers physics = collision.gameObject.GetComponent<PhysicsModifiers>();
                physics.HorizontalSpeed(conveyorSpeed);

            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {

            if (collision.gameObject.GetComponent<PhysicsModifiers>() != null)
            {
                PhysicsModifiers physics = collision.gameObject.GetComponent<PhysicsModifiers>();
                physics.HorizontalSpeed(0f);

            }
        }
    }
}
