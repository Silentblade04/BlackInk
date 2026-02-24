using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
//with AI assistance
public class RTSNavCommander : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;

    private NavMeshAgent currentlySelectedAgent = null;

    // Stores cached moves per unit
    private Dictionary<NavMeshAgent, Vector3> queuedMoves = new Dictionary<NavMeshAgent, Vector3>();

    void Update()
    {
        HandleSelection();
        HandleRaycast();
        HandleMoveExecution();
    }

    // ===============================
    // 1️⃣ Selection (Single Active Unit)
    // ===============================
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

    // ===============================
    // 2️⃣ Raycast + Cache Move
    // ===============================
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
                    // Rounded position (whole integers)
                    Vector3 roundedPosition = new Vector3(
                        Mathf.Round(navHit.position.x),
                        navHit.position.y, // preserve height
                        Mathf.Round(navHit.position.z)
                    );

                    TryCacheMove(currentlySelectedAgent, roundedPosition);
                }
            }
        }
    }

    // ===============================
    // 3️⃣ Cache Move (No Duplicates)
    // ===============================
    void TryCacheMove(NavMeshAgent agent, Vector3 destination)
    {
        if (agent == null) return;

        // 🔵 Prevent two agents from reserving same position
        if (IsLocationAlreadyQueued(destination))
        {
            Debug.Log("Location already reserved by another unit.");
            return;
        }

        NavMeshPath path = new NavMeshPath();

        if (agent.CalculatePath(destination, path) &&
            path.status == NavMeshPathStatus.PathComplete)
        {
            queuedMoves[agent] = destination;
            Debug.Log($"Move cached for {agent.name}");
        }
        else
        {
            Debug.Log($"Destination unreachable for {agent.name}");
        }
    }

    // 🔵 Duplicate Prevention Helper
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
    // 4️⃣ Execute All Cached Moves
    // ===============================
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

            ClearAll();
        }
    }

    // ===============================
    // 5️⃣ Clear System
    // ===============================
    void ClearAll()
    {
        currentlySelectedAgent = null;
        queuedMoves.Clear();

        Debug.Log("All moves executed. System reset.");
    }
}