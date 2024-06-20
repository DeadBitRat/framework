using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueActorManager : MonoBehaviour
{
    #region Variables

    [Header("Dialogue Elements")]

    public TextMeshPro bubbleText;  // Reference to the TextMeshPro component that displays the dialogue text.

    public DBRDialogueSystem dialogueSystem;  // Reference to the dialogue system managing the dialogue flow.

    public InputDetector inputDetector;  // Reference to the input detector to manage user inputs.

    [Header("Actor Components")]

    public int actorID;  // Unique identifier for the actor.
    public string actorName;  // Name of the actor.
    public string actorType;  // Type or role of the actor.

    public AnimationClip talkingAnimation;  // Animation clip played when the actor is talking.
    public AnimationClip listeningAnimation;  // Animation clip played when the actor is listening.
    public Animator animator;  // Animator component to control animations.
    public AudioClip voice;  // Audio clip for the actor's voice.
    public AudioSource audioSource;  // Audio source component to play the voice.

    [Header("Dialogue Settings")]

    public float timePerLetter = 0.05f;  // Time taken to type each letter.
    public float timePerWords = 0.6f;  // Time taken to say each word.
    public float timeToType;  // Total time taken to type the dialogue line.
    public string typedLine;  // The line of dialogue currently being typed.
    public float timeForAutoChange = 1f;  // Time before automatically moving to the next line.

    [Header("Dialogue States")]
    public bool isIdle;  // Indicates if the actor is idle.
    public bool isActing;  // Indicates if the actor is performing an action.
    public bool isTalking;  // Indicates if the actor is talking.
    public bool isListening;  // Indicates if the actor is listening.
    public bool envolvedInDialogue;  // Indicates if the actor is involved in a dialogue.
    public bool readingSign;  // Indicates if the actor is reading a sign.
    public bool readingDescription;  // Indicates if the actor is reading a description.

    #endregion

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component attached to the actor, if it exists.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("There is no AudioSource added to " + actorName + "'s Actor Manager");
        }

        // Get the Animator component attached to the actor, if it exists.
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Set the audio source's clip to the actor's voice.
        audioSource.clip = voice;
    }

    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        // Update the animator parameters based on the actor's states.
        animator.SetBool("Idle", isIdle);
        animator.SetBool("Acting", isActing);
        animator.SetBool("Talking", isTalking);
        animator.SetBool("Listening", isListening);
    }

    #endregion

    #region General Bubble Talking Method

    // Coroutine to manage the actor's talking animation and dialogue bubble.
    public IEnumerator BubbleTalking(string text)
    {
        #region Pre-Talking Animation

        // Play pre-talking animation if it exists.
        if (dialogueSystem.currentLine.preTalkingAnimation != null)
        {
            isActing = true;
            animator.Play(dialogueSystem.currentLine.preTalkingAnimation.name, 0, 0.0f);
            yield return new WaitForSeconds(dialogueSystem.currentLine.preTalkingAnimation.length);
            isActing = false;
        }

        #endregion

        #region Saying the line

        if (text != "")
        {
            isTalking = true;

            #region Playing the Talking Animation Default or Specific

            // Play the specific or default talking animation.
            if (dialogueSystem.currentLine.actorTalkingClip != null)
            {
                animator.Play(dialogueSystem.currentLine.actorTalkingClip.name);
            }
            else if (talkingAnimation != null)
            {
                animator.Play(talkingAnimation.name);
            }

            #endregion

            // Calculate the time taken to say the line.
            int amountOfWords = CountWords(text);
            float timeTalking = amountOfWords * timePerWords;

            // Start typing the sentence.
            StartCoroutine(TypeSentence(text));
            yield return new WaitForSeconds(timeToType + timePerWords);
        }

        #endregion

        #region Post-Talking Animation

        // Play post-talking animation if it exists.
        if (dialogueSystem.currentLine.postTalkingAnimation != null)
        {
            isActing = true;
            animator.Play(dialogueSystem.currentLine.postTalkingAnimation.name, 0, 0.0f);
            yield return new WaitForSeconds(dialogueSystem.currentLine.postTalkingAnimation.length);
            isActing = false;
        }
        #endregion

        #region Waiting Time for the Next Line

        // Wait for a specific time before moving to the next line.
        yield return new WaitForSeconds(dialogueSystem.currentLine.secondsToNextLine);

        #endregion

        #region The post-Dialogue Event!

        // Invoke post-dialogue events if they exist.
        if (dialogueSystem.hayEventos)
        {
            dialogueSystem.currentEvents.events.postDialogueEvents?.Invoke();
        }

        #endregion

        #region Ending the Bubble Reading 

        // Check if the next line is the end of the dialogue.
        if (!dialogueSystem.dialogueLines[dialogueSystem.indexLine + 1].isEndOfDialogue)
        {
            dialogueSystem.indexLine++;
            dialogueSystem.DisplayNextActingDialogueLine();
        }
        else
        {
            dialogueSystem.EndDialogue();
        }

        #endregion
    }

    #endregion

    #region Typing Sentence Coroutine

    // Coroutine to type the sentence letter by letter.
    IEnumerator TypeSentence(string sentence)
    {
        Debug.Log("Typing the sentence");

        // Calculate the time taken to type the sentence.
        timeToType = sentence.Length * timePerLetter;
        foreach (char letter in sentence.ToCharArray())
        {
            typedLine += letter;
            bubbleText.text = typedLine;

            // Play the voice audio clip if the letter is not a space.
            if (letter != ' ')
            {
                if (voice != null)
                {
                    audioSource.Play();
                }
            }
            yield return new WaitForSeconds(timePerLetter);
        }

        isTalking = false;
    }

    #endregion

    #region Word Counting

    // Method to count the number of words in a string.
    public static int CountWords(string text)
    {
        text = text.Trim();

        if (text.Length == 0)
            return 0;

        string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return words.Length;
    }

    #endregion

    #region Automatic Display of the Next Line

    // Coroutine to automatically display the next line of dialogue.
    public IEnumerator AutoNextLine()
    {
        Debug.Log("Waiting to automatically move to the next line... ");
        yield return new WaitForSeconds(timeForAutoChange);

        // Additional logic for automatic line change can be added here.
    }

    #endregion
}
