using UnityEngine;

public class PuzzleSlidingDoor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MultiStagePuzzleController puzzleController;
    [SerializeField] private Transform door;

    [Header("Slide Settings")]
    [SerializeField] private float slideHeight = 4f;
    [SerializeField] private float slideSpeed = 2f;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.I;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private bool isOpen = false;
    private bool isMoving = false;

    private int objectsInside = 0;

    private void Start()
    {
        closedPosition = door.position;
        openPosition = closedPosition + Vector3.up * slideHeight;
    }

    private void Update()
    {
        // Only allow interaction if:
        // - Player/object is inside trigger
        // - Door is not already open
        if (!isOpen && objectsInside > 0 && Input.GetKeyDown(interactKey))
        {
            TryOpenDoor();
        }

        // Smooth upward movement
        if (isMoving)
        {
            door.position = Vector3.MoveTowards(
                door.position,
                openPosition,
                slideSpeed * Time.deltaTime
            );

            if (Vector3.Distance(door.position, openPosition) < 0.01f)
            {
                door.position = openPosition;
                isMoving = false;
            }
        }
    }

    private void TryOpenDoor()
    {
        // 🔒 Hard gate: requires both levers
        if (!puzzleController.IsPuzzleComplete())
        {
            return;
        }

        OpenDoor();
    }

    private void OpenDoor()
    {
        isOpen = true;
        isMoving = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Selectable"))
        {
            objectsInside++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Selectable"))
        {
            objectsInside = Mathf.Max(0, objectsInside - 1);
        }
    }
}