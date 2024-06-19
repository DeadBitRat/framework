using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayPause : MonoBehaviour
{
    public bool gameIsPaused; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PausingGame()
    {
        Time.timeScale = 0;
        gameIsPaused = true; 
    }

    public void UnpausingGame()
    {
        Time.timeScale = 1;
        gameIsPaused = false;
    }
}
