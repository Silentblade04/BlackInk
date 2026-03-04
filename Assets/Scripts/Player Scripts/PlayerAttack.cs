using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    //what the player wants to shoot
    [SerializeField] private GameObject target;

    //the player GameObject
    [SerializeField] private GameObject player;
    [SerializeField] private BasicWeaponObj weaponObj;

    //Players camera
    [SerializeField] Camera mainCamera;

    //Sets being in Attack Mode
    [SerializeField] private bool attackmode;

    //attack buttons
    [SerializeField] private Button attackButton;

    //Attacking variables
    [SerializeField] private int a; //Attacks
    [SerializeField] private List<int> b = new List<int>(); //Each base attack
    [SerializeField] private List<int> c = new List<int>(); //Each finalized attack



    private void Awake()
    {
        attackmode = false;
    }
    private void Update()
    {
        //Hiding attack UI
        if (attackmode == true)
        {
            attackButton.gameObject.SetActive(true);
        }
        else if (attackmode == false)
        {
            attackButton.gameObject.SetActive(false);
        }
        //Checking if attackmode is true
        if (attackmode == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //pulled from chatGPT
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                {
                    // Skips selection
                    return;
                }
                Ray mouseray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(mouseray, out RaycastHit hitInfo))
                {
                    //debug of what we hit
                    Debug.Log("Selected: " + hitInfo.collider.gameObject);

                    //Checking if what we hit is an enemy
                    if (hitInfo.collider.GetComponent<TestDummy>() == true)
                    {
                        //Store info about target
                        target = hitInfo.collider.gameObject;
                        
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        //end of checking if attack mode is true

        if (Input.GetKeyDown(KeyCode.F))
        {
            attackMode();
        }
        
    }

    //When the player presses the attack button, this will let them select the target.
    public void attackMode() 
    {
        Debug.Log("Entering Attack Mode");
        if (attackmode == true)
        {
            attackmode = false;
        }
        else
        {
            attackmode = true;
        }

    }

    public void attack(GameObject target, BasicWeaponObj weapon, GameObject player, int modifiers)
    {
        
        a = weapon.Burst;
        //Steave Harvey Kill Meme
        for (int i = 0; i <= a; i++)
        {
            b.Add(Random.Range(0, 20));
        }
        Debug.Log(b);
        foreach(int i in b)
        {
            c.Add(b[i] + modifiers);
        }
        Debug.Log("Attacked");
        Debug.Log(c);
    }
}
