using UnityEngine;

public class TriggerMover : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform objectToMove;

    [Header("Movement Settings")]
    [SerializeField] private float moveAmount = 3f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private int objectsInside = 0;

    private void Start()
    {
        startPosition = objectToMove.position;
        targetPosition = startPosition + Vector3.up * moveAmount;
    }

    private void Update()
    {
        Vector3 desiredPosition = (objectsInside > 0) ? targetPosition : startPosition;

        objectToMove.position = Vector3.Lerp(
            objectToMove.position,
            desiredPosition,
            Time.deltaTime * moveSpeed
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Selectable"))
        {
            objectsInside++;
            Debug.Log("Check");
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