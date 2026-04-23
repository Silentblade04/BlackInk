using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    //Camera
    [SerializeField] private Camera mainCamera;

    //Character Selection
    [SerializeField] private NavMeshAgent currentlySelectedAgent = null;
    [SerializeField] private GameObject playerObj = null;
    [SerializeField] private BaseClass playerInformation;
    [SerializeField] private GameObject enemyObj = null;
    [SerializeField] private EnemyBase enemyInformation;
    [SerializeField] private GameObject selectionTilePrefab;
    private GameObject currentTile;


    //Manager Refrences
    [SerializeField] private RTSNavCommander navCommander;
    [SerializeField] private PlayerAttack playerAttack;

    void Start()
    {
        
    }

    void Update()
    {
        HandleSelection();
        HandleRaycast();
        navCommander.currentlySelectedAgent = currentlySelectedAgent;
        playerAttack.playerObj = playerObj;
        playerAttack.attackTarget = enemyObj;
    }

    private void HandleSelection()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) SelectCharacter(1);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) SelectCharacter(2);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) SelectCharacter(3);
        if (Keyboard.current.digit4Key.wasPressedThisFrame) SelectCharacter(4);
        if (Keyboard.current.digit5Key.wasPressedThisFrame) SelectCharacter(5);
    }

    public void SelectCharacter(int index)
    {
        GameObject target = GameObject.Find("Character" + index);
        if (target == null) return;

        NavMeshAgent agent = target.GetComponent<NavMeshAgent>();
        if (agent == null) return;

        playerObj = target;
        playerInformation = playerObj.GetComponent<BaseClass>();
        currentlySelectedAgent = agent;
        Debug.Log("Selected: " + agent.name);
        // Spawn or move tile under selected character
        if (currentTile == null)
        {   
            currentTile = Instantiate(selectionTilePrefab);
        }
        currentTile.transform.position = new Vector3(
            playerObj.transform.position.x,
            playerObj.transform.position.y - 1f,
            playerObj.transform.position.z);
    }
    public void ClearSelectionTile()
    {
        if (currentTile != null)
        {
            Destroy(currentTile);
            currentTile = null;
        }
    }
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
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    enemyObj = hit.collider.gameObject;
                    enemyInformation = hit.collider.GetComponent<EnemyBase>();
                    playerAttack.selections(playerInformation, playerObj, enemyObj, enemyInformation);
                    Debug.Log("Are we getting here?" + enemyObj.name);
                }
                else
                {
                    navCommander.HandleNavMeshRay(hit);
                    Debug.Log("Sending to NavCommander");
                }
            }
        }
    }
    
}
