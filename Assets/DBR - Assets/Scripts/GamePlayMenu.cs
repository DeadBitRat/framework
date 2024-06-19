using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayMenu : MonoBehaviour
{
    public Canvas gameplayMenu;
    private GameplayPause gameplayPause; 
    // Start is called before the first frame update
    void Start()
    {
        gameplayMenu.enabled = false; 
        gameplayPause = GetComponent<GameplayPause>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameplayMenu.enabled)
            {
                gameplayPause.PausingGame();
            }

            else
            {
                gameplayPause.UnpausingGame();
            }
            gameplayMenu.enabled = !gameplayMenu.enabled;
           
         
        }
    }


}
