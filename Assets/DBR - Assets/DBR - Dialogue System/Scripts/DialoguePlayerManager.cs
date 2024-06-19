using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

using System.Linq;

[ExecuteInEditMode]
public class DialoguePlayerManager : MonoBehaviour
{
    [Header("DBR General System Elements")]
    public FunctionDetector functionDetector;
    public InputDetector inputDetector;
   
    public CharacterStates states;

    [Header("DBR Dialogue System Elements")]
    public DBRDialogueSystem dialogueSystem;
    public DialogueActorManager dialogueActorManager;
    [HideInInspector]
    public ActorNPCDialogueManager nPCDialogueManager; 

    

    public GameObject signDetected;

    public bool readingSign;
    public bool readingDescription; 

    public bool envolvedInDialogue;

    public TextMeshPro bubbleText;
    public string typedLine;

   

    public float timePerWords;
    public float timePerLetter;
    private float timeToType;
    public float timeToRead;
    public float timeForAutoChange;
    public float dialogueRestTime;
    public float timeUntilCanTalk; 



    public List<DialogueActorManager> actorsInScene;
    public Queue<SignLine> signSentences;
    public Queue<SimpleDialogueLine> dialogueSentences;
    public DialogueLine[] dialogueLines;
    
    public Queue<DescriptorLine> descriptionSentences;




    public DialogueLine currentLine;
    public DialogueActorManager currentActor; 

    public int indexLine = 0; 

    public Cutscene cutscene; 




    public AudioClip voice;
    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        if (Application.isPlaying)
        {
            bubbleText.text = "";
        }

        dialogueActorManager = GetComponent<DialogueActorManager>();

        signSentences = new Queue<SignLine>();
        dialogueSentences = new Queue<SimpleDialogueLine>();
        descriptionSentences = new Queue<DescriptorLine>();

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = voice;

      

    }

    // Update is called once per frame
    void Update()
    {
        #region Time Until Character can Read or Talk

        // This prevents players from erroneously pressing an NPC or an object to read it or talk to it at the end of the dialogue. 

        if (timeUntilCanTalk > 0)
        {
            timeUntilCanTalk -= Time.deltaTime;
        }

        else
        {
            timeUntilCanTalk = 0;
        }


        #endregion

    

        #region Bubble Orientation

        if (states.orientation == "Left")
        {
            bubbleText.transform.localScale = new Vector3(-1f,1f,1f);
        }

        else
        {
            bubbleText.transform.localScale = new Vector3(1f, 1f, 1f);
        }


            #endregion

            #region Bubble Position

            //transform.position = bubblePosition.position;

            #endregion

            #region Text Bubble in Editor 

            if (Application.isEditor && !Application.isPlaying)
        {

            if (dialogueActorManager.isTalking)
            {
                bubbleText.text = "Hola, soy una inocente burbuja de dialogo";
            }

            else
            {
                bubbleText.text = "";
            }

        }

        #endregion

        #region Sign Detected!

        if (functionDetector.sign != null && states.isIdle && timeUntilCanTalk <= 0)
        {
            if (inputDetector.functionPressed && !readingSign)
            {

                ReadSign(functionDetector.sign.signDialogue);

            }

            else if (readingSign && !dialogueActorManager.isTalking)
            {
                if (inputDetector.functionPressed)

                {
                    //StopCoroutine(AutoNextLine());
                    StopAllCoroutines();
                    DisplayNextSignLine();
                }
            }
        }

        #endregion


        #region Descriptor Detected

        if (functionDetector.descriptor != null && states.isIdle && !readingDescription && timeUntilCanTalk <= 0)
        {
            
            if (inputDetector.functionPressed && !states.isTalking)
            {

                ReadingDescriptor(functionDetector.descriptor.descriptionDialogue);

            }

            else if (readingDescription && !dialogueActorManager.isTalking)
            {
                if (inputDetector.functionPressed)

                {
                    //StopCoroutine(AutoNextLine());
                    StopAllCoroutines();
                    DisplayNextSignLine();
                }
            }

            
        }



        #endregion

        #region Simple NPC Detected! Let's Talk to the person. 

        if (functionDetector.simpleNPCDialogueManager != null && states.isIdle && timeUntilCanTalk <= 0)
        {
            


            if (inputDetector.functionPressed && !envolvedInDialogue)
            {
                // Do something when the F key is pressed


                //ReadSign(functionDetector.sign.signDialogue);
                TalkToSimpleNPC(functionDetector.simpleNPCDialogueManager.npcDialogue);

                // Add your own logic here
            }

            else if (envolvedInDialogue && !dialogueActorManager.isTalking && !functionDetector.nPCActorManager.isTalking)
            {
                if (inputDetector.functionPressed)

                {
                    StopCoroutine(AutoNextLine());
                    StopAllCoroutines();
                    DisplayNextSimpleDialogueLine();
                }
            }
        }

        #endregion

        #region Actor NPC Detected! Let's talk it the person!. 

        if (functionDetector.actorNPCDialogueManager != null && states.isIdle && timeUntilCanTalk <= 0)
        {


            // Starting the dialogue
            if (inputDetector.functionPressed && !envolvedInDialogue)
            {
                // Do something when the F key is pressed
                
                indexLine = 0; 
                TalkToActorNPC(functionDetector.actorNPCDialogueManager.npcDialogue);
                

                // Add your own logic here
            }

            // Moving to the Next Line or Finishing the Dialogue
            else if (envolvedInDialogue && !currentActor.isTalking)
            {
                if (inputDetector.functionPressed)

                {
                    Debug.Log("Vamos a mostrar la siguiente linea!"); 
                    DisplayNextActingDialogueLine();


                }
            }
        }



        #endregion

    }

    #region Player Bubble Talking

    public IEnumerator PlayerBubbleTalking(string text)  // text es la linea de diálogo actual leída por el lector. 
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

    #region Sign Reading

    public void ReadSign(SignDialogue signText)
    {
        signSentences.Clear();
        inputDetector.inputActivated = false;
        foreach (SignLine line in signText.lines)
        {
            signSentences.Enqueue(line);
        }

        DisplayNextSignLine();
    }

    #endregion

    #region Reading Descriptor

    public void ReadingDescriptor(DescriptorDialogue description)
    {
        dialogueActorManager.readingDescription = true; 
        descriptionSentences.Clear();
        inputDetector.inputActivated = false;
        foreach (DescriptorLine line in description.descriptionLines)
        {
            descriptionSentences.Enqueue(line);
        }
        readingDescription = true; 
        DisplayNextDescriptionLine();
    }

    #endregion




    #region Talking to Simple NPC

    public void TalkToSimpleNPC(SimpleNPCDialogue npcDialogue)
    {
        // Clear the current dialogue sentences queue to start fresh.
        dialogueSentences.Clear();

        // Loop through each SimpleDialogueLine in the NPCDialogue's line array.
        foreach (SimpleDialogueLine line in npcDialogue.sentences)
        {
            // Enqueue each dialogue line into the dialogueSentences queue.
            dialogueSentences.Enqueue(line);
        }

        // After enqueueing all dialogue sentences, display the next dialogue line in the queue.
        DisplayNextSimpleDialogueLine();
    }


    #endregion


    #region Talking to Actor NPC

    public void TalkToActorNPC(NPCDialogue npcDialogue)
    {
        //Getting the actors for the Dialogue; 
        Debug.Log("Solicitando la lista de actores"); 
        functionDetector.actorNPCDialogueManager.PassingTheActorsForSceneList();

        
        // Clear the current dialogue sentences queue to start fresh.
        dialogueLines = null;
        dialogueSystem.dialogueLines = null; 

        /*
        // Loop through each SimpleDialogueLine in the NPCDialogue's line array.
        foreach (SimpleDialogueLine line in npcDialogue.line)
        {
            // Enqueue each dialogue line into the dialogueSentences queue.
            dialogueSentences.Enqueue(line);
        }
        */

        // Initialize dialogueLines array with the same length as npcDialogue.lines
        dialogueLines = new DialogueLine[npcDialogue.lines.Length];

        // Use a for loop instead of foreach to copy the elements
        for (int i = 0; i < npcDialogue.lines.Length; i++)
        {
            dialogueLines[i] = npcDialogue.lines[i];
            
        }

        dialogueSystem.dialogueLines = dialogueLines;

        envolvedInDialogue = true;
        inputDetector.SwitchInputOff();

        DisplayNextActingDialogueLine();




        // After enqueueing all dialogue sentences, display the next dialogue line in the queue.
        //DisplayNextSimpleDialogueLine();
    }


    #endregion


    #region Displaying Next Line on the Sign Dialogue

    public void DisplayNextSignLine()
    {
        // Los tres siguientes son para limpiar los textos tanto del lector como del letrero. 
        bubbleText.text = "";
        typedLine = "";
        functionDetector.sign.signBubble.text = "";

        // Si se acabaron las lineas del diàlogo, entonces se acaba el diàlogo. 
        if (signSentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        readingSign = true;
        inputDetector.SwitchInputOff();  //Para que el jugador no se pueda mover. 
        SignLine sentence = signSentences.Dequeue();

        if (sentence.readByReader == true)
        {
            dialogueActorManager.isTalking = true;

            if (dialogueActorManager.isTalking)
            {
                StartCoroutine(PlayerBubbleTalking(sentence.dialogueLine));  // To start the talking animation
            }
        }
        else // El caso en que el diàlogo es mostrado por el letrero. 
        {
            functionDetector.sign.signBubble.text = sentence.dialogueLine;
        }
    }

    #endregion


    #region Displaying Next Line on the Description Dialogue

    public void DisplayNextDescriptionLine()
    {
        // Los tres siguientes son para limpiar los textos tanto del lector como del letrero. 
        bubbleText.text = "";
        typedLine = "";
       

        // Si se acabaron las lineas del diàlogo, entonces se acaba el diàlogo. 
        if (descriptionSentences.Count == 0)
        {
            readingDescription = false;
            EndDialogue();
            return;
        }
        readingDescription = true;
        inputDetector.SwitchInputOff();  //Para que el jugador no se pueda mover. 
        DescriptorLine sentence = descriptionSentences.Dequeue();

        StartCoroutine(PlayerBubbleTalking(sentence.dialogueLine));  // To start the talking animation
            
        
       
    }

    #endregion


    #region Display Next Line on the Dialogue with a Simple NPC

    public void DisplayNextSimpleDialogueLine()
    {
        // Los tres siguientes son para limpiar los textos tanto del lector como del letrero. 
        bubbleText.text = "";
        typedLine = "";

        functionDetector.simpleNPCDialogueManager.npcBubble.text = "";
        functionDetector.simpleNPCDialogueManager.typedLine = "";

        if (dialogueSentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        envolvedInDialogue = true;
        inputDetector.SwitchInputOff();

        SimpleDialogueLine sentence = dialogueSentences.Dequeue();

        #region Actor reading line

        if (sentence.lineByPlayer)
        {
            dialogueActorManager.isTalking = true;

            if (dialogueActorManager.isTalking)
            {
                StartCoroutine(PlayerBubbleTalking(sentence.dialogueLine));  // To start the talking animation
            }
        }
        else // El caso en que la línea la dice el NPC
        {
            functionDetector.nPCActorManager.isTalking = true;

            if (functionDetector.nPCActorManager.isTalking)
            {
                StartCoroutine(functionDetector.simpleNPCDialogueManager.BubbleTalking(sentence.dialogueLine));
            }
            //functionDetector.sign.signBubble.text = sentence.dialogueLine;
        }



        #endregion


    }

    #endregion

    #region Display Next Line on the Dialogue with an Actor NPC

    public void DisplayNextActingDialogueLine()
    {
        // Los tres siguientes son para limpiar los textos tanto del lector como del letrero. 
        foreach(DialogueActorManager actor in actorsInScene)
        {
            actor.bubbleText.text = "";
            actor.typedLine = ""; 
        }
      

        
       





        SetCurrentDialogueLine(indexLine);

        if (currentLine.isEndOfDialogue)
        {
            Debug.Log("Finalizando el diálogo");
            EndDialogue();
            return;
        }

        currentActor = actorsInScene[currentLine.indexActorTalking];
        dialogueSystem.currentActor = actorsInScene[currentLine.indexActorTalking];


        // Clearing Bubbles. 
        //line.actorTalking.bubbleText.text = ""; 

        //foreach (ActorListening actorListening in line.actorsListening)
        //{
        //   actorListening.actorListening.bubbleText.text = "";
        //}


        #region Actor reading line

        // Identify who is the actor talking this line. 
        int i = currentLine.indexActorTalking; 
        dialogueSystem.actorTalking = actorsInScene[i];
        DialogueActorManager actorTalking = dialogueSystem.actorTalking;

        //line.actorTalking.dialogueActorManager.isTalking = true;
        Debug.Log("La linea a leer es: " + currentLine.dialogueLine); 
        StartCoroutine(actorTalking.BubbleTalking(currentLine.dialogueLine));
        

        indexLine = indexLine + 1;

        if (envolvedInDialogue && !dialogueActorManager.isTalking)
        {
            Debug.Log("Listo para la siguiente línea!"); 
        }
        /*

        if (sentence.lineByPlayer)
        {
            dialogueActorManager.isTalking = true;

            if (dialogueActorManager.isTalking)
            {
                StartCoroutine(BubbleTalking(sentence.dialogueLine));  // To start the talking animation
            }
        }
        
        
        
        else // El caso en que la línea la dice el NPC
        {
            functionDetector.simpleNPC.dialogueActorManager.isTalking = true;

            if (functionDetector.simpleNPC.dialogueActorManager.isTalking)
            {
                StartCoroutine(functionDetector.simpleNPC.BubbleTalking(sentence.dialogueLine));
            }
            //functionDetector.sign.signBubble.text = sentence.dialogueLine;
        }
        */

        #endregion
    }

    #endregion

  



    #region General Bubble Talking

    public IEnumerator BubbleTalking(string text)  // text es la linea de diálogo actual leída por el lector. 
    {
        Debug.Log("Bubble Talking!- Inicio");
        int amountOfWords = CountWords(text); // Calculamos la cantidad de palabras
        float timeTalking = amountOfWords * timePerWords; // Calculamos la cantidad de tiempo que el personaje estará hablando
        // El texto de la burbuja de diálogo es el mismo que estará tipeado. 
        StartCoroutine(TypeSentence(text)); //Se comienza a tipear 
        // Debemos esperar a que se termine de tipear la palabra + un tiempo para lectura en función de la cantidad de palabras. 
        yield return new WaitForSeconds(timeToType + timePerWords);

        StartCoroutine(AutoNextLine());
        Debug.Log("Bubble Talking!- final!");
    }

    #endregion





    #region Typing Sentence

    IEnumerator TypeSentence(string sentence)
    {
        
        dialogueActorManager.isTalking = true;
        timeToType = sentence.Length * timePerLetter;
        foreach (char letter in sentence.ToCharArray())
        {
            typedLine += letter;
            bubbleText.text = typedLine;

            if (letter != ' ')
            {
                audioSource.Play();
            }
            yield return new WaitForSeconds(timePerLetter);
        }
        

        dialogueActorManager.isTalking = false;
    }

    #endregion

    #region Ending Dialogue

    void EndDialogue()
    {
        timeUntilCanTalk = dialogueRestTime; 

       
        readingSign = false;
        readingDescription = false;
        envolvedInDialogue = false;

        dialogueActorManager.readingSign = false;
        dialogueActorManager.envolvedInDialogue = false;
        dialogueActorManager.readingDescription = false;
        inputDetector.SwitchInputOn();

        foreach (DialogueActorManager actor in actorsInScene)
        {
            actor.bubbleText.text = "";
            actor.typedLine = "";
        }


       
        Debug.Log("End of dialogue");
    }

    #endregion


    #region Automatic Display of the Next Line

    public IEnumerator AutoNextLine()
    {
        Debug.Log("Esperando a pasar de linea automáticamente... ");
        yield return new WaitForSeconds(timeForAutoChange);
        if (readingSign && !dialogueActorManager.isTalking)
        {
            
            DisplayNextSignLine();
        }

        if (envolvedInDialogue && !dialogueActorManager.isTalking)
        {
            DisplayNextSimpleDialogueLine();
        }
        
        if (readingDescription && !dialogueActorManager.isTalking)
        {
            
            DisplayNextDescriptionLine();
        }

    }

    #endregion


    #region Set Current Dialogue Line

    public void SetCurrentDialogueLine(int indexLine)
    {
        if (indexLine >= 0 && indexLine < dialogueLines.Count())
        {
            currentLine = dialogueLines[indexLine];
            dialogueSystem.currentLine = dialogueLines[indexLine];

        }
        else
        {
            HandleOutOfRangeIndex();
        }
    }



    private void HandleOutOfRangeIndex()
    {
        EndDialogue(); 
    }

    #endregion
}

