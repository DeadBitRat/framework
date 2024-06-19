using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieVictimDetector : MonoBehaviour
{
    public float radius = 5f; // The radius within which to search for players
    public LayerMask layerMask; // Layer mask to exclude walls and platforms
    public Transform baseTransform;

    public GameObject detectedVictim; 
    public Collider2D[] hits;
    public float maxChaseDistance;

    public ZombieWalkBehavior zWB; 
    

    void Start()
    {
        
    }

    void Update()
    {
        // Find the child GameObject named "base"
        //baseTransform = transform.Find("base");
        if (baseTransform == null)
        {
            Debug.LogError("No child GameObject named 'base' found.");
            return;
        }

        // Find all colliders within the radius
        hits = Physics2D.OverlapCircleAll(baseTransform.position, radius);
        Debug.Log("Hits: " + hits);



        
        foreach (Collider2D hit in hits)
        {
            
            if (hit.CompareTag("Player"))
            {
               
                Vector2 direction = (hit.transform.position - baseTransform.position).normalized;
               
                RaycastHit2D raycastHit = Physics2D.Raycast(baseTransform.position, direction, radius, layerMask);
                

                if (raycastHit.collider != null && raycastHit.collider.CompareTag("Player"))
                {
                    detectedVictim = hit.gameObject;
                    
                }
            }
        }

        if (detectedVictim != null)
        {
            float distance = Vector3.Distance(baseTransform.position, detectedVictim.transform.position);
            if (distance > maxChaseDistance)
            {
                detectedVictim = null;
                
            }


        }



    }

    void OnDrawGizmosSelected()
    {
        if (baseTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(baseTransform.position, radius);
        }

        if (detectedVictim != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(baseTransform.position, detectedVictim.transform.position);
        }
    }

    void ChaseVictim()
    {

    }

    
}
