using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputDetector))]
public class InputDetectorEditor : Editor
{
    private Texture2D _logo2 = null;

    void OnEnable()
    {
        if (_logo2 == null)
        {

            _logo2 = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/DBR - Assets/Editor/Graphics/IconInputDetector.png", typeof(Texture2D));
        }
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Box(_logo2, GUIStyle.none);
        EditorGUILayout.LabelField("Dead Bit Rats");

        GUILayout.EndVertical();

        base.OnInspectorGUI();
    }
}