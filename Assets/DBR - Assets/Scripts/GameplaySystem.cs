using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySystem : MonoBehaviour
{
   

    // Types of Gameplay

    // 1- "Platformer"
    // 2- "Topdown"
    // 3- "2point5"

    public GameplayType gameplay;

    public enum GameplayType
    {
        Platformer,
        TopDown,
        twoPointFive,
        ClickAdventure

    }


    public bool characterActivated;


   
    public GameObject playerBase;
    public GameObject character;
    public GroundDetector groundDetector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
