using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SquareCharacter : MonoBehaviour
{
    [SerializeField]
    public GridSystem gridSystem;
    public GridSquare currentSquare;
    public bool isWalking = false;
    public bool isIdle = true;
    public List<GridSquare> path = new List<GridSquare>();

    [SerializeField]
    private float speed = 5f;

    private bool initialized = false; // Flag to track initialization

    void Start()
    {
        if (gridSystem == null)
        {
            gridSystem = FindObjectOfType<GridSystem>();
            if (gridSystem == null)
            {
                Debug.LogError("GridSystem not found in the scene.");
            }
        }
    }

    void Update()
    {
        // Check if the grid has been generated
        if (!initialized && gridSystem != null && gridSystem.GetComponent<GridGenerator>().enabled == false)
        {
            InitializeCharacter();
            initialized = true;
        }

        // Detect changes in the grid
        if (gridSystem != null && gridSystem.GetComponent<GridGenerator>().enabled == false)
        {
            // Your logic to handle changes in the grid here...
        }

        // Update currentSquare based on character's position
        currentSquare = FindSquareAtPosition(transform.position);

        // Your existing update logic here...
        if (!gridSystem.editableGrid && Input.GetMouseButtonDown(0))
        {
            if (!isWalking)
            {
                Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.z = 0f;

                GridSquare targetSquare = FindSquareAtPosition(targetPosition);
                if (targetSquare != null && targetSquare.walkable)
                {
                    FindPath(currentSquare, targetSquare);
                    StartCoroutine(MoveAlongPath());
                }
            }
        }
    }

    void InitializeCharacter()
    {
        // Find the current square
        currentSquare = FindSquareAtPosition(transform.position);
        if (currentSquare == null)
        {
            // If the character is not positioned over a square, find the nearest square
            currentSquare = FindNearestSquare();
            if (currentSquare == null)
            {
                Debug.LogError("No grid squares found in the scene.");
                // You might want to handle this situation differently based on your game's requirements
                // For now, let's assume the character is placed at a default position
                currentSquare = FindObjectOfType<GridSquare>();
                if (currentSquare == null)
                {
                    Debug.LogError("No default grid square found in the scene.");
                }
                else
                {
                    Debug.LogWarning("Character placed over non-grid area. Moved to nearest square.");
                    transform.position = new Vector3(currentSquare.transform.position.x, currentSquare.transform.position.y, transform.position.z);
                }
            }
            else
            {
                Debug.LogWarning("Character placed over non-grid area. Moved to nearest square.");
                transform.position = new Vector3(currentSquare.transform.position.x, currentSquare.transform.position.y, transform.position.z);
            }
        }
        else
        {
            Debug.Log("Current square found: " + currentSquare.name);
        }

        if (currentSquare != null)
        {
            // Now that we have a valid current square, let's proceed with the rest of the initialization
            // (e.g., finding path, etc.)
        }
    }

    GridSquare FindSquareAtPosition(Vector3 position)
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(position);
        if (hitCollider != null)
        {
            return hitCollider.GetComponent<GridSquare>();
        }
        return null;
    }

    void FindPath(GridSquare startSquare, GridSquare targetSquare)
    {
        path.Clear();
        Queue<GridSquare> queue = new Queue<GridSquare>();
        Dictionary<GridSquare, GridSquare> cameFrom = new Dictionary<GridSquare, GridSquare>();
        HashSet<GridSquare> visited = new HashSet<GridSquare>();

        queue.Enqueue(startSquare);
        visited.Add(startSquare);

        while (queue.Count > 0)
        {
            GridSquare current = queue.Dequeue();
            if (current == targetSquare)
            {
                ReconstructPath(cameFrom, current);
                return;
            }

            foreach (GridSquare neighbor in GetNeighbors(current))
            {
                if (neighbor != null && neighbor.walkable && !visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        // If no path found, display warning
        Debug.LogWarning("There's no possible path");
    }

    List<GridSquare> GetNeighbors(GridSquare square)
    {
        List<GridSquare> neighbors = new List<GridSquare>();

        // Add neighboring squares (up, down, left, right)
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = new Vector2Int((int)square.position.x + dir.x, (int)square.position.y + dir.y);
            GridSquare neighborSquare = GetSquareAtPosition(neighborPos);
            if (neighborSquare != null && neighborSquare.walkable)
            {
                neighbors.Add(neighborSquare);
            }
        }

        return neighbors;
    }

    GridSquare GetSquareAtPosition(Vector2Int position)
    {
        GameObject squareObj = GameObject.Find("Square_" + position.x + "_" + position.y);
        if (squareObj != null)
        {
            return squareObj.GetComponent<GridSquare>();
        }
        return null;
    }

    void ReconstructPath(Dictionary<GridSquare, GridSquare> cameFrom, GridSquare current)
    {
        path.Clear();
        while (cameFrom.ContainsKey(current))
        {
            path.Insert(0, current);
            current = cameFrom[current];
        }
    }

    IEnumerator MoveAlongPath()
    {
        isWalking = true;
        isIdle = false;
        foreach (var square in path)
        {
            Vector3 targetPosition = new Vector3(square.transform.position.x, square.transform.position.y, transform.position.z);
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
            // Ensure the character reaches the exact target position
            transform.position = targetPosition;
            currentSquare = square;
        }
        isWalking = false;
        isIdle = true;
    }

    GridSquare FindNearestSquare()
    {
        GridSquare[] allSquares = FindObjectsOfType<GridSquare>();
        if (allSquares.Length == 0)
        {
            return null;
        }

        float minDistance = float.MaxValue;
        GridSquare nearestSquare = null;

        foreach (GridSquare square in allSquares)
        {
            float distance = Vector3.Distance(transform.position, square.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestSquare = square;
            }
        }

        return nearestSquare;
    }
}
