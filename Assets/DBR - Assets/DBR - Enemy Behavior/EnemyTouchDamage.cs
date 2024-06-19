using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{
    public float damageDuration = 1f; 

    public int damageByTouch; 

   
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if the collided object has the tag "Player"
            if (collision.gameObject.CompareTag("Player"))
            {
                // Get the parent object of the collided object
                Transform playerParent = collision.gameObject.transform.parent;

                // Check if the parent object exists
                if (playerParent != null)
                {
                    // Get the HealthSystem component from the parent object
                    HealthSystem healthSystem = playerParent.GetComponent<HealthSystem>();

                    // Check if the HealthSystem component exists on the parent object
                    if (healthSystem != null)
                    {
                        // Call the ReceiveDamage method
                        healthSystem.ReceivingDamage(damageByTouch, damageDuration);

                    }
                    else
                    {
                        Debug.LogWarning("No HealthSystem component found on the parent object of the Player.");
                    }
                }
                else
                {
                    Debug.LogWarning("The Player object does not have a parent.");
                }

                gameObject.GetComponent<Selfdestroy>().Destroy();
            }
        }
    }
