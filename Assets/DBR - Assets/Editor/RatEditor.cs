using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Jump))]
public class RatEditor : Editor
{
    private Texture2D _logo = null;

    void OnEnable()
    {
        if (_logo == null)
        {
           
            _logo = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/DBR - Assets/Graphics/Logo/Favicon12.png", typeof(Texture2D));
        }
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Box(_logo, GUIStyle.none);
        EditorGUILayout.LabelField("Dead Bit Rats");
        
        GUILayout.EndVertical();

        base.OnInspectorGUI();
    }
}
