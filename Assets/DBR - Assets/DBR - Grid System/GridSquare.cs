
using UnityEngine;

[ExecuteInEditMode]
public class GridSquare : MonoBehaviour
{
    public GridSystem gridSystem;
    public Vector2 position;
    public bool walkable;

    private SpriteRenderer spriteRenderer;
    private bool isRed = false;

    private Collider2D boxCollider;

    private string sortingLayerName = "Guides for Editing";
    void Start()
    {
        // Verificar si la sorting layer existe, si no, crearla
        if (!SortingLayerExists(sortingLayerName))
        {
            AddSortingLayer(sortingLayerName);
        }

        // Asignar el objeto actual a la sorting layer
        SetSortingLayer(gameObject, sortingLayerName);

        gameObject.tag = "GridSquare"; 
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Adding the click event
        boxCollider = GetComponent<Collider2D>();
        //boxCollider = new Vector2(0.16f, 0.16f);
        boxCollider.isTrigger = true;

        // Find the GridSystem object
        gridSystem = FindObjectOfType<GridSystem>();
        if (gridSystem == null)
        {
            Debug.LogError("GridSystem not found in the scene.");
        }
    }

    void Update()
    {
        
        
        if (Application.isEditor && !Application.isPlaying)
        {
           
            spriteRenderer.enabled = true;
           
           
            SetSortingLayer("Default");

            if (!walkable)
            {

                SetColorAndTransparency(Color.red, 0.25f);

            }

            else
            {
                SetColorAndTransparency(Color.green, 0.25f);
            }
        }
        else
        {
            spriteRenderer.enabled = false;

        }
    }

    public void OnMouseDown()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            Debug.Log("The square position (" + position.x + "," + position.y + ") is being pressed");
            OnClicked();
        }
    }

    public void OnClicked()
    {
        Debug.Log($"Object {gameObject.name} was clicked!");
        ToggleWalkability();
    }

    public void ToggleWalkability()
    {
        // Toggle color
        isRed = !isRed;
        spriteRenderer.color = isRed ? Color.red : Color.white;
        walkable = !isRed;
    }


    bool SortingLayerExists(string layerName)
    {
        foreach (var layer in SortingLayer.layers)
        {
            if (layer.name == layerName)
            {
                return true;
            }
        }
        return false;
    }

    void AddSortingLayer(string layerName)
    {
        // Unity no permite agregar Sorting Layers en tiempo de ejecución,
        // por lo tanto, esto debe hacerse en el Editor.
        Debug.LogWarning("Sorting Layer '" + layerName + "' does not exist. Please add it manually in the Tags and Layers settings.");
    }

    void SetSortingLayer(GameObject obj, string layerName)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sortingLayerName = layerName;
        }
        else
        {
            Debug.LogWarning("No Renderer component found on this GameObject. Cannot set sorting layer.");
        }
    }


    private void SetColorAndTransparency(Color color, float alpha)
    {
        color.a = alpha;
        spriteRenderer.color = color;
    }

    private void SetSortingLayer(string layerName)
    {
        spriteRenderer.sortingLayerName = layerName;
    }
}
