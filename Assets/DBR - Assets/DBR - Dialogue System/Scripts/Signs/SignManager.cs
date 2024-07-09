using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class SignManager : MonoBehaviour
{
    public TextMeshPro signBubble;
    
    public SignDialogue signDialogue;

    


    public void Start()
    {
        if (Application.isPlaying)
        {
            signBubble.text = ""; 

        }
    }

    public void StopReading()
    {
        signBubble.text = ""; 
    }

}
