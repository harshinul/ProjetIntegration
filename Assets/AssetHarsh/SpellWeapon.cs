using MaykerStudio.Demo;
using UnityEngine;
using UnityEngine.Rendering;

public class SpellWeapon : WeaponScript
{
    [SerializeField] GameObject smallFireball;
    [SerializeField] GameObject bigFireball;
    [SerializeField] Transform firePoint;

    ClassType classType;
    CharacterStats characterStats;
    private void Start()
    {
        ultCharge = player.GetComponent<UltimateAbilityComponent>();
        Collider playerCollider = player.GetComponent<Collider>();
        characterStats = CharacterStats.GetStatsForClass(classType);
    }

    override public void Attack1()
    {
        canDealDamage = true;
        var obj = Instantiate(smallFireball, firePoint.position, Quaternion.Euler(0, transform.rotation.y > 0 ? 90 : -90, 90));
        obj.GetComponent<Projectile>().damage = characterStats.lightDamage;
        obj.GetComponent<Projectile>().player = player;
        obj.GetComponent<Projectile>().Fire();
    }

    override public void Attack2()
    {
        canDealDamage = true;
        var obj = Instantiate(bigFireball, firePoint.position, Quaternion.Euler(0, transform.rotation.y > 0 ? 90 : -90, 90));
        obj.GetComponent<Projectile>().damage = characterStats.heavyDamage;
        obj.GetComponent<Projectile>().player = player;
        obj.GetComponent<Projectile>().Fire();
    }
    override public void StopAttack()
    {
        canDealDamage = false;
    }
}