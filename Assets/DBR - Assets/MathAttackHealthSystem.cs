using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathAttackHealthSystem : MonoBehaviour
{
    public int health = 5;

    public bool healthBarShown; 
    
    public GameObject[] healthBoxes;

    public CannonController cannonController; 



    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject healthBox in healthBoxes)
        {
            healthBox.SetActive(false);
            

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 1)
        {
            GameOver();
            cannonController.cannonActivated = false; 
        }
    }

    public void recieveDamage(int damage)
    {
        Debug.Log("Just recieved damage!");
        health = health - damage; 
    }


    public void GameOver()
    {
        Debug.Log("Gameover"); 
    }

    public IEnumerator ShowingHealthBar()
    {
        healthBarShown = true;

        foreach (GameObject healthBox in healthBoxes)
        {
            healthBox.SetActive(true);
            yield return new WaitForSeconds(0.5f);

        }

    }

    public IEnumerator HidingHealthBar()
    {

        for (int i = healthBoxes.Length - 1; i >= 0; i--)
        {
            // Get the current GameObject from the array
            GameObject healthBox = healthBoxes[i];
            healthBox.SetActive(false);
            yield return new WaitForSeconds(0.5f);


        }

        healthBarShown = false;
    }

}
