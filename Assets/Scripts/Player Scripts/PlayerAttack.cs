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

    //Players camera
    [SerializeField] Camera mainCamera;

    //Sets being in Attack Mode
    [SerializeField] private bool attackmode;

    //Sets if they should attack
    [SerializeField] private bool attackExecute;

    //attack buttons
    [SerializeField] private Button attackButton;

    //Handles player Base Class interactions
    [SerializeField] private BaseClass playerClass;
    [SerializeField] private EnemyBase attackTargetInfo;

    //Log refrence
    //[SerializeField] private Log log;

    //Chacheing Attacks
    [SerializeField] private Dictionary<BaseClass, EnemyBase> queuedAttacks = new Dictionary<BaseClass, EnemyBase>();



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
    public void selections(BaseClass playerInfo, GameObject player, GameObject enemy, EnemyBase enemyInfo)
    {
        Debug.Log("Calling selections");
        attackCaching(playerInfo, enemyInfo);
        playerObj = player;
        attackTarget = enemy;
    }
     
    //Handles attack storing
    private void attackCaching(BaseClass player,EnemyBase enemy)
    {
        if (player == null || enemy == null)
        {
            Debug.Log(player == null || enemy == null);
            return;
        }
        queuedAttacks[player] = enemy;

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
            foreach (KeyValuePair<BaseClass, EnemyBase> entry in queuedAttacks)
            {
                if (entry.Key == null)
                {
                    Debug.Log("This enemy is already dead");
                    return;
                }
                attack(entry.Value, entry.Key);
            }
            attackExecute = false;
            clearAtacks();
        }
    }

    //Handles all the attack inner workings
    private void attack(EnemyBase targetInfo, BaseClass baseClass)
    {
        //log.handleLog("attack");
        if (targetInfo == null)
        {
            Debug.Log("Base Class Null");
            return;
        }
        targetInfo.takingDamage(baseClass.Damage);

        Debug.Log(" Player attacked target");
        attackTarget = null;
        playerObj = null;
    }

    private void clearAtacks()
    {
        queuedAttacks.Clear();
    }
    
}
