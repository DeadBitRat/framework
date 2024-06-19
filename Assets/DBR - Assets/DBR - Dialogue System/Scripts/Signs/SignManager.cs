using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

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



}
