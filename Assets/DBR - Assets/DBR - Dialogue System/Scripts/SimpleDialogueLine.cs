using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SimpleDialogueLine
{
    public bool lineByPlayer; 
   
    [TextArea(3, 10)]
    public string dialogueLine;
}
