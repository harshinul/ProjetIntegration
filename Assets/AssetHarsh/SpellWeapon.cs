using MaykerStudio.Demo;
using UnityEngine;
using UnityEngine.Rendering;

public class SpellWeapon : WeaponScript
{
    [SerializeField] GameObject smallFireball;
    [SerializeField] GameObject bigFireball;
    [SerializeField] Transform firePoint;
    [SerializeField] PlayerMovementComponent playerMovement;

    ClassType classType = ClassType.Mage;
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
        var lightAttack = Instantiate(smallFireball, firePoint.position, Quaternion.Euler(0, transform.rotation.y > 0 ? 90 : -90, 90));
        var obj = lightAttack.GetComponent<Projectile>();
        obj.damage = characterStats.lightDamage;
        obj.player = player;
        obj.Fire();
    }

    override public void Attack2()
    {
        canDealDamage = true;
        var heavyAttack = Instantiate(bigFireball, firePoint.position, Quaternion.Euler(0, transform.rotation.y > 0 ? 90 : -90, 90));
        var obj = heavyAttack.GetComponent<Projectile>();
        obj.damage = characterStats.heavyDamage;
        obj.player = player;
        obj.Fire();
    }
    override public void StopAttack()
    {
        canDealDamage = false;
    }
}