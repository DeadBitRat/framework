using UnityEngine;

public class DynamicSortingOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Set the sorting order based on the y-position of the object
        // Assuming a scale factor to adjust the sensitivity of the sorting order change
        int sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);

        // Apply the sorting order to the SpriteRenderer
        spriteRenderer.sortingOrder = sortingOrder;
    }
}

