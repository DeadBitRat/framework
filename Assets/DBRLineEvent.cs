using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DBRLineEvent : MonoBehaviour
{
    //public LineChangeEvent lineChangeEvent;
    [Header("Events to Trigger")]

    public bool changeALine;


    [Header("Parameters")]
    public DBRDialogue dialogue;
    public int indexToChange;
    public string newLine;

    public bool triggered; 
    

    //public UnityEvent eventsBeforeLine; 
    // Start is called before the first frame update
    void Start()
    {
        triggered = false; 


    }

    public void TriggerEvents()
    {
        Debug.Log("Triggering events!!!"); 
        triggered = true; 

        if (changeALine)
        {
            ChangeALine(dialogue, indexToChange, newLine);
        }

    }

    public void ChangeALine(DBRDialogue dialogue, int index, string newLine)
    {
        dialogue.directedDialogue[index].dialogueLine = newLine;
    }

}
