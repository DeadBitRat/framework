using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class DBRDialogue : MonoBehaviour
{
    public NPCDialogue writtenDialogue;

    public List<string> actorsInScene;

    public DialogueLine[] directedDialogue;

    //public DialogueQuestion[] questions; 

    public DBRDialogueSystem dialogueSystem; 

    [Header("Dialogue Attributes")]

    public int entryIndex = 0; 


    // Start is called before the first frame update
    void Start()
    {
       dialogueSystem = FindObjectOfType<DBRDialogueSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor)
        {
            if (writtenDialogue != null)
            {
                actorsInScene = writtenDialogue.actorsInScene; 

                directedDialogue = new DialogueLine[writtenDialogue.lines.Length];
                int i = 0;
                foreach (DialogueLine line in writtenDialogue.lines)
                {
                   
                    directedDialogue[i] = writtenDialogue.lines[i];
                    i++;
                }
            }


        }
    }




}
