using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ActorNPCDialogueManager : MonoBehaviour
{
    public DialoguePlayerManager playerDialogueManager;
    public bool isTalking;

    public TextMeshPro npcBubble;

    public List<string> actorsInScene;
    public List<DialogueActorManager> matchedActorsList;

    public NPCDialogue npcDialogue;

    public bool talkOnProximity;
    public NPCDialogue proximityDialogue;

    public bool talkOffProximity;
    public NPCDialogue offDialogue;

    public bool talksWhenSeesPlayer;
    public NPCDialogue whenSeeingPlayerDialogue;

    public bool talksAlone;
    private bool talkingAlone;
    public float aloneTalkInterval;
    public NPCDialogue aloneDialogue;

    public float timePerWords;
    public float timeToType;
    public float timeForAutoChange;
    public float timePerLetter;

    public string typedLine;



  


    // Start is called before the first frame update
    void Start()
    {
        if (Application.isPlaying)
        {
            npcBubble.text = "";

        }

    
        FindAndAddActors();
    }

   void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (talksAlone && !talkingAlone)
        {
            StartCoroutine(TalkingAlone());
        }


       

    
    }

    #region Talking Alone
    public IEnumerator TalkingAlone()
    {
        talkingAlone = true;
        Debug.Log("I'm talking alone");
        yield return new WaitForSeconds(aloneTalkInterval);
        talkingAlone = false;
    }

    #endregion


    // This function will search through the scene for matching DialogueActorManager components
    public void FindAndAddActors()
    {
        
        // Clear the matched actors list to avoid duplicates
        matchedActorsList.Clear();

        // Get all root objects in the active scene
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();

        // Iterate through each actor name in the actorsInScene list
        foreach (string actorName in npcDialogue.actorsInScene)
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

    public void PassingTheActorsForSceneList()
    {
        Debug.Log("Pasando la lista de actores"); 
        foreach (DialogueActorManager actor in matchedActorsList)
        {
            playerDialogueManager.actorsInScene.Add(actor); 
        }
    }
}
