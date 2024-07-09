using UnityEngine;
using UnityEditor;

// Este atributo indica a Unity que use este editor personalizado para la clase DialogueLine
[CustomEditor(typeof(DialogueLine))]
public class DialogueLineEditor : Editor
{
    // Sobrescribe el método OnInspectorGUI para personalizar el diseño del inspector
    public override void OnInspectorGUI()
    {
        // Actualiza el objeto serializado para reflejar los cambios en el inspector
        serializedObject.Update();

        // Obtén referencias a las propiedades del objeto serializado
        SerializedProperty nameOfLineProp = serializedObject.FindProperty("nameOfLine");
        SerializedProperty indexActorTalkingProp = serializedObject.FindProperty("indexActorTalking");
        SerializedProperty actorsListeningProp = serializedObject.FindProperty("actorsListening");
        SerializedProperty preTalkingAnimationProp = serializedObject.FindProperty("preTalkingAnimation");
        SerializedProperty dialogueLineProp = serializedObject.FindProperty("dialogueLine");
        SerializedProperty actorTalkingActingClipProp = serializedObject.FindProperty("actorTalkingActingClip");
        SerializedProperty postTalkingAnimationProp = serializedObject.FindProperty("postTalkingAnimation");
        SerializedProperty secondsToNextLineProp = serializedObject.FindProperty("secondsToNextLine");
        SerializedProperty isQuestionProp = serializedObject.FindProperty("isQuestion");
        SerializedProperty optionsProp = serializedObject.FindProperty("options");
        SerializedProperty nextLineDiffersProp = serializedObject.FindProperty("nextLineDiffers");
        SerializedProperty nextLineProp = serializedObject.FindProperty("nextLine");
        SerializedProperty dialogueEventsNameProp = serializedObject.FindProperty("dialogueEventsName");
        SerializedProperty isEndOfDialogueProp = serializedObject.FindProperty("isEndOfDialogue");

        // Muestra el campo de propiedad 'isEndOfDialogue'
        EditorGUILayout.PropertyField(isEndOfDialogueProp);

        // Desactiva todas las propiedades si 'isEndOfDialogue' es verdadero
        GUI.enabled = !isEndOfDialogueProp.boolValue;

        // Muestra los campos de propiedad en el inspector
        EditorGUILayout.PropertyField(nameOfLineProp);
        EditorGUILayout.PropertyField(indexActorTalkingProp);
        EditorGUILayout.PropertyField(actorsListeningProp, true);
        EditorGUILayout.PropertyField(preTalkingAnimationProp);
        EditorGUILayout.PropertyField(dialogueLineProp);
        EditorGUILayout.PropertyField(actorTalkingActingClipProp);
        EditorGUILayout.PropertyField(postTalkingAnimationProp);
        EditorGUILayout.PropertyField(secondsToNextLineProp);
        EditorGUILayout.PropertyField(isQuestionProp);

        // Si 'isQuestion' es verdadero, muestra el campo de propiedad 'options' como una lista
        if (isQuestionProp.boolValue)
        {
            EditorGUILayout.PropertyField(optionsProp, true);
        }

        // Muestra el campo de propiedad 'nextLineDiffers'
        EditorGUILayout.PropertyField(nextLineDiffersProp);
        // Si 'nextLineDiffers' es verdadero, muestra el campo de propiedad 'nextLine'
        if (nextLineDiffersProp.boolValue)
        {
            EditorGUILayout.PropertyField(nextLineProp);
        }

        EditorGUILayout.PropertyField(dialogueEventsNameProp);

        // Vuelve a habilitar las propiedades
        GUI.enabled = true;

        // Aplica las propiedades modificadas al objeto serializado
        serializedObject.ApplyModifiedProperties();
    }
}

