using System.Xml.Serialization;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private int health = 0;

    private void Start()
    {
        health = 5;
    }

    public void takingDamage(int damage)
    {
        damage = damage * -1;
        healthChange(damage);
    }
    private void healthChange(int change)
    {
        health = health + change;
        if (health > 0)
        {
            health = 5;
        }
        if (health <= 0)
        {
            Debug.Log("Killed this one");
            Destroy(gameObject);
        }
    }
}
