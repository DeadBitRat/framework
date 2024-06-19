using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


[ExecuteInEditMode]
public class DBRPlatform : MonoBehaviour
{

    public float platformWidth;
    public float platformHeight;
    private float _platformWidth; 
    private float _platformHeight;

    public Vector2 locationPoint;
    private Vector2 realLocationPoint; 


    private BoxCollider2D boxCollider2D;

    [SerializeField]
    private GameObject groundTag;
    private EdgeCollider2D edgeCollider2D;

    [HideInInspector]
    public int tileSizeIndex; 
    [HideInInspector]
    public string[] tileSize = new string[] { "10x10", "16x16", "32x32", "Other" };

    private float x;
    private float y;

    [SerializeField]
    private GameObject guideSprite;

    public bool showTestColorWhilePlaying; 





    public void Start()
    {
        if (Application.isPlaying)
        {
            PlatformConstruction();
            
        }
    }

    public void Update()
    {
        if (Application.isEditor) {

            PlatformConstruction();
            guideSprite.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (showTestColorWhilePlaying) {
            if (Application.isPlaying)
            {

                guideSprite.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else
        {

       
        if (Application.isPlaying)
        {
            
            guideSprite.GetComponent<SpriteRenderer>().enabled = false;
        }
        }





    }



    public void PlatformConstruction()
    {
       

        
        boxCollider2D = GetComponent<BoxCollider2D>();
        edgeCollider2D = groundTag.GetComponent<EdgeCollider2D>();
        

        

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

        realLocationPoint = new Vector2(locationPoint.x * x, locationPoint.y * y);

        transform.position = realLocationPoint;

        // 2D Box Collider Construction

        // Size

        boxCollider2D.size = new Vector2(platformWidth * x, platformHeight * y);
        boxCollider2D.offset = new Vector2(platformWidth * x/2, platformHeight * y/2);

        guideSprite.transform.localScale = new Vector2(platformWidth * x, platformHeight * y);
        guideSprite.transform.localPosition = new Vector2(platformWidth * x / 2, platformHeight * y / 2);

        // Edge Collider 2D points

        Vector2[] edgePoints = new Vector2[2];
        edgePoints[0] = new Vector2(0, platformHeight * y);
        edgePoints[1] = new Vector2(platformWidth * x, platformHeight * y); 

        edgeCollider2D.points = edgePoints;
    }

}
