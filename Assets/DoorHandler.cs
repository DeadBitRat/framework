using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    private FunctionDetector functionDetector; 
    private InputDetector inputDetector;
    private InventorySystem inventorySystem;
    private DialoguePlayerManager dialoguePlayerManager;
    private DialogueActorManager dialogueActorManager;

    private string nameOfTheKey;

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        functionDetector = FindChildWithFunctionDetector(gameObject.transform); 
        if (functionDetector == null )
        {
            Debug.LogWarning("No se encontró Function Detector!!! NOOO!"); 
        }

        inputDetector = GetComponent<InputDetector>();
        if (inputDetector == null )
        {
            Debug.LogWarning("No se encontró Input Detector!!! NOOO!!!"); 
        }

        inventorySystem = GetComponent<InventorySystem>(); 
        if (inventorySystem == null )
        {
            Debug.LogWarning("No se encontró Inventory System!!! NOOO!!!");
        }

        dialoguePlayerManager = FindChildWithDialoguePlayerManager(gameObject.transform);
        if (dialoguePlayerManager == null)
        {
            Debug.LogWarning("No se encontró el Dialogue Player Manager!!! NOOO!");
        }

        dialogueActorManager = FindChildWithDialogueActorManager(gameObject.transform);
        if (dialogueActorManager == null)
        {
            Debug.LogWarning("No se encontró el Dialogue Player Manager!!! NOOO!");
        }
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        if (functionDetector.doorController != null)
        {
            if (inputDetector.functionPressed && (!dialogueActorManager.readingDescription && !dialoguePlayerManager.envolvedInDialogue && !dialoguePlayerManager.readingSign) )
            {
                if (functionDetector.doorController.isLocked) { 
                    if (functionDetector.doorController.nameOfKey != "")
                    {
                        SearchingForTheKey(functionDetector.doorController.nameOfKey);
                    }

                    else
                    {
                        // Crear una instancia de DescriptorLine con el diálogo específico
                        DescriptorLine line = new DescriptorLine
                        {
                            dialogueLine = "No existe llave para esta puerta"
                        };

                        // Crear una instancia de DescriptorDialogue y asignar la línea de diálogo
                        DescriptorDialogue descriptorWhenKeyNotExists = new DescriptorDialogue
                        {
                            descriptionLines = new DescriptorLine[] { line }
                        };

                        dialoguePlayerManager.ReadingDescriptor(descriptorWhenKeyNotExists); 
                    }
                
                }
            }
        }
    }



    public void SearchingForTheKey(string nameOfTheKey)
    {
        bool keyFound = false;

        foreach (Keys key in inventorySystem.inventory.keys)
        {
           

            if (key.nameOfTheKey == nameOfTheKey)
            {
                Debug.Log("Tengo la llave!");
                
                keyFound = true; 
                
            }
        }

        if (keyFound)
        {
            functionDetector.doorController.isLocked = false;
            
            dialoguePlayerManager.ReadingDescriptor(functionDetector.doorController.descriptionWhenOpened);
            functionDetector.doorController.OpenDoor();

        }

        else
        {
            dialoguePlayerManager.ReadingDescriptor(functionDetector.doorController.descriptionWhenClosed);
        }
    }


    #region Function Detector Searcher

    FunctionDetector FindChildWithFunctionDetector(Transform parent)
    {
        // Iterate through each child of the parent
        foreach (Transform child in parent)
        {
            // Check if the child has the FunctionDetector component
            FunctionDetector detector = child.GetComponent<FunctionDetector>();
            if (detector != null)
            {
                return detector;
            }

            // Recursively check in the child's children
            detector = FindChildWithFunctionDetector(child);
            if (detector != null)
            {
                return detector;
            }
        }
        return null;
    }

    #endregion

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


    #region Dialogue Player Manager Searcher

    DialogueActorManager FindChildWithDialogueActorManager(Transform parent)
    {
        // Iterate through each child of the parent
        foreach (Transform child in parent)
        {
            // Check if the child has the FunctionDetector component
            DialogueActorManager dialoguePlayer = child.GetComponent<DialogueActorManager>();
            if (dialoguePlayer != null)
            {
                return dialoguePlayer;
            }

            // Recursively check in the child's children
            dialoguePlayer = FindChildWithDialogueActorManager(child);
            if (dialoguePlayer != null)
            {
                return dialoguePlayer;
            }
        }
        return null;
    }

    #endregion
}
