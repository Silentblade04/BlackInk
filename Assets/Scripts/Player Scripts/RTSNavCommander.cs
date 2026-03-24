using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RTSNavCommander : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;

    // Destination markers
    private Dictionary<NavMeshAgent, GameObject> destinationTiles = new Dictionary<NavMeshAgent, GameObject>();

    public NavMeshAgent currentlySelectedAgent = null;

    public int maxMoveDistance = 15;

    // Movement state
    private bool MovingPrimed = false;

    // Movement Execution variable
    private bool MoveExecute = false;

    // Cached moves
    private Dictionary<NavMeshAgent, Vector3> queuedMoves = new Dictionary<NavMeshAgent, Vector3>();

    //move buttons
    [SerializeField] private Button moveButton;

    void Update()
    {
        //Hiding attack UI
        if (MovingPrimed == true)
        {
            moveButton.gameObject.SetActive(true);
        }
        else if (MovingPrimed == false)
        {
            moveButton.gameObject.SetActive(false);
        }

        if (Keyboard.current == null) return;

        // Prime movement with keyboard
        if (Keyboard.current.mKey.wasPressedThisFrame && queuedMoves.Count == 0 && MovingPrimed == false)
        {
            PrimeMovement();
        }

        if (MovingPrimed)
        {
            //HandleSelection();
            //HandleRaycast();
            HandleMoveExecution();
        }
    }

    // ===============================
    // UI ACCESS
    // ===============================

    // Called by UI Button
    public void PrimeMovement()
    {
        Debug.Log("Moving Not Primed.");

        if (queuedMoves.Count == 0 && MovingPrimed == false)
        {
            MovingPrimed = true;
            Debug.Log("Moving Primed.");
        }
        else if (queuedMoves.Count == 0 && MovingPrimed == true)
        {
            MovingPrimed = false;
            Debug.Log("Moving Deprimed.");
        }
    }

    // ===============================
    // SELECTION
    // ===============================

    /* void HandleSelection()
     {
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
    */

    // ===============================
    // DESTINATION RAYCAST
    // ===============================

    /*
    void HandleRaycast()
    {
        if (currentlySelectedAgent == null) return;
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return; // Skip selection
            }

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
    */

    public void HandleNavMeshRay(RaycastHit hit)
    {
        if (currentlySelectedAgent == null) return;

        if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
        {
            Vector3 roundedPosition = new Vector3(
                Mathf.Round(navHit.position.x),
                navHit.position.y,
                Mathf.Round(navHit.position.z)
            );

            NavMeshPath path = new NavMeshPath();

            // Calculate full path
            if (!currentlySelectedAgent.CalculatePath(roundedPosition, path) || path.status != NavMeshPathStatus.PathComplete)
            {
                Debug.Log("Invalid path.");
                return;
            }

            float pathLength = GetPathLength(path);

            // Clamp to maxMoveDistance along path
            if (pathLength > maxMoveDistance)
            {
                Vector3 clampedPosition = GetPointAlongPath(path, maxMoveDistance);

                if (NavMesh.SamplePosition(clampedPosition, out NavMeshHit clampedHit, 1f, NavMesh.AllAreas))
                {
                    roundedPosition = new Vector3(
                        Mathf.Round(clampedHit.position.x),
                        clampedHit.position.y,
                        Mathf.Round(clampedHit.position.z)
                    );
                }
                else
                {
                    Debug.Log("Failed to clamp to NavMesh.");
                    return;
                }
            }

            TryCacheMove(currentlySelectedAgent, roundedPosition);
        }
    }

    // ===============================
    // PATH UTILITIES (NEW)
    // ===============================

    float GetPathLength(NavMeshPath path)
    {
        float length = 0f;

        for (int i = 1; i < path.corners.Length; i++)
        {
            length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }

        return length;
    }

    Vector3 GetPointAlongPath(NavMeshPath path, float maxDistance)
    {
        float distanceSoFar = 0f;

        for (int i = 1; i < path.corners.Length; i++)
        {
            Vector3 start = path.corners[i - 1];
            Vector3 end = path.corners[i];

            float segmentLength = Vector3.Distance(start, end);

            if (distanceSoFar + segmentLength >= maxDistance)
            {
                float remaining = maxDistance - distanceSoFar;
                Vector3 direction = (end - start).normalized;

                return start + direction * remaining;
            }

            distanceSoFar += segmentLength;
        }

        return path.corners[path.corners.Length - 1];
    }

    // ===============================
    // CACHE MOVES
    // ===============================

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
            UpdateDestinationTile(agent, destination);

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

    // ===============================
    // DESTINATION TILE VISUALS
    // ===============================

    void UpdateDestinationTile(NavMeshAgent agent, Vector3 destination)
    {
        if (destinationTiles.ContainsKey(agent))
        {
            destinationTiles[agent].transform.position = destination;
        }
        else
        {
            GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tile.transform.position = destination;
            tile.transform.localScale = new Vector3(1, 0.1f, 1);

            tile.GetComponent<Renderer>().material.color = Color.blue;

            destinationTiles[agent] = tile;
        }
    }

    // ===============================
    // EXECUTE MOVES
    // ===============================

    public void MoveExecuteVar()
    {
        MoveExecute = true;
    }

    void HandleMoveExecution()
    {
        if (MoveExecute == true && queuedMoves.Count > 0)
        {
            foreach (var move in queuedMoves)
            {
                if (move.Key != null)
                    move.Key.SetDestination(move.Value);
            }

            MoveExecute = false;
            ClearTiles();
            ClearAll();
        }
    }

    // ===============================
    // CLEANUP
    // ===============================

    void ClearTiles()
    {
        foreach (var tile in destinationTiles.Values)
        {
            if (tile != null) Destroy(tile);
        }

        destinationTiles.Clear();
    }

    void ClearAll()
    {
        currentlySelectedAgent = null;
        queuedMoves.Clear();

        Debug.Log("All moves executed. System reset.");
    }
}