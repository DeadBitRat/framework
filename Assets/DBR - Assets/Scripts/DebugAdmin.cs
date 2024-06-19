using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class DebugAdmin : MonoBehaviour
{
    public bool debugAdminActive;

    public Canvas debugCanvas;


    public TMP_Text speedText; 

    private int speedIndex = 0;
    private float[] speeds = { 1f, 2f, 5f, 0.1f, 0.25f, 0.5f, 0.75f };
    private float currentSpeed = 1f;

    private GameplayPause gameplayPause; 


    // Start is called before the first frame update
    void Start()
    {
        debugCanvas.enabled = false;
        gameplayPause = GetComponent<GameplayPause>();
    }

    // Update is called once per frame
    void Update()
    {
        if (debugAdminActive)
        {
            if (Input.GetKeyDown(KeyCode.F9) )
            {
                debugCanvas.enabled = !debugCanvas.enabled;
            }

            if (Input.GetKeyDown(KeyCode.F8) && !gameplayPause.gameIsPaused)
            {
                // Incrementa el índice de velocidad circularmente
                speedIndex = (speedIndex + 1) % speeds.Length;
                currentSpeed = speeds[speedIndex];

                // Actualiza la velocidad del juego
                Time.timeScale = currentSpeed;
                Debug.Log("Gameplay Speed: " + currentSpeed);
                speedText.text = currentSpeed.ToString() + "X";


            }

            
        }
    }

    public void UpdateSpeed(float newSpeed)
    {
        currentSpeed = newSpeed;
    }

}
