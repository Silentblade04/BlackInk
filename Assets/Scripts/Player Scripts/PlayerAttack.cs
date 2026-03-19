using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{

    //what the player wants to shoot
    public GameObject target;

    //the player GameObject
    public GameObject player = null;
    [SerializeField] private BasicWeaponObj weaponObj;

    //Players camera
    [SerializeField] Camera mainCamera;

    //Sets being in Attack Mode
    [SerializeField] private bool attackmode;

    //attack buttons
    [SerializeField] private Button attackButton;

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

        }
        //end of checking if attack mode is true     
        if (Keyboard.current.nKey.wasPressedThisFrame && attackmode == false)
        {
            attackMode();
        }

    }

    //When the player presses the attack button or hotkey, this will let them select the target.
    public void attackMode() 
    {
        if (attackmode == false)
        {
            attackmode = true;
        }
        else if (attackmode == true)
        {
            attackmode = false;
        }
    }
    public void selections(GameObject target, GameObject player)
    {

    }

    private void attack(GameObject target, BasicWeaponObj weapon, GameObject player, int modifiers)
    {

    }
    
}
