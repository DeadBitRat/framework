using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathfindingDoorController : MonoBehaviour
{
    private DBRDoorController doorController;
    private BoxCollider2D doorCollider;
    private BoxCollider2D pathfinderBoxCollider;

    public List<GridSquare> gridSquaresUnderDoor;
    private bool previousLockState;

    void Start()
    {
        doorController = GetComponent<DBRDoorController>();
        doorCollider = GetComponent<BoxCollider2D>();
        pathfinderBoxCollider = transform.Find("Pathfinder Door Handler").GetComponent<BoxCollider2D>();
        gridSquaresUnderDoor = new List<GridSquare>();

        // Adjust the size of the pathfinder box collider
        AdjustPathfinderBoxCollider();

        UpdateGridSquaresList();
        UpdateGridSquaresWalkability();

        // Store the initial lock state
        previousLockState = doorController.isLocked;
    }

    void Update()
    {
        // Check if the lock state has changed
        if (doorController.isLocked != previousLockState)
        {
            UpdateGridSquaresWalkability();
            previousLockState = doorController.isLocked;
        }
    }

    private void AdjustPathfinderBoxCollider()
    {
        // Make the pathfinder box collider slightly smaller proportionally than the door collider
        float reductionFactor = 0.9f; // 10% smaller

        pathfinderBoxCollider.size = new Vector2(
            doorCollider.size.x * reductionFactor,
            doorCollider.size.y * reductionFactor
        );

        pathfinderBoxCollider.isTrigger = true;
    }

    private void UpdateGridSquaresList()
    {
        // Disable pathfinder box collider temporarily to avoid self-detection
        pathfinderBoxCollider.enabled = false;

        // Get all colliders overlapping with the pathfinder box collider's bounds
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pathfinderBoxCollider.bounds.center, pathfinderBoxCollider.bounds.size, 0);

        // Re-enable pathfinder box collider
        pathfinderBoxCollider.enabled = true;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("GridSquare"))
            {
                GridSquare gridSquare = collider.GetComponent<GridSquare>();
                if (gridSquare != null && !gridSquaresUnderDoor.Contains(gridSquare))
                {
                    gridSquaresUnderDoor.Add(gridSquare);
                }
            }
        }
    }

    private void UpdateGridSquaresWalkability()
    {
        foreach (GridSquare gridSquare in gridSquaresUnderDoor)
        {
            gridSquare.walkable = !doorController.isLocked;
        }
    }
}
