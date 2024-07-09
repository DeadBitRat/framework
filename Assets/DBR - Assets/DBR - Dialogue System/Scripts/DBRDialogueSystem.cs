
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class DBRDialogueSystem : MonoBehaviour
{
    public InputDetector inputDetector;
    public DialoguePlayerManager dialoguePlayerManager;

    public List<string> actorsInScene;
    public List<DialogueActorManager> matchedActorsList;

    public DialogueActorManager actorTalking;

    public List<ActorListening> actorsListening;
    public List<ActiveActorListening> activeActorsListening;

    public DialogueLine[] dialogueLines;
    public DBRDialogue dialogue;


    public DialogueLine currentLine;
    public DialogueActorManager currentActor;

    public DBRDialogueEventsAssigner currentEvents;
    public bool hayEventos;
    public int chosenOption; 

    public int indexLine = 0;

    public UnityEvent eventoPrueba; 




    void Start()
    {
        // Check for duplicate actor names in the scene
        CheckForDuplicateActorNames();

        inputDetector = FindObjectOfType<InputDetector>();
        dialoguePlayerManager = FindObjectOfType<DialoguePlayerManager>();

        
    }

    #region Verificador de Actores Duplicados

    // This function will search through the scene for matching DialogueActorManager components
    public void FindAndAddActors()
    {
        // Clear the matched actors list to avoid duplicates
        matchedActorsList.Clear();

        // Get all root objects in the active scene
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();

        // Iterate through each actor name in the actorsInScene list
        foreach (string actorName in actorsInScene)
        {
            // Iterate through each root object in the scene
            foreach (GameObject obj in rootObjects)
            {
                // Get all DialogueActorManager components in the root object and its children
                DialogueActorManager[] actorComponents = obj.GetComponentsInChildren<DialogueActorManager>();

                // Check each component for a matching actor name
                foreach (DialogueActorManager actorComponent in actorComponents)
                {
                    if (actorComponent.actorName == actorName)
                    {
                        // Add the matching component to the matchedActorsList
                        matchedActorsList.Add(actorComponent);
                    }
                }
            }
        }
    }

    // This function will search the entire scene for DialogueActorManager components and warn about duplicates
    public void CheckForDuplicateActorNames()
    {
        // Dictionary to keep track of actor names and their counts
        Dictionary<string, int> actorNameCounts = new Dictionary<string, int>();

        // Get all root objects in the active scene
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();

        // Iterate through each root object in the scene
        foreach (GameObject obj in rootObjects)
        {
            // Get all DialogueActorManager components in the root object and its children
            DialogueActorManager[] actorComponents = obj.GetComponentsInChildren<DialogueActorManager>();

            // Count each actor name
            foreach (DialogueActorManager actorComponent in actorComponents)
            {
                string actorName = actorComponent.actorName;

                if (actorNameCounts.ContainsKey(actorName))
                {
                    actorNameCounts[actorName]++;
                }
                else
                {
                    actorNameCounts[actorName] = 1;
                }
            }
        }

        // Check for duplicates and log warnings
        foreach (KeyValuePair<string, int> entry in actorNameCounts)
        {
            if (entry.Value > 1)
            {
                Debug.LogWarning($"Duplicate actor name found: {entry.Key} (Count: {entry.Value})");
            }
        }
    }

    #endregion

    #region Display Next Acting Dialogue Line



    public IEnumerator DisplayNextActingDialogueLine()
    {
        Debug.Log("Displaying Next Acting Dialogue Line from the Dialogue System!"); 
        // Clear the texts of both the reader and the signboard.
        foreach (DialogueActorManager actor in matchedActorsList)
        {
            Debug.Log("Limpiando las burbujas de " + actor); 
            actor.bubbleText.text = "";
            actor.typedLine = "";
        }


        Debug.Log("Asignando el index actual con el index: " + indexLine); 
        SetCurrentDialogueLine(indexLine);

        

        if (currentLine.isEndOfDialogue)
        {
            Debug.Log("La línea es fin del diálogo!"); 
            EndDialogue();
            yield break;
        }

        currentActor = matchedActorsList[currentLine.indexActorTalking];
        currentLine = dialogueLines[indexLine]; 
        //dialogueSystem.currentActor = actorsInScene[currentLine.indexActorTalking];

        Debug.Log("Designando al actor que habla durante esta línea: " +  currentActor);

        #region Actor reading line

        // Identify who is the actor talking this line.
        int i = currentLine.indexActorTalking;

        Debug.Log("La línea actual es: " + i); 

        actorTalking = matchedActorsList[i];

        Debug.Log("Designando al actor que habla durante esta línea: " + actorTalking);


        //line.actorTalking.dialogueActorManager.isTalking = true;

        //yield return

        #region Eventos Pre-Dialogo
        Debug.Log("Hay Eventos pre-dialogos?");
        if (hayEventos)
        {
            Debug.Log("Hay eventos pre-diálogo!"); 
            currentEvents.events.preDialogueEvents?.Invoke();
        }

        #endregion

        Debug.Log("La linea es: " + currentLine.dialogueLine); 
        StartCoroutine(actorTalking.BubbleTalking(currentLine.dialogueLine));

        #region Case: There is a question in the line

        if (currentLine.isQuestion)
        {
            yield return new WaitUntil(() => currentActor.questionAnswered);
        }

        #endregion

        if (currentLine.nextLineDiffers)
        {
            indexLine = currentLine.nextLine;
        }
        else
        {
            indexLine = indexLine + 1;
        }





        // Playing the animation of actors listening. 
        foreach (ActorListening actorListening in currentLine.actorsListening)
        {
            foreach (DialogueActorManager activeActor in matchedActorsList)
            {
                if (actorListening.actorName == activeActor.actorName)
                {
                    // Instantiate the ActiveActorListening object
                    ActiveActorListening activeActorListening = new ActiveActorListening();

                    activeActorListening.actorName = activeActor.actorName;
                    activeActorListening.actorManager = activeActor.GetComponent<DialogueActorManager>();
                    activeActorListening.listeningClip = actorListening.actorListeningClip;
                    activeActorsListening.Add(activeActorListening);
                }
            }
        }


        foreach (ActiveActorListening actorListening in activeActorsListening)
        {
            if (actorListening.listeningClip != null)
            {
                actorListening.actorManager.animator.Play(actorListening.listeningClip.name);
                Debug.Log("Animando a los actores escuchando");
            }

        }







        #endregion
    }


    #endregion












    #region Set Current Dialogue Line
    public void SetCurrentDialogueLine(int indexLine)
    {
        if (indexLine >= 0 && indexLine < dialogueLines.Count())
        {
            Debug.Log("Comenzando la revisión");
            currentLine = dialogueLines[indexLine];
            Debug.Log("Setting the current line which is: " + currentLine.dialogueLine); 

        }
        else
        {
            HandleOutOfRangeIndex();
        }
    }

    #endregion

    #region Handle Out Out of Range Index
    private void HandleOutOfRangeIndex()
    {
        EndDialogue();
    }

    #endregion

    #region Ending Dialogue

    public void EndDialogue()
    {

        foreach (DialogueActorManager actor in matchedActorsList)
        {
            
            inputDetector.inputActivated = true;

            actor.bubbleText.text = "";
            actor.typedLine = "";
            actor.questionAnswered = false; 
        }
        matchedActorsList.Clear();
        indexLine = 0;
        dialogue = null;
        actorTalking = null; 

        if (dialoguePlayerManager != null)
        {
            
        }
         
        inputDetector.inputActivated = true;

        Debug.Log("End of dialogue");
    }
    #endregion

    #region Actors Listening Class

    [Serializable]
    public class ActiveActorListening
    {
        public string actorName;
        public DialogueActorManager actorManager;
        public AnimationClip listeningClip;
    }

    #endregion



    DBRDialogueEventsAssigner FindObjectWithEventName(string eventName)
    {
        // Find all objects in the scene with the DBRDialogueEventsAssigner component
        DBRDialogueEventsAssigner[] assigners = FindObjectsOfType<DBRDialogueEventsAssigner>();

        // Iterate through each assigner to find the one with the matching event name
        foreach (DBRDialogueEventsAssigner assigner in assigners)
        {
            if (assigner.nameOfTheEvent == eventName)
            {
                return assigner;
            }
        }

        // Return null if no matching object is found
        return null;
    }


    public void AssignOptionsToQuestions()
    {
        

    }


    public void AnswerQuestion()
    {
        currentActor.questionAnswered = true;
    }

    public int DebugNumberTest(int optionNumber)
    {
        return optionNumber; 
    }

    public void DebugTestAction (int number)
    {
        Debug.Log("You have chosen the option Number: " + number + "Yaaayy!!!"); 
    }



    


    public void DebugPrueba()
    {
        Debug.Log("probando esta mierda!!!"); 
    }

    public void InvokeOption1Events()

    {
         
        Debug.Log("Invocando al evento prueba de la opción 1 ");
        
        

    }

    public void InvokeOption2Events()

    {
        

    }

    public void InvokeOption3Events()

    {


    }

    public void InvokeOption4Events()

    {


    }

    public void InvokeOption5Events()

    {

    }


}



