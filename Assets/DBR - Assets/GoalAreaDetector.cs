using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GoalAreaDetector : MonoBehaviour
{
    public SpaceShootBallManager manager;

    public void OnTriggerEnter2D(Collider2D other)
    {
     

        if (other.gameObject == manager.shootBall)
        {
           
            if (gameObject == manager.redGoalArea)
            {
                Debug.Log("Gol del equipo azul!!!");
                manager.blueTeamScore++;
                StartCoroutine(manager.HandleGoal());
            }
            else if (gameObject == manager.blueGoalArea)
            {
                Debug.Log("Gol del equipo rojo!!!");
                manager.redTeamScore++;
                StartCoroutine(manager.HandleGoal());
            }
        }
    }

   
}
