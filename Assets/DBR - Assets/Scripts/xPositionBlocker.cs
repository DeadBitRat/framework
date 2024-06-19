using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xPositionBlocker : MonoBehaviour
{
   
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector2(0f, transform.localPosition.y); 
    }
}
