using UnityEngine;
[CreateAssetMenu(fileName = "Weapon", menuName = "Dev/Weapons")] 
public class BasicWeaponObj : ScriptableObject
{
    [SerializeField] protected int damage;
    [SerializeField] protected int ammunition;
    [SerializeField] protected float accuracy;
    [SerializeField] protected int armorpiercing;
    [SerializeField] protected int burst;
    [SerializeField] protected int range;

    public int Damage { get { return damage; } }
    public int Ammunition { get { return ammunition; } }
    public float Accuracy { get { return accuracy; } }
    public int ArmorPiercing { get { return armorpiercing; } }
    public int Burst { get { return burst; } }
    public int Range { get { return range; } }
}
