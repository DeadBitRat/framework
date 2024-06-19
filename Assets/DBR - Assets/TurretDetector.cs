using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDetector : MonoBehaviour
{
    public bool detectingPlayer;
    
    
  

    public void OnTriggerStay2D(Collider2D collision)
    {
        // Verifica si el objeto que entró en contacto tiene el tag "Player".
        if (collision.gameObject.tag == "Player")
        {
            detectingPlayer = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        // Verifica si el objeto que entró en contacto tiene el tag "Player".
        if (collision.gameObject.tag == "Player")
        {
            detectingPlayer = false;
        }
    }




}
