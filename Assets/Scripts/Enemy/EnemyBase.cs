using System.Xml.Serialization;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected int health = 0;
    [SerializeField] public bool seen;
    [SerializeField] public bool allwaysSeen;

    protected void Start()
    {
        health = 5;
        seen = false;
    }
    protected void Update()
    {
        if (allwaysSeen)
        {
            seen = true;
        }
        if (seen == false)
        {
            //hiding code
            gameObject.GetComponent<Renderer>().enabled = false;
        }
        else if (seen == true)
        {
            //showing code
            gameObject.GetComponent<Renderer>().enabled = true;
        }
    }

    public void takingDamage(int damage)
    {
        damage = damage * -1;
        healthChange(damage);
    }

    protected void healthChange(int change)
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
