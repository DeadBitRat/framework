using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoverController : MonoBehaviour
{
    public bool isSplashScreenOver; 
    // Start is called before the first frame update
    void Start()
    {
        isSplashScreenOver = false; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void FinishingSplashScreen()
    {
        isSplashScreenOver = true;
    }
}
