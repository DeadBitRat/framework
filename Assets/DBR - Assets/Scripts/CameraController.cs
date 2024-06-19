using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class CameraController : MonoBehaviour
{
    public GameObject camera; 
    public GameObject target;
    public Transform cameraLocation;

    

    public float leftLimit;
    public float rightLimit;
    public float inferiorLimit;
    public float superiorLimit;

 


    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (target.transform.position.x <= leftLimit)
        {
            camera.transform.position = new Vector3(leftLimit, target.transform.position.y, -10f);
        }

        else if (target.transform.position.x >= rightLimit)
        {
            camera.transform.position = new Vector3(rightLimit, target.transform.position.y, -10f);
        }

        else
        {
            camera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10f);
        }



    }

}
