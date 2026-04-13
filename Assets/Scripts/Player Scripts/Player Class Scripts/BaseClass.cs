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
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private BasicWeaponObj weapon1 = null;
    [SerializeField] private BasicWeaponObj weapon2 = null;
    [SerializeField] private BasicWeaponObj activeWeapon = null;
    [Header("Health")]
    //Health Based Stats
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int healthLevel = 0;

    [Header("Action Points")]
    //Action Based Stats
    [SerializeField] private int maxActions;
    [SerializeField] private int currentActions;
    [SerializeField] private int actionsLevel = 0;

    [Header("Speed")]
    //Speed Based Stats
    [SerializeField] private int speed;
    [SerializeField] private int speedLevel = 0;

    [Header("Player Attributes")]
    //Strength Based Stats
    [SerializeField] private int strength;
    [SerializeField] private int strengthLevel = 0;

    //Dexterity Based Stats
    [SerializeField] private int dexterity;
    [SerializeField] private int dexterityLevel = 0;

    //Perception Based Stats
    [SerializeField] private int perception;
    [SerializeField] private int perceptionLevel = 0;

    //Charisma based Stats
    [SerializeField] private int charisma;
    [SerializeField] private int charismaLevel = 0;

    [Header("Level Up Stuff")]
    [SerializeField] private float experiencePoints;

    [Header("Weapon Stuff")]
    [SerializeField] private int weaponDamage;
    [SerializeField] private int ammo;
    [SerializeField] private float accuracy;
    [SerializeField] private int armorPiercing;
    [SerializeField] private int burst;
    [SerializeField] private int range;

    private void Start()
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
    }

    private void Update()
    {
        weaponDamage = activeWeapon.Damage;
        ammo = activeWeapon.Ammunition;
        accuracy = activeWeapon.Accuracy;
        armorPiercing = activeWeapon.ArmorPiercing;
        burst = activeWeapon.Burst;
        range = activeWeapon.Range;
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
    private void healthChange(int amount)
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

    private void death()
    {
        maxHealth = 0;
        currentHealth = 0;
        maxActions = 0;
        currentActions = 0;
    }
}
