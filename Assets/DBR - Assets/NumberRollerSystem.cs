using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberRollerSystem : MonoBehaviour
{
    public int number;


    public GameObject[] digitOnelates;

    public DigitRoller[] digitRollers;


    public int[] digits = new int[6];


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void ExtractDigits(int number)
    {
       

        for (int i = 0; i < 6; i++)
        {
            digits[i] = number % 10;
            number /= 10;
        }

        // Now digits array contains all the digits of the number in the correct positional order
        // You can iterate through the array or use individual indices to access each digit
        for (int i = 0; i < 6; i++)
        {
            if (digits[i] == 0)
            {
                if (i == 0 || digits[i - 1] == 0)
                {
                    digitRollers[i].targetIndex = 0; // Si es un 0 a la izquierda o es el primer dígito
                }
                else
                {
                    digitRollers[i].targetIndex = 1; // Si es un 0, pero no es un 0 a la izquierda
                }
            }
            else
            {
                digitRollers[i].targetIndex = digits[i] + 1; // Si el dígito es mayor que 0
            }

            Debug.Log("Digit at position " + (i + 1) + " is: " + digits[i]);
        }
    }

    public IEnumerator UsingNumberRoller(int number)
    {
        ExtractDigits(number);
        for (int i = 0; i < 6; i++)
        {
            yield return new WaitForSeconds(0.2f);
            digitRollers[i].ChangeNumber();
        }
            
        
    }

    public void StartRoller(int number)
    {
        StartCoroutine(UsingNumberRoller(number));
    }

}
