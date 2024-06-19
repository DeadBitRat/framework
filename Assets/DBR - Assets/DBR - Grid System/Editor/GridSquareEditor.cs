using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridSquare))]
public class GridSquareEditor : Editor
{
    void OnSceneGUI()
    {
        if (Application.isPlaying) return; // Ensure this only runs in edit mode

        GridSquare gridSquare = (GridSquare)target;

        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null && hit.collider.gameObject == gridSquare.gameObject)
            {
                gridSquare.OnClicked();
                // Mark the object as dirty to save changes
                EditorUtility.SetDirty(gridSquare);
            }
        }
    }
}
