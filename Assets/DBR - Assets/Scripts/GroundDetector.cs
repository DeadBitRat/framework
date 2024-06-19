using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public bool touchingGround;

    public bool detectingFrontOfLadder;
    public bool detectingTopOfLadder;
    public bool detectingBottomOfLadder;

    //[HideInInspector]
    public GameObject detectedLadder; 

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            touchingGround = true;
        }

        if (collision.tag == "Ladder")
        {
            detectingFrontOfLadder = true;
            detectedLadder = collision.gameObject; 

           
        }

        if (collision.name == "LadderTop")
        {

            detectingTopOfLadder = true;

        }

        if (collision.name == "LadderBottom")
        {
            detectingBottomOfLadder = true;
        }


    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            touchingGround = true;
        }

    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            touchingGround = false; 
        }

        if (collision.tag == "Ladder" )
        {
            detectingFrontOfLadder = false;
            detectedLadder = null;


 

           


        }


        if (collision.name == "LadderBottom")
        {
            detectingBottomOfLadder = false;
        }

        if (collision.name == "LadderTop")
        {
            detectingTopOfLadder = false;
        }

    }



}
