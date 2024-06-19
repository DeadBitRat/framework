using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBRDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Trigger Components")]
    public Collider2D triggerCollider; 


    [Header("DBR Dialogue System Elements")]
    public DBRDialogueSystem dialogueSystem;
    private DialoguePlayerManager dialoguePlayerManager;

    [Header("The Dialogue to Play")]

    public NPCDialogue dialogue;

    public DescriptorDialogue monologue; 

    [Header("Settings")]
    public bool playSceneOnStart;
    public bool playMonologueDialogueOnTouch; 


    // Start is called before the first frame update
    void Start()
    {
        dialogueSystem = FindObjectOfType<DBRDialogueSystem>();
        if (dialogueSystem == null)
        {
            Debug.LogError("No dialogue system found in the scene.");
        }

        if (playSceneOnStart)
        {
            PlayScene(); 
        }

        triggerCollider = GetComponent<Collider2D>();
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayScene()
    {
        PassingActorsList();
        dialogueSystem.FindAndAddActors();
        PassingTheDialogueScript();
        dialogueSystem.DisplayNextActingDialogueLine(); 
    }

    public void PassingActorsList()
    {
        dialogueSystem.actorsInScene.Clear();
        dialogueSystem.actorsInScene = dialogue.actorsInScene; 
    }

    public void PassingTheDialogueScript()
    {
        dialogueSystem.dialogueLines = null; 
        dialogueSystem.dialogueLines = dialogue.lines;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Buscar el componente DialoguePlayerManager en los hijos del objeto Player
            DialoguePlayerManager dialogueManager = collision.GetComponentInChildren<DialoguePlayerManager>();

            // Si se encuentra el componente, llamar al método ReadingDescriptor
            if (dialogueManager != null)
            {
                dialogueManager.ReadingDescriptor(monologue);
            }
            else
            {
                Debug.LogWarning("No DialoguePlayerManager found in the Player's children.");
            }

            // Desactivar el colisionador del trigger
            triggerCollider.enabled = false;
        }
    }

    #region Dialogue Player Manager Searcher

    DialoguePlayerManager FindChildWithDialoguePlayerManager(Transform parent)
    {
        // Iterate through each child of the parent
        foreach (Transform child in parent)
        {
            // Check if the child has the FunctionDetector component
            DialoguePlayerManager dialoguePlayer = child.GetComponent<DialoguePlayerManager>();
            if (dialoguePlayer != null)
            {
                return dialoguePlayer;
            }

            // Recursively check in the child's children
            dialoguePlayer = FindChildWithDialoguePlayerManager(child);
            if (dialoguePlayer != null)
            {
                return dialoguePlayer;
            }
        }
        return null;
    }

    #endregion
}
