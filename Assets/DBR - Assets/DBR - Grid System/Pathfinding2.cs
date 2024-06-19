using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding2 : MonoBehaviour
{
    // Is Pathfinding Active? 
    public bool pathfindingActive;

    // Reference to the grid system
    [SerializeField]
    private GridSystem gridSystem;
    // Layer mask for the "Pathfinding Grid" layer
    public LayerMask pathfindingGridLayer;

    // Input Detector
    private InputDetector inputDetector;

    // The Horizontal Movement Controller
    public HorizontalMovement horizontalMovement;

    // The current square the NPC is on
    public GridSquare currentSquare;

    // Indicates if the NPC is currently walking
    public bool isWalking = false;

    // The path the NPC will follow
    public List<GridSquare> path = new List<GridSquare>();

    // The target position to move towards
    public Vector3 targetPosition;

    // Speed of the NPC movement, adjustable in the inspector
    [SerializeField]
    public float speed = 5f;

    // Flag to track if the character has been initialized
    public bool initialized = false;

    // Coroutine handle for the movement
    public Coroutine moveCoroutine;

    public CharacterStates states;

    // Controls whether the character moves on click
    public bool clickMovementActivated = true;

    // Reference to the CapsuleCollider2D
    private CapsuleCollider2D capsuleCollider;

    void Start()
    {
        // Find the GridSystem object if it's not assigned
        if (gridSystem == null)
        {
            gridSystem = FindObjectOfType<GridSystem>();
            if (gridSystem == null)
            {
                Debug.LogError("GridSystem not found!");
            }
        }

        if (CompareTag("Player"))
        {
            inputDetector = transform.parent.GetComponent<InputDetector>();
        }

        // Get the CapsuleCollider2D component
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        if (capsuleCollider == null)
        {
            Debug.LogError("CapsuleCollider2D component not found!");
        }
    }

    void Update()
    {
        if (pathfindingActive && inputDetector.inputActivated)
        {
            // Continuously find the current square based on the NPC's position
            currentSquare = FindSquareAtPosition(transform.position);

            // Check if the grid has been generated and initialize the character
            if (!initialized && gridSystem != null && !gridSystem.GetComponent<GridGenerator>().enabled)
            {
                initialized = true;
                InitializeCharacter();
            }

            // Handle player input for setting a new target position
            if (clickMovementActivated && !gridSystem.editableGrid && Input.GetMouseButtonDown(0))
            {
                // Get the target position from the mouse click
                Vector3 newTargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                newTargetPosition.z = 0f;

                SetNewTarget(newTargetPosition);
            }
        }
    }

    void InitializeCharacter()
    {
        // Find the current square based on the NPC's position
        if (currentSquare == null)
        {
            // If the NPC is not positioned over a square, find the nearest square
            currentSquare = FindNearestSquare();
            if (currentSquare != null)
            {
                transform.position = new Vector3(currentSquare.transform.position.x, currentSquare.transform.position.y, transform.position.z);
            }
        }
    }

    GridSquare FindSquareAtPosition(Vector3 position)
    {
        // Find the GridSquare at the given position
        Collider2D hitCollider = Physics2D.OverlapPoint(position, pathfindingGridLayer);
        if (hitCollider != null && hitCollider.CompareTag("GridSquare"))
        {
            return hitCollider.GetComponent<GridSquare>();
        }
        return null;
    }

    void FindPath(GridSquare startSquare, GridSquare targetSquare)
    {
        // Clear the current path
        path.Clear();

        // Initialize data structures for pathfinding
        Queue<GridSquare> queue = new Queue<GridSquare>();
        Dictionary<GridSquare, GridSquare> cameFrom = new Dictionary<GridSquare, GridSquare>();
        HashSet<GridSquare> visited = new HashSet<GridSquare>();

        // Start the pathfinding from the start square
        queue.Enqueue(startSquare);
        visited.Add(startSquare);

        while (queue.Count > 0)
        {
            GridSquare current = queue.Dequeue();
            if (current == targetSquare)
            {
                // If the target square is reached, reconstruct the path
                ReconstructPath(cameFrom, current);
                return;
            }

            // Process each neighbor of the current square
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
    }

    List<GridSquare> GetNeighbors(GridSquare square)
    {
        List<GridSquare> neighbors = new List<GridSquare>();

        // Add neighboring squares (up, down, left, right, diagonal)
        Vector2Int[] directions = {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
            new Vector2Int(-1, 1), new Vector2Int(1, 1), new Vector2Int(-1, -1), new Vector2Int(1, -1)
        };

        // Find the neighbor squares based on the given directions
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
        // Find the GridSquare at the given grid position
        GameObject squareObj = GameObject.Find("Square_" + position.x + "_" + position.y);
        if (squareObj != null)
        {
            return squareObj.GetComponent<GridSquare>();
        }
        return null;
    }

    void ReconstructPath(Dictionary<GridSquare, GridSquare> cameFrom, GridSquare current)
    {
        // Reconstruct the path from the target square to the start square
        path.Clear();
        while (cameFrom.ContainsKey(current))
        {
            path.Insert(0, current);
            current = cameFrom[current];
        }
    }

    IEnumerator MoveAlongPath()
    {
        // Move the NPC along the path
        isWalking = true;
        Vector3 previousPosition = transform.position;

        Vector3? lastPosition = null;
        foreach (var square in path)
        {
            Vector3 targetPosition = new Vector3(square.transform.position.x, square.transform.position.y, transform.position.z);

            if (lastPosition.HasValue)
            {
                targetPosition = Vector3.Lerp(lastPosition.Value, targetPosition, 0.5f); // Interpolate to smooth the path
            }

            // Move towards the target position until it's reached
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                // Check for player input to interrupt the movement
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    isWalking = false;
                    yield break;
                }

                Vector3 currentPosition = transform.position;

                // Detect left or right movement
                if (currentPosition.x > previousPosition.x)
                {
                    horizontalMovement.OrientingHorizontally(1f);
                }
                else if (currentPosition.x < previousPosition.x)
                {
                    horizontalMovement.OrientingHorizontally(-1f);
                }

                previousPosition = currentPosition;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            // Ensure the NPC reaches the exact target position
            transform.position = targetPosition;
            currentSquare = square;
            lastPosition = targetPosition; // Update lastPosition
        }

        // Final smooth movement towards the exact clicked point
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        // Ensure the NPC reaches the exact clicked point smoothly
        transform.position = Vector3.Lerp(transform.position, targetPosition, 1f);

        isWalking = false;
    }

    GridSquare FindNearestSquare()
    {
        // Find the nearest grid square to the NPC
        GridSquare[] allSquares = FindObjectsOfType<GridSquare>();
        if (allSquares.Length == 0)
        {
            return null;
        }

        float minDistance = float.MaxValue;
        GridSquare nearestSquare = null;

        // Calculate the distance to each grid square and find the nearest one
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

    // Public method to set a new target and start moving towards it
    public void SetNewTarget(Vector3 newTargetPosition)
    {
        // Update the target position
        targetPosition = AdjustTargetPosition(newTargetPosition);

        // Find the target square based on the new target position
        GridSquare targetSquare = FindSquareAtPosition(targetPosition);
        if (targetSquare != null && targetSquare.walkable)
        {
            // Stop the current movement coroutine if it's running
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            // Clear the current path and find a new path to the target square
            path.Clear();
            FindPath(currentSquare, targetSquare);

            // Start moving along the new path
            moveCoroutine = StartCoroutine(MoveAlongPath());
        }
    }

    private Vector3 AdjustTargetPosition(Vector3 targetPos)
    {
        // Check for nearby non-walkable areas and adjust the target position if necessary
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(targetPos, capsuleCollider.size.y / 2);
        foreach (var hitCollider in hitColliders)
        {
            if (!hitCollider.CompareTag("GridSquare"))
            {
                Vector3 direction = (transform.position - targetPos).normalized;
                targetPos += direction * (capsuleCollider.size.y / 2);
            }
        }
        return targetPos;
    }
}
