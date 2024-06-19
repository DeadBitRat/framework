using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileLocator))]
public class TileLocatorEditor : Editor
{
    private Texture2D _logo2 = null;

    void OnEnable()
    {
        if (_logo2 == null)
        {

            _logo2 = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/DBR - Assets/Editor/Graphics/TileLocator.png", typeof(Texture2D));
        }
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Box(_logo2, GUIStyle.none);
        EditorGUILayout.LabelField("Dead Bit Rats");

        GUILayout.EndVertical();

        base.OnInspectorGUI();

      
        TileLocator script = (TileLocator)target;

        GUIContent tileSize = new GUIContent("Tile Size");
        script.tileSizeIndex = EditorGUILayout.Popup(tileSize, script.tileSizeIndex, script.tileSize);
    }


}
