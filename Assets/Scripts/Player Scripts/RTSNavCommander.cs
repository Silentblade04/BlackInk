using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
//Written with GPT assistance

public class RTSNavCommander : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;

    private List<NavMeshAgent> selectedAgents = new List<NavMeshAgent>();
    private Dictionary<NavMeshAgent, Vector3> queuedMoves = new Dictionary<NavMeshAgent, Vector3>();

    private bool awaitingDestination = false;
    private Vector3 cachedDestination;

    void Update()
    {
        HandleSelection();
        HandleRaycast();
        HandleMoveExecution();
    }

    // ===============================
    // 1️⃣ Selection (Keys 1–5)
    // ===============================
    void HandleSelection()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame) TrySelectCharacter(1);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) TrySelectCharacter(2);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) TrySelectCharacter(3);
        if (Keyboard.current.digit4Key.wasPressedThisFrame) TrySelectCharacter(4);
        if (Keyboard.current.digit5Key.wasPressedThisFrame) TrySelectCharacter(5);
    }

    void TrySelectCharacter(int index)
    {
        GameObject target = GameObject.Find("Character" + index);
        if (target == null) return;

        NavMeshAgent agent = target.GetComponent<NavMeshAgent>();
        if (agent == null) return;

        if (!selectedAgents.Contains(agent))
        {
            selectedAgents.Add(agent);
            Debug.Log("Selected: " + target.name);
        }

        awaitingDestination = true;
    }

    // ===============================
    // 2️⃣ Raycast + NavMesh Validation
    // ===============================
    void HandleRaycast()
    {
        if (!awaitingDestination || selectedAgents.Count == 0) return;
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
                {
                    cachedDestination = navHit.position;
                    QueueMoves(cachedDestination);
                    Debug.Log("Destination Cached");
                }
            }
        }
    }

    // ===============================
    // 3️⃣ Queue Valid Paths
    // ===============================
    void QueueMoves(Vector3 destination)
    {
        queuedMoves.Clear();

        foreach (NavMeshAgent agent in selectedAgents)
        {
            if (agent == null) continue;

            NavMeshPath path = new NavMeshPath();

            if (agent.CalculatePath(destination, path) &&
                path.status == NavMeshPathStatus.PathComplete)
            {
                queuedMoves[agent] = destination;
            }
        }
    }

    // ===============================
    // 4️⃣ Execute All on 'M'
    // ===============================
    void HandleMoveExecution()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.mKey.wasPressedThisFrame && queuedMoves.Count > 0)
        {
            foreach (var move in queuedMoves)
            {
                move.Key.SetDestination(move.Value);
            }

            ClearSelection();
        }
    }

    // ===============================
    // 5️⃣ Clear Selection
    // ===============================
    void ClearSelection()
    {
        selectedAgents.Clear();
        queuedMoves.Clear();
        awaitingDestination = false;

        Debug.Log("Selection Cleared");
    }
}
