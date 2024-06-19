using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(DBRTopDownFloor))]
public class DBRTopDownFloorEditor : Editor
{
    private Texture2D _logo2 = null;

    void OnEnable()
    {
        if (_logo2 == null)
        {

            _logo2 = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/DBR - Assets/Editor/Graphics/TopDownFloor.png", typeof(Texture2D));
        }
    }


    public override void OnInspectorGUI()
    {


        GUILayout.BeginVertical();
        GUILayout.Box(_logo2, GUIStyle.none);
        EditorGUILayout.LabelField("Dead Bit Rats");

        GUILayout.EndVertical();

        base.OnInspectorGUI();

        DBRTopDownFloor script = (DBRTopDownFloor)target;

        GUIContent tileSize = new GUIContent("Tile Size");
        script.tileSizeIndex = EditorGUILayout.Popup(tileSize, script.tileSizeIndex, script.tileSize);




    }



}

