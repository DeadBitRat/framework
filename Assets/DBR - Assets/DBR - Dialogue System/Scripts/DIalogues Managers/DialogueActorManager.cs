using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueActorManager : MonoBehaviour
{
    #region Variables

    [Header("Dialogue Elements")]

    public TextMeshPro bubbleText;
    public Animator animator;
    public DBRDialogueSystem dialogueSystem;

    public InputDetector inputDetector;

    public int actorID;
    public string actorName;
    public string actorType;

    public AnimationClip talkingAnimation;
    public AnimationClip listeningAnimation;
    public AudioClip voice;
    public AudioSource audioSource;

    [Header("Dialogue Settings")]

    public float timePerLetter = 0.05f;
    public float timePerWords = 0.6f;
    public float timeToType;
    public string typedLine;
    public float timeForAutoChange = 1f;




    [Header("Dialogue States")]
    public bool isIdle;
    public bool isActing;
    public bool isTalking;
    public bool isListening;
    public bool envolvedInDialogue;
    public bool readingSign;
    public bool readingDescription; 

    #endregion

    #region Start

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("Theres no AudioSource addad to " + actorName + "'s Actor Manager");
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        audioSource.clip = voice;

        
    }

    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Idle", isIdle);
        animator.SetBool("Acting", isActing);
        animator.SetBool("Talking", isTalking);
        animator.SetBool("Listening", isListening);


    }

    #endregion

    #region General Bubble Talking Method

    public IEnumerator BubbleTalking(string text)  // text es la linea de diálogo actual leída por el lector. 
    {
       

        #region Pre-Talking Animation

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

            if (dialogueSystem.currentLine.actorTalkingClip != null)
            {
                animator.Play(dialogueSystem.currentLine.actorTalkingClip.name); ;
            }
            else if (talkingAnimation != null)
            {
                animator.Play(talkingAnimation.name);
            }

            #endregion

            

            int amountOfWords = CountWords(text); // Calculamos la cantidad de palabras
            float timeTalking = amountOfWords * timePerWords; // Calculamos la cantidad de tiempo que el personaje estará hablando
                                                              // El texto de la burbuja de diálogo es el mismo que estará tipeado. 
            StartCoroutine(TypeSentence(text)); //Se comienza a tipear 
                                                // Debemos esperar a que se termine de tipear la palabra + un tiempo para lectura en función de la cantidad de palabras. 

            yield return new WaitForSeconds(timeToType + timePerWords);

        }

        #endregion

        #region Post-Talking Animation

        if (dialogueSystem.currentLine.postTalkingAnimation != null)
        {
            isActing = true;
            animator.Play(dialogueSystem.currentLine.postTalkingAnimation.name, 0, 0.0f);
            Debug.Log("The animator is : " + animator);
            yield return new WaitForSeconds(dialogueSystem.currentLine.postTalkingAnimation.length);
            isActing = false;
        }
        #endregion

        #region Waiting Time for the Next Line

        yield return new WaitForSeconds(dialogueSystem.currentLine.secondsToNextLine);

        #endregion

        #region The post-Dialogue Event!

        if (dialogueSystem.hayEventos)
        {
            dialogueSystem.currentEvents.events.postDialogueEvents?.Invoke();
        }

        #endregion

        #region Ending the Bubble Reading 

        if (!dialogueSystem.dialogueLines[dialogueSystem.indexLine + 1].isEndOfDialogue)
        {
            dialogueSystem.indexLine = dialogueSystem.indexLine + 1;
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

    IEnumerator TypeSentence(string sentence)
    {
        Debug.Log("Escribiendo la oración");

        timeToType = sentence.Length * timePerLetter;
        foreach (char letter in sentence.ToCharArray())
        {
            typedLine += letter;
            bubbleText.text = typedLine;

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
    public static int CountWords(string text)
    {
        // Elimina los espacios en blanco al principio y al final del texto
        text = text.Trim();

        // Si el texto está vacío, retorna 0 palabras
        if (text.Length == 0)
            return 0;

        // Cuenta las palabras dividiendo el texto en espacios en blanco
        // y contando cuántos elementos no están vacíos
        string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return words.Length;
    }
    #endregion

    #region Automatic Display of the Next Line

    public IEnumerator AutoNextLine()
    {
        Debug.Log("Esperando a pasar de linea automáticamente... ");
        yield return new WaitForSeconds(timeForAutoChange);

        if (envolvedInDialogue && isTalking)
        {

        }
    }

    #endregion



}
