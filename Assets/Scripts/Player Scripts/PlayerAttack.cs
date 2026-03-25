using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerAttack : MonoBehaviour
{

    //what the player wants to shoot
    public GameObject attackTarget;

    //the player GameObject
    public GameObject playerObj = null;
    [SerializeField] private BasicWeaponObj weaponObj = null;

    //Players camera
    [SerializeField] Camera mainCamera;

    //Sets being in Attack Mode
    [SerializeField] private bool attackmode;

    //Sets if they should attack
    [SerializeField] private bool attackExecute;

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
        attackHandler();
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
    
    //Handles selecting what is shooting/getting shot
    public void selections(GameObject player, GameObject enemy)
    {
        Debug.Log("Calling selections");
        playerObj = player;
        attackTarget = enemy;
    }

    //Handles attack storing
    private void attackCaching(GameObject player,GameObject enemy)
    {
        if (player == null || enemy == null)
        {
            Debug.Log(player == null || enemy == null);
            return;
        }

    }

    //Handles the UI executing the attack
    public void attackVariableExecute()
    {
        Debug.Log("attack variable executed");
        attackExecute = true;
    }

   //Handles the actual attack execution
    private void attackHandler()
    {
        if (attackExecute == true)
        {
            Debug.Log("Attacked Handler fired");
            attack(attackTarget, weaponObj, playerObj, 0);
            attackExecute = false;
        }
    }

    //Handles all the attack inner workings
    private void attack(GameObject target, BasicWeaponObj weapon, GameObject player, int modifiers)
    {
        Debug.Log(player + " attacked " + target);
        attackTarget = null;
        playerObj = null;
    }
    
}
