using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class CameraTileLocator : MonoBehaviour
{
    [HideInInspector]
    public int tileSizeIndex;
    [HideInInspector]
    public string[] tileSize = new string[] { "10x10", "16x16", "32x32", "Other" };

    public Vector2 locationPoint;
    private Vector3 realLocationPoint;

    private float x;
    private float y;


    // Start is called before the first frame update
    void Start()
    {
        if (Application.isPlaying)
        {

            Locating();

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {

            Locating();

        }
    }



    public void Locating()
    {
        // Size of Map
        if (tileSizeIndex == 0)
        {
            x = 0.1f;
            y = 0.1f;
        }

        else if (tileSizeIndex == 1)
        {
            x = 0.16f;
            y = 0.16f;
        }

        else if (tileSizeIndex == 2)
        {
            x = 0.32f;
            y = 0.32f;
        }

        else if (tileSizeIndex == 3)
        {
            Debug.Log("Todavia no defino el 'otros'");
        }


        realLocationPoint = new Vector3(locationPoint.x * x, locationPoint.y * y, -10f);

        transform.position = realLocationPoint;


    }

}

