using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DBRDialogueTrigger))]
public class DBRDialogueTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DBRDialogueTrigger script = (DBRDialogueTrigger)target;
        if (GUILayout.Button("Play Scene"))
        {
            script.PlayScene();
        }
    }
}
