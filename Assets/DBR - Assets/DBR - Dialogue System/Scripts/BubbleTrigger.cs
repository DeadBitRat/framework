using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTrigger : MonoBehaviour
{
    public SignManager signManager;
    

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dialogue"))
        {
            
            
        }

        if (collision.CompareTag("Player"))
        {
           
            

        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        

        if (collision.tag == "DialogueTrigger")
        {
            
            
        }
    }


}
