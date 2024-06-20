using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionDetector : MonoBehaviour
{
    [Header("Player Components")]
    public DialoguePlayerManager dialoguePlayerManager;
    public MachinesController machineController;


    [Header("Detected Dialogue Components")]
    public SignManager sign;
    public DBRFunctionDescriptor descriptor; 
    public DialogueActorManager nPCActorManager; 
    public SimpleNPCDialogueManager simpleNPCDialogueManager;
    public ActorNPCDialogueManager actorNPCDialogueManager;


    [Header("Doors")]

    public DBRDoorController doorController; 
    public DoorKeyController doorKeyController;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        #region Sign Detected
        if (collision.CompareTag("Sign"))
        {
            sign = collision.GetComponent<SignManager>(); 
        }
        #endregion

        #region Descriptor Detected
        if (collision.CompareTag("Descriptor"))
        {
            descriptor = collision.GetComponent<DBRFunctionDescriptor>();
        }
        #endregion

        #region Simple NPC detected
        if (collision.CompareTag("SimpleNPC"))
        {
            nPCActorManager = collision.GetComponent<DialogueActorManager>();
            simpleNPCDialogueManager = collision.GetComponent<SimpleNPCDialogueManager>();

            simpleNPCDialogueManager.dialoguePlayerManager = dialoguePlayerManager; 
        }
        #endregion

        #region Actor NPC Detected
        if (collision.CompareTag("ActorNPC"))
        {
            nPCActorManager = collision.GetComponent<DialogueActorManager>();
            actorNPCDialogueManager = collision.GetComponent<ActorNPCDialogueManager>();

            actorNPCDialogueManager.playerDialogueManager = dialoguePlayerManager;
           
        }

        #endregion

        #region Machine Detected
        if (collision.CompareTag("Machine"))
        {
            if(collision.GetComponent<MachineActivatorPanel>() != null)
            {
                
                machineController.machineActivatorPanel = collision.GetComponent<MachineActivatorPanel>();
            }
            Debug.Log("Estoy detectando una maquina"); 
        }

        #endregion

        #region Door Detected
        if (collision.CompareTag("Door"))
        {
            doorController = collision.GetComponent<DBRDoorController>();
        }
        #endregion

        #region Key Detected
        if (collision.CompareTag("Key"))
        {
            doorKeyController = collision.GetComponent<DoorKeyController>();
        }
        #endregion

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Sign"))
        {
            sign = null; 
        }

        if (collision.CompareTag("Descriptor"))
        {
            descriptor = null; 
        }


        if (collision.CompareTag("SimpleNPC"))
        {
            simpleNPCDialogueManager.dialoguePlayerManager = null;
            simpleNPCDialogueManager = null;
            
        }

        if (collision.CompareTag("ActorNPC"))
        {
            actorNPCDialogueManager.playerDialogueManager = null;
            nPCActorManager = null;
            actorNPCDialogueManager = null;

            

        }

        if (collision.CompareTag("Machine"))
        {

            machineController.machineActivatorPanel = null; 
            
            
        }

        if (collision.CompareTag("Door"))
        {
            doorController = null;
        }

        #region Door Detected
        if (collision.CompareTag("Key"))
        {
            doorKeyController = null;
        }
        #endregion

    }
}
