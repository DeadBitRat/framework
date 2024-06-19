using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int columns = 10;
    public int rows = 10;
    public GameObject squarePrefab;
    public Vector2 originPoint;
    public string gridName = "GridContainer";

    void Start()
    {
        //GenerateGrid(columns, rows, originPoint);
    }

    public void GenerateGrid(int columns, int rows, Vector2 origin)
    {
        // Find or create a unique grid container name
        string uniqueGridName = gridName;
        int counter = 1;
        while (transform.Find(uniqueGridName) != null)
        {
            uniqueGridName = gridName + "_" + counter;
            counter++;
        }

        // Create the grid container
        GameObject gridContainerObject = new GameObject(uniqueGridName);
        gridContainerObject.transform.parent = this.transform;
        Transform gridContainer = gridContainerObject.transform;

        // Clear existing grid (if any)
        foreach (Transform child in gridContainer)
        {
            DestroyImmediate(child.gameObject);
        }

        // Generate new grid
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector2 position = new Vector2(origin.x + x * 0.16f, origin.y + y * 0.16f);
                GameObject newSquare = Instantiate(squarePrefab, position, Quaternion.identity);
                newSquare.transform.parent = gridContainer;
                newSquare.name = "Square_" + x + "_" + y;

                GridSquare square = newSquare.GetComponent<GridSquare>();
                if (square != null)
                {
                    square.position = new Vector2(x, y);
                    square.walkable = true;
                }
            }
        }
    }
}

