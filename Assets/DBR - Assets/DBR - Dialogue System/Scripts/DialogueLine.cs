using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DialogueOption
{
    public string optionText;
    public int nextLineIndex; // Index of the next dialogue line if this option is selected
}

[Serializable]
public class DialogueLine
{
    [Header("Basic Information")]
    public string nameOfLine;
    public int indexActorTalking;

    [Header("Actors")]
    public List<ActorListening> actorsListening;

    [Header("Animations")]
    public AnimationClip preTalkingAnimation;

    [TextArea(3, 10)]
    [Tooltip("The line of dialogue to be displayed.")]
    public string dialogueLine;

    [Tooltip("If blank, then plays the default talking animation")]
    public AnimationClip actorTalkingClip;

    public AnimationClip postTalkingAnimation;

    [Header("Timing")]
    [Tooltip("The number of seconds to wait before moving to the next line.")]
    public float secondsToNextLine;

    [Header("Dialogue Options")]
    public bool isQuestion;
    public DialogueOption[] options; // Options for questions

    [Header("Dialogue Flow")]
    public bool nextLineDiffers;
    public int nextLine;

    public bool isEndOfDialogue;

    [Header("Events")]
    [Space(10)]

    public string dialogueEventsName;

}

[Serializable]
public class ActorListening
{
    public string actorName;
    public AnimationClip actorListeningClip;
}
