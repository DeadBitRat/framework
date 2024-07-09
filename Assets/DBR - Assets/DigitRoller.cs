using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class DigitRoller : MonoBehaviour
{
    public int currentIndex;
    public int targetIndex;

    public NumberPlate[] numberPlates;


    public bool rollingUp = false;
    public bool rollingUpSuperior = false;
    public bool rollingUpInferior = false;

    public bool rollingDown = false;
    public bool rollingDownSuperior = false;
    public bool rollingDownInferior = false;

    public float rotationBaseSpeed = 1f;
    public float rotationRange; 
    private float rotationSpeed = 1f;

    public float angleRotationSuperior;
    public float angleRotationInferior;

    public Transform superior;
    public Transform inferior;

    public bool targetReached;

    public AudioSource audioSource; 

    // Start is called before the first frame update
    void Start()
    {
        superior = numberPlates[currentIndex].superior;
        inferior = numberPlates[currentIndex].inferior;

        audioSource = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (rollingUp)
        {

            RollUp();
        }


        if (rollingDown)
        {

            RollDown();
        }
    }

    #region Roll up 
    public void RollUp()
    {
        CalculateRotationSpeed();

        int placaSiguiente = currentIndex + 1;
        Debug.Log("El índice de la placa siguiente es: " + placaSiguiente + " Y el tamaño del arra y es: " + numberPlates.Length);


        if (placaSiguiente < numberPlates.Length) //Si el índice de la placa es menor a la cantidad de placas, entonces asignamos las mitades de placas a mover durante el el rolleo. 
        {
            superior = numberPlates[currentIndex].superior;
            inferior = numberPlates[currentIndex + 1].inferior;
        }

        else
        {
            Debug.Log("No existe una placa más arriba");
            return;
        }



        // Si la rotación aún no ha alcanzado -90 grados en la placa superior
        if (!rollingUpInferior)
        {
            Debug.Log("Rotando hacia arriba!");

            angleRotationSuperior -= Time.deltaTime * rotationSpeed;

            // Rotar la placa superior hacia el ángulo objetivo
            superior.rotation = Quaternion.Euler(angleRotationSuperior, 0f, 0f);

            if (angleRotationSuperior <= -90f)
            {
                Debug.Log("El ángulo de rotación alcanzó los -90°");
                superior.rotation = Quaternion.Euler(-90f, 0f, 0f);
                angleRotationInferior = 90f;
                rollingUpSuperior = false;
                rollingUpInferior = true;

            }

        }

        if (rollingUpInferior)
        {
            angleRotationInferior -= Time.deltaTime * rotationSpeed;

            inferior.rotation = Quaternion.Euler(angleRotationInferior, 0f, 0f);


            if (angleRotationInferior <= 0f)
            {
                inferior.rotation = Quaternion.Euler(0f, 0f, 0f);

                currentIndex += 1;
                StopRotation();
            }

            CheckIfTargetReached();

            audioSource.Play();

            if (!targetReached)
            {
                Debug.Log("No he terminado... cambiando otra vez");
                ChangeNumber();
            }

            else
            {
                rollingUp = false;
                rollingDown = false;

            }
        }
    }

    #endregion

    public void RollDown()
    {
        CalculateRotationSpeed();

        int placaAnterior = currentIndex - 1;
        Debug.Log("El índice de la placa anterior es: " + placaAnterior + " Y el tamaño del arra y es: " + numberPlates.Length);

        if (placaAnterior >= 0) //Si el índice de la placa es menor a la cantidad de placas, entonces asignamos las mitades de placas a mover durante el el rolleo. 
        {
            superior = numberPlates[currentIndex - 1].superior;
            inferior = numberPlates[currentIndex].inferior;
        }

        else
        {
            Debug.Log("No existe una placa más abajo");
            return;
        }



        if (!rollingDownSuperior)
        {
            Debug.Log("Rotando hacia abajo! Parte Inferior");
            angleRotationInferior += Time.deltaTime * rotationSpeed;
            inferior.rotation = Quaternion.Euler(angleRotationInferior, 0f, 0f);

            if (angleRotationInferior >= 90f)
            {
                Debug.Log("El ángulo de rotación alcanzó los 90°");
                inferior.rotation = Quaternion.Euler(90f, 0f, 0f);
                angleRotationSuperior = -90f;
                rollingDownInferior = false;
                rollingDownSuperior = true;

            }

        }

        if (rollingDownSuperior)
        {
            Debug.Log("Rotando hacia abajo! Parte Superior");
            angleRotationSuperior += Time.deltaTime * rotationSpeed;
            superior.rotation = Quaternion.Euler(angleRotationSuperior, 0f, 0f);


            if (angleRotationSuperior >= 0f)
            {
                superior.rotation = Quaternion.Euler(0f, 0f, 0f);
                currentIndex -= 1;
                StopRotation();
            }

            CheckIfTargetReached();
            audioSource.Play();

            if (!targetReached)
            {
                Debug.Log("No he terminado... cambiando otra vez");
                ChangeNumber();
            }

            else
            {
                rollingUp = false;
                rollingDown = false;

            }

        }



    }

    public void ChangeNumber()
    {
        Debug.Log("Cambiando el número");

        if (currentIndex == targetIndex)
        {
            Debug.Log("El index objetivo es el mismo");
            return;
        }

        else if (currentIndex < targetIndex)
        {

            RollingUp();

        }

        else if (currentIndex > targetIndex)
        {
            RollingDown();
        }
    }



    public void RollingUp()
    {
        rollingUp = true;
    }

    public void RollingDown()
    {
        rollingDown = true;
    }

    void StopRotation()
    {
        

        rollingUpSuperior = false;
        rollingUpInferior = false;


        rollingDownSuperior = false;
        rollingDownInferior = false;

        angleRotationSuperior = 0f;
        angleRotationInferior = 0f;

        Debug.Log("Rotación terminada");

    }

    void CheckIfTargetReached()
    {
        if (currentIndex == targetIndex)
        {
            targetReached = true;
        }

        else { targetReached = false; }
    }



    void CalculateRotationSpeed()
    {
        // Calculate a random deviation within the specified range
        float deviation = Random.Range(-rotationRange, rotationRange);
        // Calculate the rotation speed based on the base speed and deviation
        rotationSpeed = rotationBaseSpeed + deviation;
    }
}
