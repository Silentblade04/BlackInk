using UnityEngine;
[CreateAssetMenu(fileName = "Explosive Weapon", menuName = "Dev/Explosive Weapons")]
public class ExplosiveWeaponObj : BasicWeaponObj
{
    [SerializeField] protected float blastrange;
    [SerializeField] protected float shockwaveradius;
    [SerializeField] protected float killrange;

    public float BlastRange { get { return blastrange; } }
    public float ShockWaveRadious { get { return shockwaveradius; } }
    public float KillRange { get { return killrange; } }
}
