using UnityEngine;

public class HingedDoor : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private Transform door; // The rotating door
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.I;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private bool isOpen = false;
    private int objectsInside = 0;

    private void Start()
    {
        closedRotation = door.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    private void Update()
    {
        // Handle key input ONLY if something valid is inside trigger
        if (objectsInside > 0 && Input.GetKeyDown(interactKey))
        {
            ToggleDoor();
        }

        // Smooth rotation
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

        door.rotation = Quaternion.Slerp(
            door.rotation,
            targetRotation,
            Time.deltaTime * openSpeed
        );
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }

    public void OpenDoor()
    {
        isOpen = true;
    }

    public void CloseDoor()
    {
        isOpen = false;
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