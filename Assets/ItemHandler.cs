using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    private FunctionDetector functionDetector;
    private InputDetector inputDetector;
    private InventorySystem inventorySystem;
    private DialoguePlayerManager dialoguePlayerManager;


    public void Start()
    {
        functionDetector = FindChildWithFunctionDetector(gameObject.transform);
        if (functionDetector == null)
        {
            Debug.LogWarning("No se encontró Function Detector!!! NOOO!");
        }

        inputDetector = GetComponent<InputDetector>();
        if (inputDetector == null)
        {
            Debug.LogWarning("No se encontró Input Detector!!! NOOO!!!");
        }

        inventorySystem = GetComponent<InventorySystem>();
        if (inventorySystem == null)
        {
            Debug.LogWarning("No se encontró Inventory System!!! NOOO!!!");
        }

        dialoguePlayerManager = FindChildWithDialoguePlayerManager(gameObject.transform);
        if (dialoguePlayerManager == null)
        {
            Debug.LogWarning("No se encontró el Dialogue Player Manager!!! NOOO!");
        }
    }


    public void Update()
    {
        if (functionDetector.doorKeyController != null)
        {
            if (inputDetector.functionPressed)
            {
                inventorySystem.inventory.keys.Add(functionDetector.doorKeyController.key);
                dialoguePlayerManager.ReadingDescriptor(functionDetector.doorKeyController.descriptionWhenKeyIsPickedUp);
                functionDetector.doorKeyController.DestroyKey(); 
            }
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
}
