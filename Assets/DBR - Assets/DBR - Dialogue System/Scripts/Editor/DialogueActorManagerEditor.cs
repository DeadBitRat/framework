using UnityEngine;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(DialogueActorManager))]
public class DialogueActorManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DialogueActorManager dialogueActorManager = (DialogueActorManager)target;

        EditorGUILayout.LabelField("Dialogue Elements", EditorStyles.boldLabel);
        dialogueActorManager.bubbleText = (TextMeshPro)EditorGUILayout.ObjectField("Bubble Text", dialogueActorManager.bubbleText, typeof(TextMeshPro), true);
        dialogueActorManager.dialogueSystem = (DBRDialogueSystem)EditorGUILayout.ObjectField("Dialogue System", dialogueActorManager.dialogueSystem, typeof(DBRDialogueSystem), true);
        dialogueActorManager.inputDetector = (InputDetector)EditorGUILayout.ObjectField("Input Detector", dialogueActorManager.inputDetector, typeof(InputDetector), true);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Actor Components", EditorStyles.boldLabel);
        dialogueActorManager.actorID = EditorGUILayout.IntField("Actor ID", dialogueActorManager.actorID);
        dialogueActorManager.actorName = EditorGUILayout.TextField("Actor Name", dialogueActorManager.actorName);
        dialogueActorManager.actorType = EditorGUILayout.TextField("Actor Type", dialogueActorManager.actorType);
        dialogueActorManager.talkingAnimation = (AnimationClip)EditorGUILayout.ObjectField("Talking Animation", dialogueActorManager.talkingAnimation, typeof(AnimationClip), true);
        dialogueActorManager.listeningAnimation = (AnimationClip)EditorGUILayout.ObjectField("Listening Animation", dialogueActorManager.listeningAnimation, typeof(AnimationClip), true);
        dialogueActorManager.animator = (Animator)EditorGUILayout.ObjectField("Animator", dialogueActorManager.animator, typeof(Animator), true);
        dialogueActorManager.voice = (AudioClip)EditorGUILayout.ObjectField("Voice", dialogueActorManager.voice, typeof(AudioClip), true);
        dialogueActorManager.audioSource = (AudioSource)EditorGUILayout.ObjectField("Audio Source", dialogueActorManager.audioSource, typeof(AudioSource), true);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Dialogue Settings", EditorStyles.boldLabel);
        dialogueActorManager.timePerLetter = EditorGUILayout.FloatField("Time Per Letter", dialogueActorManager.timePerLetter);
        dialogueActorManager.timePerWords = EditorGUILayout.FloatField("Time Per Words", dialogueActorManager.timePerWords);
        dialogueActorManager.timeToType = EditorGUILayout.FloatField("Time To Type", dialogueActorManager.timeToType);
        dialogueActorManager.typedLine = EditorGUILayout.TextField("Typed Line", dialogueActorManager.typedLine);
        dialogueActorManager.timeForAutoChange = EditorGUILayout.FloatField("Time For Auto Change", dialogueActorManager.timeForAutoChange);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Dialogue States", EditorStyles.boldLabel);
        dialogueActorManager.isIdle = EditorGUILayout.Toggle("Is Idle", dialogueActorManager.isIdle);
        dialogueActorManager.isActing = EditorGUILayout.Toggle("Is Acting", dialogueActorManager.isActing);
        dialogueActorManager.isTalking = EditorGUILayout.Toggle("Is Talking", dialogueActorManager.isTalking);
        dialogueActorManager.isListening = EditorGUILayout.Toggle("Is Listening", dialogueActorManager.isListening);
        dialogueActorManager.envolvedInDialogue = EditorGUILayout.Toggle("Envolved In Dialogue", dialogueActorManager.envolvedInDialogue);
        dialogueActorManager.readingSign = EditorGUILayout.Toggle("Reading Sign", dialogueActorManager.readingSign);
        dialogueActorManager.readingDescription = EditorGUILayout.Toggle("Reading Description", dialogueActorManager.readingDescription);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(dialogueActorManager);
        }
    }
}
