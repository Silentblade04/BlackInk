using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class RTSNavCommander : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;

    // New fields for tile generation
    private Dictionary<NavMeshAgent, GameObject> destinationTiles = new Dictionary<NavMeshAgent, GameObject>(); // Destination markers

    private NavMeshAgent currentlySelectedAgent = null;
    private bool MovingPrimed = false;

    private Dictionary<NavMeshAgent, Vector3> queuedMoves = new Dictionary<NavMeshAgent, Vector3>();

    void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame && queuedMoves.Count == 0 && MovingPrimed == false)
        {
            MovingPrimed = true;
            Debug.Log("Moving Primed.");
        }
        if (MovingPrimed)
        {
            HandleSelection();
            HandleRaycast();
            HandleMoveExecution();
        }
    }

    void HandleSelection()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame) SelectCharacter(1);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) SelectCharacter(2);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) SelectCharacter(3);
        if (Keyboard.current.digit4Key.wasPressedThisFrame) SelectCharacter(4);
        if (Keyboard.current.digit5Key.wasPressedThisFrame) SelectCharacter(5);
    }

    void SelectCharacter(int index)
    {
        GameObject target = GameObject.Find("Character" + index);
        if (target == null) return;

        NavMeshAgent agent = target.GetComponent<NavMeshAgent>();
        if (agent == null) return;

        currentlySelectedAgent = agent;
        Debug.Log("Selected: " + agent.name);
    }

    void HandleRaycast()
    {
        if (currentlySelectedAgent == null) return;
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
                {
                    Vector3 roundedPosition = new Vector3(
                        Mathf.Round(navHit.position.x),
                        navHit.position.y,
                        Mathf.Round(navHit.position.z)
                    );

                    TryCacheMove(currentlySelectedAgent, roundedPosition);
                }
            }
        }
    }

    void TryCacheMove(NavMeshAgent agent, Vector3 destination)
    {
        if (agent == null) return;

        if (IsLocationAlreadyQueued(destination))
        {
            Debug.Log("Location already reserved by another unit.");
            return;
        }

        NavMeshPath path = new NavMeshPath();

        if (agent.CalculatePath(destination, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            queuedMoves[agent] = destination;
            UpdateDestinationTile(agent, destination); // Update the destination tile
            Debug.Log($"Move cached for {agent.name}");
        }
        else
        {
            Debug.Log($"Destination unreachable for {agent.name}");
        }
    }

    bool IsLocationAlreadyQueued(Vector3 position)
    {
        foreach (Vector3 queuedPosition in queuedMoves.Values)
        {
            if (queuedPosition == position)
                return true;
        }
        return false;
    }

    // Update or create the destination tile for the specific agent
    private void UpdateDestinationTile(NavMeshAgent agent, Vector3 destination)
    {
        if (destinationTiles.ContainsKey(agent))
        {
            // Move existing tile to the new destination
            destinationTiles[agent].transform.position = destination;
        }
        else
        {
            // Create the tile if it doesn't exist
            GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tile.transform.position = destination;
            tile.transform.localScale = new Vector3(1, 0.1f, 1);
            tile.GetComponent<Renderer>().material.color = Color.blue; // Color for visibility
            destinationTiles[agent] = tile; // Store the tile in the dictionary
        }
    }

    void HandleMoveExecution()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.mKey.wasPressedThisFrame && queuedMoves.Count > 0)
        {
            foreach (var move in queuedMoves)
            {
                if (move.Key != null)
                    move.Key.SetDestination(move.Value);
            }

            ClearTiles(); // Clear the tiles when executing moves
            ClearAll();
        }
    }

    private void ClearTiles()
    {
        foreach (var tile in destinationTiles.Values)
        {
            if (tile != null) Destroy(tile);
        }
        destinationTiles.Clear(); // Clear the dictionary after destroying tiles
    }

    void ClearAll()
    {
        currentlySelectedAgent = null;
        queuedMoves.Clear();
        Debug.Log("All moves executed. System reset.");
    }
}