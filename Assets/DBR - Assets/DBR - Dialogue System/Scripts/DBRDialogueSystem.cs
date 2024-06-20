using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DBRDialogueSystem : MonoBehaviour
{
   

    public List<string> actorsInScene;
    public List<DialogueActorManager> matchedActorsList;

    public DialogueActorManager actorTalking;

    public List<ActorListening> actorsListening;
    public List<ActiveActorListening> activeActorsListening;

    public DialogueLine[] dialogueLines;

    public InputDetector inputDetector;

    public DialogueLine currentLine;
    public DialogueActorManager currentActor;

    public DBRDialogueEventsAssigner currentEvents;
    public bool hayEventos; 

    public int indexLine = 0;

    void Start()
    {
        // Check for duplicate actor names in the scene
        CheckForDuplicateActorNames();
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

    public void DisplayNextActingDialogueLine()
    {
        inputDetector.inputActivated = false;

        // Los tres siguientes son para limpiar los textos tanto del lector como del letrero. 
        foreach (DialogueActorManager actor in matchedActorsList)
        {
            actor.bubbleText.text = "";
            actor.typedLine = "";
        }

        actorsListening.Clear();
        activeActorsListening.Clear(); 

        SetCurrentDialogueLine(indexLine);

        if (currentLine.isEndOfDialogue)
        {
            Debug.Log("Finalizando el diálogo");
            EndDialogue();
            return;
        }
        Debug.Log("Línea de Guión #" + indexLine);

        if (matchedActorsList.Count() != 0)
        { 
        currentActor = matchedActorsList[currentLine.indexActorTalking];
        }

        else

        {
            Debug.LogWarning("No tenemos lista de actores"); 
        }

        actorsListening.Clear();

        // Getting the Events

        // First we clean the variable... just in case. 
        currentEvents = null;

        if (currentLine.dialogueEventsName != "")
        {
            currentEvents = FindObjectWithEventName(currentLine.dialogueEventsName);
            hayEventos = true; 
        }

        else
        {
            hayEventos = false;
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
            }

        }

        #region Eventos Pre-Dialogo

        if (hayEventos)
        {
            currentEvents.events.preDialogueEvents?.Invoke();
        }

        #endregion


        #region Actor reading line

        // Identify who is the actor talking this line. 
        int i = currentLine.indexActorTalking;
        actorTalking = matchedActorsList[i];


        //line.actorTalking.dialogueActorManager.isTalking = true;

        StartCoroutine(actorTalking.BubbleTalking(currentLine.dialogueLine));






        #endregion



    }

    #endregion

    #region Set Current Dialogue Line
    public void SetCurrentDialogueLine(int indexLine)
    {
        if (indexLine >= 0 && indexLine < dialogueLines.Count())
        {
            currentLine = dialogueLines[indexLine];


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
}
