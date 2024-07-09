using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private SaveLoadManager saveLoadManager;
    public int playerScore;
    public bool playerHasKey;

    void Start()
    {
        saveLoadManager = GetComponent<SaveLoadManager>();

        // Cargar las variables al iniciar la escena
        playerScore = saveLoadManager.LoadInt("PlayerScore", 0);
        playerHasKey = saveLoadManager.LoadBool("PlayerHasKey", false);
    }

    void Update()
    {
        /*
        // Ejemplo para actualizar el puntaje del jugador y guardar el valor
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerScore += 10;
            saveLoadManager.SaveInt("PlayerScore", playerScore);
        }

        // Ejemplo para actualizar el estado del jugador y guardar el valor
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerHasKey = !playerHasKey;
            saveLoadManager.SaveBool("PlayerHasKey", playerHasKey);
        }
        */
      

    }

   public void ReLoadScene()
    {
        // Recargar la escena para probar la persistencia del valor
        
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }
}
