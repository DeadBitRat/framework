using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class NumberPlate : MonoBehaviour 
{
    public Transform superior;
    public Transform inferior;

    public string state;

    public float supAngle;
    public float infAngle; 

    private void Update()
    {   
        supAngle = superior.transform.rotation.eulerAngles.x;
        infAngle = inferior.transform.rotation.eulerAngles.x;

        if (supAngle == -90 && infAngle == 0)
        {
            state = "Behind"; 
        }

        if (supAngle == 0 && infAngle == 0)
        {
            state = "Current";
        }

        if (supAngle == 0 && infAngle == 90)
        {
            state = "Ahead";
        }
    }

}
