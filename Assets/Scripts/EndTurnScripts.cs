using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // <-- add this

public class EndTurnScripts : MonoBehaviour
{

    [SerializeField] private List<EnemyBase> enemies = new List<EnemyBase>();
    [SerializeField] private List<BaseClass> playerCharacters = new List<BaseClass>();

    [System.Obsolete]
    void Start()
    {
        enemies.Clear();
        foreach (GameObject obj in Object.FindObjectsOfType<GameObject>())
        {
            // Check for enemy scripts
            EnemyBase E = obj.GetComponent<EnemyBase>();
            if (E != null) enemies.Add(E);

            // Check for player scripts
            BaseClass PC = obj.GetComponent<BaseClass>();
            if (PC != null) playerCharacters.Add(PC);
        }
        Debug.Log("Found " + enemies.Count + " Enemy components.");
        Debug.Log("Found " + playerCharacters.Count + " Player components.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void endTurn()
    {
        foreach (EnemyBase E in enemies)
        {
            E.seen = false;
        }
    }
}
