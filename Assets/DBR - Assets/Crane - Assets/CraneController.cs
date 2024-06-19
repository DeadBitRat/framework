using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneController : MonoBehaviour
{
    public MachineActivator machineActivator; 
    public GameObject clawLeft; // Objeto Claw que quieres rotar
    public GameObject clawRight; // Objeto Claw que quieres rotar
    public float cierre; // Variable para controlar la apertura/cierre

    // Definir los ángulos de rotación correspondientes a cierre 0 y 1
    public float anguloCierre0 = 60f;
    public float anguloCierre1 = 30f;

    public float craneClawSpeed = 1f; // Velocidad de cambio de cierre

    public bool craneActivated;
    public float craneMovementSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (machineActivator.activated)
        {
            if (Input.GetKey(KeyCode.E))
            {
                cierre += craneClawSpeed * Time.deltaTime;
            }
            // Mantener presionada la tecla Q para disminuir el valor de cierre
            else if (Input.GetKey(KeyCode.Q))
            {
                cierre -= craneClawSpeed * Time.deltaTime;
            }



            // Movimiento Horizontal y Vertical de la Grúa

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * craneMovementSpeed * Time.deltaTime;
            transform.Translate(movement);

        }

        // Asegúrate de que cierre está entre 0 y 1
        cierre = Mathf.Clamp01(cierre);

        // Interpolación lineal entre los ángulos de cierre 0 y 1 basado en el valor de "cierre"
        float anguloRotacionLeft = Mathf.Lerp(-1f * anguloCierre0, anguloCierre1, cierre);
        float anguloRotacionRight = Mathf.Lerp(anguloCierre0, -1f * anguloCierre1, cierre);

        // Aplica la rotación al eje Z del objeto "claw"
        clawLeft.transform.rotation = Quaternion.Euler(0, 0, anguloRotacionLeft);
        clawRight.transform.rotation = Quaternion.Euler(0, 0, anguloRotacionRight);
    }
}
