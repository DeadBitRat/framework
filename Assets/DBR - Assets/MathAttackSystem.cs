using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml;

public class MathAttackSystem : MonoBehaviour
{
    public string level; 
    
    public TMP_Text question;
    public TMP_Text exerciseText; 

    public int number1;
    public int number2;
    public int result;

    

    public CannonController cannonController;

    public bool isExerciseOver;

    public float spawnWaitingTime = 1f; 
    public List<GameObject> monstersList;
    public int monstersNumber;


    public TMP_InputField inputField;
    public int answerInt;
    public int answerFloat;

    public MathAttackHealthSystem healthSystem;
 


    // Start is called before the first frame update
    void Start()
    {
        inputField.interactable = false;
        exerciseText.text = null;

    }

    // Update is called once per frame
    void Update()
    {

        // Check if the input field is active, has an integer in its text field, and is not empty
        if (inputField != null && !string.IsNullOrEmpty(inputField.text) )
        {
            

           
                

               

                // Check if the user presses Enter or Return
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                int intValue = int.Parse(inputField.text); 
                    
                    Answering(intValue);
                    inputField.interactable = false;
                }

               
            
        }

        if (monstersList.Count == 0)
        {
            isExerciseOver = true; 
        }

        if (cannonController.cannonActivated && cannonController.cannonMode == "MathAttack" && !healthSystem.healthBarShown)
        {
            StartCoroutine(healthSystem.ShowingHealthBar()); 
        }

        else if (!cannonController.cannonActivated)
        {
            StartCoroutine(healthSystem.HidingHealthBar()); 
        }


    }


    public void GenerateExercise()
    {
        isExerciseOver = false; 

        number1 = UnityEngine.Random.Range(0, 11);
        number2 = UnityEngine.Random.Range(0, 11);
        result = number1 + number2;

        string exercise = number1.ToString() + " + " + number2.ToString() + " = ";
        exerciseText.text = exercise;

        StartCoroutine(GenerateMonsters());

        inputField.interactable = true;
        inputField.ActivateInputField();
        inputField.Select(); 



        //StartCoroutine(cannonController.MathAttackShooting(result)); 
     

        
    }

    public IEnumerator GenerateMonsters()
    {
        yield return new WaitForSeconds(spawnWaitingTime);

        if (level == "Level 1")
        {
            monstersNumber = result;

            for (int i = 0; i < monstersNumber; i++)
            {
                float randomX = Random.Range(3f, 4f + 0.001f);
                float randomY = Random.Range(0.48f, 0.49f + 0.001f);
                Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

                GameObject monster = Instantiate(monstersList[0], spawnPosition, Quaternion.identity);
                
                EnemySimpleMovement movement = monster.GetComponent<EnemySimpleMovement>();
                movement.xDirection = "Left";
                movement.xSpeed = 0.25f; 

                HealthSystem monsterHealth = monster.GetComponent<HealthSystem>();
                monsterHealth.maxHealth = 1; 
                
                

                
            }


        }

       

    }

    public void Answering(int value)

    {
        // Your code for handling the input value goes here
        Debug.Log("Answering with value: " + value);

        answerInt = value;

        if (result == answerInt) {
            Debug.Log("La respuesta es correcta"); 
        StartCoroutine(cannonController.MathAttackShooting(value));
        }

        else
        {
            Debug.Log("La respuesta es Incorrecta"); 
        }

        exerciseText.text = null; 
        inputField.text = null;
        isExerciseOver = true;

        StartCoroutine(WaitingForResults()); 
    }

    public IEnumerator WaitingForResults()
    {
        yield return new WaitForSeconds(2f);
        cannonController.canShoot = true;

    }


}
