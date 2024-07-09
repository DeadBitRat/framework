using System.Collections;
using UnityEngine;
using TMPro; 

public class SpaceShootBallManager : MonoBehaviour
{
    public GameObject shootBall; // Reference to the ball object
    public Transform ballSpawnPoint; // Reference to the ball spawn point in the center of the field
    public Transform[] redTeamSpawnPoints; // Array of spawn points for the red team
    public Transform[] blueTeamSpawnPoints; // Array of spawn points for the blue team
    public GameObject redGoalArea; // Reference to the red team's goal area
    public GameObject blueGoalArea; // Reference to the blue team's goal area
    public int redTeamScore = 0; // Red team's score
    public int blueTeamScore = 0; // Blue team's score
    public float resetDelay = 2f; // Time to wait before resetting the ball after a goal

    [Header("UI Elements")]
    public string player1Name; 
    public string player2Name;


    public TextMeshProUGUI player1NamePanel; 
    public TextMeshProUGUI player2NamePanel;
    public TextMeshProUGUI redScorePanel;
    public TextMeshProUGUI blueScorePanel; 



    private void Start()
    {
        ResetMatch();

        // UI
        player1NamePanel.text = player1Name;
        player2NamePanel.text = player2Name;
    }

    private void ResetMatch()
    {
        // UI
        redScorePanel.text = redTeamScore.ToString();
        blueScorePanel.text = blueTeamScore.ToString();


        // Reset the ball position
        shootBall.transform.position = ballSpawnPoint.position;
        shootBall.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        shootBall.GetComponent<Rigidbody2D>().angularVelocity = 0f;

        // Reset player positions
        ResetTeamPositions(redTeamSpawnPoints);
        ResetTeamPositions(blueTeamSpawnPoints);
    }

    private void ResetTeamPositions(Transform[] teamSpawnPoints)
    {
        foreach (var spawnPoint in teamSpawnPoints)
        {
            var player = spawnPoint.GetComponentInChildren<Rigidbody2D>();
            if (player != null)
            {
                player.transform.position = spawnPoint.position;
                player.transform.rotation = spawnPoint.rotation;
                player.velocity = Vector3.zero;
                player.angularVelocity = 0f;
            }
        }
    }

 

    public IEnumerator HandleGoal()
    {
        // Play goal celebration animation (implementation depends on your setup)
        PlayGoalCelebrationAnimation();

        // Wait for resetDelay seconds
        yield return new WaitForSeconds(resetDelay);

        // Reset the match
        ResetMatch();
    }

    private void PlayGoalCelebrationAnimation()
    {
        // Implement your goal celebration animation here
    }
}
