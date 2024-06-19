
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SignManager))]
public class SignManagerEditor : Editor
{
    private Texture2D _logo2 = null;

    void OnEnable()
    {
        if (_logo2 == null)
        {

            _logo2 = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/DBR - Assets/DBR - Dialogue System/Editor/Graphics/Sign Manager.png", typeof(Texture2D));
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
