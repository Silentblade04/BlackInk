using UnityEngine;

public class PuzzleLever : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private MultiStagePuzzleController puzzleController;
    [SerializeField] private bool isLeverA = true;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.I;

    [Header("Lever Rotation")]
    [SerializeField] private Transform leverHandle; // Assign the part that rotates
    [SerializeField] private float rotateAngle = -45f;
    [SerializeField] private float rotateSpeed = 4f;

    private bool hasActivated = false;
    private bool isRotating = false;

    private int objectsInside = 0;

    private Quaternion startRotation;
    private Quaternion targetRotation;

    private void Start()
    {
        if (leverHandle != null)
        {
            startRotation = leverHandle.localRotation;
            targetRotation = startRotation * Quaternion.Euler(rotateAngle, 0f, 0f);
        }
    }

    private void Update()
    {
        // Interaction check
        if (!hasActivated && objectsInside > 0 && Input.GetKeyDown(interactKey))
        {
            ActivateLever();
        }

        // Handle smooth rotation
        if (isRotating && leverHandle != null)
        {
            leverHandle.localRotation = Quaternion.Slerp(
                leverHandle.localRotation,
                targetRotation,
                Time.deltaTime * rotateSpeed
            );

            if (Quaternion.Angle(leverHandle.localRotation, targetRotation) < 0.5f)
            {
                leverHandle.localRotation = targetRotation;
                isRotating = false;
            }
        }
    }

    private void ActivateLever()
    {
        hasActivated = true;

        if (isLeverA)
            puzzleController.ActivateLeverA();
        else
            puzzleController.ActivateLeverB();

        StartRotation();
    }

    private void StartRotation()
    {
        if (leverHandle != null)
        {
            isRotating = true;
        }
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