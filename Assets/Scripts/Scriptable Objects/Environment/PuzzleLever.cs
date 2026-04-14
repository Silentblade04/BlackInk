using UnityEngine;

public class PuzzleLever : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private MultiStagePuzzleController puzzleController;
    [SerializeField] private bool isLeverA = true;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.I;

    private bool hasActivated = false;
    private int objectsInside = 0;

    private void Update()
    {
        // Only allow interaction if:
        // - Not already used
        // - A valid object is inside trigger
        if (!hasActivated && objectsInside > 0 && Input.GetKeyDown(interactKey))
        {
            ActivateLever();
        }
    }

    private void ActivateLever()
    {
        hasActivated = true;

        if (isLeverA)
            puzzleController.ActivateLeverA();
        else
            puzzleController.ActivateLeverB();

        // Optional: animation / feedback here
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