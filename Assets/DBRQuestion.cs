using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class DBRQuestion : MonoBehaviour
{
    [Header("Option 1")]
    public string option1Text;
    
    public int option1NextLineIndex;

    [Header("Option 2")]
    public string option2Text;
    
    public int option2NextLineIndex;

    [Header("Option 3")]
    public string option3Text;
    
    public int option3NextLineIndex;

    [Header("Option 4")]
    public string option4Text;
    
    public int option4NextLineIndex;

    [Header("Option 5")]
    public string option5Text;

    [Header("Question Settings")]
    public int option5NextLineIndex;

    [Header("Events to Play when Asked")]

    [Tooltip("0 for no change")]
    public int indexEntryNumberWhenAsked; 

    public UnityEvent toExecuteWhenAsked; 


    [Header("DBR Interactive Dialogue Elements")]
    public GameObject dialogueCanvas;
    public GameObject dialoguePanel;
    public GameObject inputPanel;
    public GameObject answerPanel;

    [Header("Buttons")]
    public GameObject option1Button;
    public GameObject option2Button;
    public GameObject option3Button;
    public GameObject option4Button;
    public GameObject option5Button;

    [Header("Buttons' Texts")]
    public TMP_Text option1ButtonText;
    public TMP_Text option2ButtonText;
    public TMP_Text option3ButtonText;
    public TMP_Text option4ButtonText;
    public TMP_Text option5ButtonText;

    [Header("Dialogue Elements")]
    public DBRDialogue dialogue; 



    private void OnEnable()
    {
        //toExecuteWhenAsked.Invoke(); 
    }


    // Start is called before the first frame update
    void Start()
    {
        TurnOffDialoguePanels();
        toExecuteWhenAsked.AddListener(ChangeEntryIndexNumber);
    }
    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor)
        {
            option1ButtonText.text = option1Text;
            option2ButtonText.text = option2Text;
            option3ButtonText.text = option3Text;
            option4ButtonText.text = option4Text;
            option5ButtonText.text = option5Text;
        }
    }



    public void TurnOnDialogueQuestionPanel()
    {
        dialogueCanvas.SetActive(true);
        dialoguePanel.SetActive(true);
        inputPanel.SetActive(false);
        answerPanel.SetActive(true);
    }


    public void TurnOnDialogueInputAnswerPanel()
    {
        dialogueCanvas.SetActive(true);
        dialoguePanel.SetActive(true);
        inputPanel.SetActive(true);
        answerPanel.SetActive(false);
    }
    public void TurnOffDialoguePanels()
    {
        dialogueCanvas.SetActive(false);
        dialoguePanel.SetActive(false);
        inputPanel.SetActive(false);
        answerPanel.SetActive(false);
    }

    public void ChangeEntryIndexNumber()
    {
        if (indexEntryNumberWhenAsked != 0)
        {

       
        Debug.Log("Cambianding el dialogue entry index!"); 
        dialogue.entryIndex = indexEntryNumberWhenAsked;
        }
    }


    public void InvokeEventsWhenAsked()
    {
        Debug.Log("Según la pregunta, el dialogo es: " + dialogue); 
        Debug.Log("Ejecutando Eventos cuando se pregunta"); 
        toExecuteWhenAsked.Invoke();
        ChangeEntryIndexNumber();
        Debug.Log("Eventos ejecutados (supueeestamente)");
    }

}
