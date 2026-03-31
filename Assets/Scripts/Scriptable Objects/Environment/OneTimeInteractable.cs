using UnityEngine;

public class OneTimeInteractable : MonoBehaviour
{
    [Header("Objects To Remove")]
    [SerializeField] private GameObject objectToRemoveA;
    [SerializeField] private GameObject objectToRemoveB;

    [Header("Object To Move")]
    [SerializeField] private Transform objectToMove;
    [SerializeField] private float moveHeight = 3f;
    [SerializeField] private float moveSpeed = 2f;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.I;

    private bool hasActivated = false;
    private bool isMoving = false;

    private Vector3 startPos;
    private Vector3 targetPos;

    private int objectsInside = 0;

    private void Start()
    {
        if (objectToMove != null)
        {
            startPos = objectToMove.position;
            targetPos = startPos + Vector3.up * moveHeight;
        }
    }

    private void Update()
    {
        // Only allow interaction once and if something valid is inside
        if (!hasActivated && objectsInside > 0 && Input.GetKeyDown(interactKey))
        {
            Activate();
        }

        // Handle smooth upward movement
        if (isMoving && objectToMove != null)
        {
            objectToMove.position = Vector3.Lerp(
                objectToMove.position,
                targetPos,
                Time.deltaTime * moveSpeed
            );

            // Stop when close enough
            if (Vector3.Distance(objectToMove.position, targetPos) < 0.01f)
            {
                objectToMove.position = targetPos;
                isMoving = false;
            }
        }
    }

    private void Activate()
    {
        hasActivated = true;

        // Remove objects
        if (objectToRemoveA != null)
            Destroy(objectToRemoveA);

        if (objectToRemoveB != null)
            Destroy(objectToRemoveB);

        // Start movement
        if (objectToMove != null)
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