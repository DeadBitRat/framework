using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundhogHangoverManager : MonoBehaviour
{
    

    public InputDetector input;
    public Pathfinding2 pathfinding;

    public int numberOfDays; 

    public SaveLoadManager saveLoadManager;
    public GameManager gameManager; 

    // Start is called before the first frame update
    void Start()
    {
        // Obtener la referencia del componente SaveLoadManager
        saveLoadManager = GetComponent<SaveLoadManager>();

        // Cargar el valor de numberOfDays al iniciar la escena, con valor predeterminado de 1
        numberOfDays = saveLoadManager.LoadInt("NumberOfDays", 1);
    }

    // Update is called once per frame
    void Update()
    {
        // Incrementar y guardar el valor de numberOfDays al presionar la tecla R
        if (Input.GetKeyDown(KeyCode.R))
        {
            numberOfDays += 1;
            saveLoadManager.SaveInt("NumberOfDays", numberOfDays);

            // Recargar la escena
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void TurnOffPlayerControllers()
    {
        input.inputActivated = false; 
        pathfinding.clickMovementActivated = false;
        
    }

    public void TurnOnPlayerControllers()
    {
        input.inputActivated = true;
        pathfinding.clickMovementActivated = true;
    }
}
