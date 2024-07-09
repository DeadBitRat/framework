using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StoryEvents : MonoBehaviour
{
    private InputDetector inputDetector; 

    [Header("For Changing Dialogues for NPCs")]

    public DialogueAssignment[] dialogueAssignments;

    [Header("For Playing an Animation")]

    public AnimationAssignment[] animationAssignments;
    


    // Start is called before the first frame update
    void Start()
    {
        inputDetector = FindObjectOfType<InputDetector>();
        if (inputDetector != null )
        {
            Debug.Log("No se encontró Input Detector en esta escena"); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignNewDialogueToNPC(DialogueAssignment dialogue) 

    {
        foreach (var dialogueAssignment in dialogueAssignments)
        {
            //dialogue.npc.npcDialogue = dialogue.dialogue;
        }
        
    }

    public void PlayAnimation(AnimationAssignment animation)

    {
        foreach (var animationAssignment in animationAssignments)
        {
            animation.animator.Play(animation.animationClip.name); 
        }
    }

    public void TurnOffControlsForSeconds(int seconds)
    {

    }

    public void DebugLog(string message)
    {
        Debug.Log(message); 
    }
   

}
[System.Serializable]
public class DialogueAssignment
{
    public NPCDialogue dialogue;
    public ActorNPCDialogueManager npc; 
}

[System.Serializable]
public class AnimationAssignment
{
    public AnimationClip animationClip;
    public Animator animator; 
}
