using System.Reflection;
using UnityEngine;
using UnityEngine.Events; // UnityEvent lives here
using UnityEngine.UI; // For accessing the Button component

[RequireComponent(typeof(Button))] // Ensure there's a Button component on the GameObject
public class DBRButton : MonoBehaviour
{
    public DBRDialogueSystem dialogueSystem; 

    public UnityEvent optionEvents = new UnityEvent();
  


    void Start()
    {
        dialogueSystem = FindObjectOfType<DBRDialogueSystem>();

        // Get the Button component and add a listener for when it's clicked
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("Button component not found on the GameObject.");
        }

        optionEvents.AddListener(dialogueSystem.AnswerQuestion); 
    }

   public void OnButtonClick()
    {
        optionEvents.GetPersistentEventCount();
        
        Debug.Log("Hay " + optionEvents.GetPersistentEventCount() + " eventos");
        optionEvents.Invoke();
        
        /*
        switch (this.name)
        {
            case "Button (1)":
                Debug.Log("From DBR Button: Button 1 Pressed!!!");
                option1Events.Invoke(); 


                break;
            case "Button (2)":
                Debug.Log("Button 2 Pressed!!!");
                option2Events.Invoke();
                break;
            case "Button (3)":
                Debug.Log("Button 3 Pressed!!!");
                option3Events.Invoke();
                break;
            case "Button (4)":
                Debug.Log("Button 4 Pressed!!!");
                option4Events.Invoke();
                break;
            case "Button (5)":
                Debug.Log("Button 5 Pressed!!!");
                option2Events.Invoke();
                break;
            default:
                Debug.Log("No specific action defined for this button.");
                break;
        }
        */
    }



    }