using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SimpleNPCDialogueManager : MonoBehaviour
{
    [Header("Dialogue Components")]
    
    public DialoguePlayerManager dialoguePlayerManager;
    
    public DialogueActorManager playerActorManager;


    [Header("State")]
    public bool isTalking; 

    public TextMeshPro npcBubble;

    [Header("Simple NPC Dialogue")]
    public SimpleNPCDialogue simpleNPCDialogue;


    [Header("Dialogue Modes")]
    public bool talkOnProximity;
    public SimpleNPCDialogue proximityDialogue;

    public bool talkOffProximity;
    public SimpleNPCDialogue offDialogue;

    public bool talksWhenSeesPlayer;
    public SimpleNPCDialogue whenSeeingPlayerDialogue;


    
    public bool talksAlone;
    private bool talkingAlone;
    public float aloneTalkInterval; 
    public SimpleNPCDialogue aloneDialogue;


    [Header("Dialogue Settings")]

    public float timePerWords = 0.6f;
    public float timeToType; 
    public float timeForAutoChange = 1f;
    public float timePerLetter = 0.05f;

    public string typedLine;


    [Header("Audio and Animation Settings")]
    public AudioClip voice;
    private AudioSource audioSource;
    private Animator anim;





    // Start is called before the first frame update
    void Start()
    {
        if (Application.isPlaying)
        {
            npcBubble.text = "";

        }

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = voice;

        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (talksAlone && !talkingAlone)
        {
            StartCoroutine(TalkingAlone());
        }



    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (talkOnProximity)
            {
                Debug.Log("Hablo cuando se me acerca un jugador!");
            }
        
        }
          }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (talkOffProximity)
            {
                Debug.Log("Hablo cuando un jugador se aleja!");
            
            }
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

    #region Bubble Talking
    public IEnumerator BubbleTalking(string text)  // text es la linea de diálogo actual leída por el lector. 
    {
        
        int amountOfWords = CountWords(text); // Calculamos la cantidad de palabras
        float timeTalking = amountOfWords * timePerWords; // Calculamos la cantidad de tiempo que el personaje estará hablando
        // El texto de la burbuja de diálogo es el mismo que estará tipeado. 
        
        StartCoroutine(TypeSentence(text)); //Se comienza a tipear 
        // Debemos esperar a que se termine de tipear la palabra + un tiempo para lectura en función de la cantidad de palabras. 
        yield return new WaitForSeconds(timeToType + timePerWords);
        
        StartCoroutine(AutoNextLine());

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


    #region Typing Sentence

    IEnumerator TypeSentence(string sentence)
    {
        Debug.Log("Escribiendo la oración");
        npcBubble.text = ""; 
        timeToType = sentence.Length * timePerLetter;

        playerActorManager.isTalking = true;
        playerActorManager.animator.Play(playerActorManager.talkingAnimation.name); 

        foreach (char letter in sentence.ToCharArray())
        {
            typedLine += letter;
            npcBubble.text = typedLine;

            if (letter != ' ')
            {
                audioSource.Play();
            }
            yield return new WaitForSeconds(timePerLetter);
            
        }


        playerActorManager.isTalking = false;
        isTalking = false;
    }

    #endregion


    #region Automatic Display of the Next Line

    IEnumerator AutoNextLine()
    {
        Debug.Log("Esperando a pasar de linea automáticamente... ");
        yield return new WaitForSeconds(timeForAutoChange);
        if (playerActorManager.envolvedInDialogue && !isTalking)
        {
            Debug.Log("Pasando de línea ");
            dialoguePlayerManager.DisplayNextSignLine();
        }
    }

    #endregion


}
