using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridGenerator))]
public class GridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridGenerator gridGenerator = (GridGenerator)target;
        if (GUILayout.Button("Generate Grid"))
        {
            gridGenerator.GenerateGrid(gridGenerator.columns, gridGenerator.rows, gridGenerator.originPoint);
        }
    }
}
