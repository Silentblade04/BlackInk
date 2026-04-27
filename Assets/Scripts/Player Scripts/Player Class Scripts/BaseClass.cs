using UnityEngine;

public class BaseClass : MonoBehaviour
{
    [Header("Public Weapon Stats")]
    public int Damage { get { return weaponDamage; } }
    public int Ammo { get { return ammo; } }
    public float Accuracy { get { return accuracy; } }
    public int ArmorPiercing { get { return armorPiercing; } }
    public int Burst { get { return burst; } }
    public int Range { get { return range; } }


    [Header("Scriptable Objects")]
    [SerializeField] protected PlayerStats playerStats;
    [SerializeField] protected BasicWeaponObj weapon1 = null;
    [SerializeField] protected BasicWeaponObj weapon2 = null;
    [SerializeField] protected BasicWeaponObj activeWeapon = null;
    [Header("Health")]
    //Health Based Stats
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int healthLevel = 0;

    [Header("Action Points")]
    //Action Based Stats
    [SerializeField] protected int maxActions;
    [SerializeField] protected int currentActions;
    [SerializeField] protected int actionsLevel = 0;

    [Header("Speed")]
    //Speed Based Stats
    [SerializeField] protected int speed;
    [SerializeField] protected int speedLevel = 0;

    [Header("Player Attributes")]
    //Strength Based Stats
    [SerializeField] protected int strength;
    [SerializeField] protected int strengthLevel = 0;

    //Dexterity Based Stats
    [SerializeField] protected int dexterity;
    [SerializeField] protected int dexterityLevel = 0;

    //Perception Based Stats
    [SerializeField] protected int perception;
    [SerializeField] protected int perceptionLevel = 0;
    [SerializeField] protected float sightRange;

    //Charisma based Stats
    [SerializeField] protected int charisma;
    [SerializeField] protected int charismaLevel = 0;

    [Header("Level Up Stuff")]
    [SerializeField] protected float experiencePoints;

    [Header("Weapon Stuff")]
    [SerializeField] protected int weaponDamage;
    [SerializeField] protected int ammo;
    [SerializeField] protected float accuracy;
    [SerializeField] protected int armorPiercing;
    [SerializeField] protected int burst;
    [SerializeField] protected int range;

    protected void Start()
    {
        //Setting the player levels to their base
        healthLevel = playerStats.hp;
        actionsLevel = playerStats.act;
        speedLevel = playerStats.spd;
        strengthLevel = playerStats.str;
        dexterityLevel = playerStats.dex;
        perceptionLevel = playerStats.precep;
        charismaLevel = playerStats.chr;

        //Setting the max for stats
        maxHealth = healthLevel * 3;
        maxActions = actionsLevel + 2;
        speed = speedLevel * 10;

        //Setting the current stats
        currentHealth = maxHealth;
        currentActions = maxActions;
        activeWeapon = weapon1;

        //sets attribute based stats
        sightRange = perceptionLevel * 10;
    }

    protected void Update()
    {
        weaponDamage = activeWeapon.Damage;
        ammo = activeWeapon.Ammunition;
        accuracy = activeWeapon.Accuracy;
        armorPiercing = activeWeapon.ArmorPiercing;
        burst = activeWeapon.Burst;
        range = activeWeapon.Range;

        lineOfSight();
    }

    //Deals with the player taking damage
    public void playerDamaged (int damage)
    {
        damage = damage * -1;
        healthChange (damage);
    }
    public void playerHealed (int heal)
    {
        healthChange(heal);
    }

    //Deals with any changes in player health
    protected void healthChange(int amount)
    {
        currentHealth = currentHealth + amount;
        if (currentHealth >  maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth <= 0)
        {
            death();
        }
    }

    protected void lineOfSight()
    {
        Collider[] losCollision= Physics.OverlapSphere(gameObject.transform.position, sightRange);
        foreach (var collision in losCollision)
        {
            if (collision.gameObject.TryGetComponent<EnemyBase>(out EnemyBase hitenemy))
            {
                //Debug.DrawRay(gameObject.transform.position, (collision.transform.position - transform.position), Color.red, 1f);
                if (Physics.Raycast(gameObject.transform.position, (collision.transform.position - transform.position), out RaycastHit hit))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.yellow);
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        hit.collider.GetComponent<EnemyBase>().seen = true;
                    }
                }
            }
        }

    }

    protected void death()
    {
        maxHealth = 0;
        currentHealth = 0;
        maxActions = 0;
        currentActions = 0;
    }
}
