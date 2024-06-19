using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Ladder : MonoBehaviour
{
    
    public float ladderHeight;
    public float ladderCenter; 

    public Vector2 locationPoint;
    private Vector2 realLocationPoint;


    private BoxCollider2D boxCol;

    
    public GameObject ladderTop;
    public GameObject ladderBottom;
    public GameObject hatch; 

    // Start is called before the first frame update
    public void Start()
    {
        if (Application.isPlaying)
        {
            LadderConstruction();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (Application.isEditor)
        {

            LadderConstruction();
        }


    }

    public void LadderConstruction()
    {
       

       
        Debug.LogWarning("ARREGLAR ESTO PARA QUE EL TAMAÑO DE LA ESCALERA DEPENDA DEL TAMAÑO DE LOS TILES SELEECIONADOS POR EL USUARIO");

        boxCol = GetComponent<BoxCollider2D>();


        
        realLocationPoint = new Vector2(locationPoint.x * 0.16f, locationPoint.y * 0.16f);
        ladderCenter = locationPoint.x * 0.16f + 0.08f; 

        transform.position = realLocationPoint;

        boxCol.isTrigger = true;
        boxCol.size = new Vector2(0.04f, ladderHeight * 0.16f);
        boxCol.offset = new Vector2(0.08f, ladderHeight * 0.16f / 2);

        ladderTop.transform.localPosition = new Vector2(0.08f, ladderHeight * 0.16f);
        ladderTop.GetComponent<BoxCollider2D>().size = new Vector2(0.08f, 0.025f);
        ladderTop.GetComponent<BoxCollider2D>().isTrigger = true;

        ladderBottom.transform.localPosition = new Vector2(0.08f, 0f);
        ladderBottom.GetComponent<BoxCollider2D>().size = new Vector2(0.08f, 0.025f);
        ladderBottom.GetComponent<BoxCollider2D>().isTrigger = true;

        hatch.transform.localPosition = new Vector2(0.08f, ladderHeight*0.16f - 0.08f); 

    }

}
