using UnityEngine;

public class MeleeWeapon : WeaponScript
{
    private void Start()
    {
        ultCharge = player.GetComponent<UltimateAbilityComponent>();
        Collider weaponCollider = GetComponent<Collider>();
        Collider playerCollider = player.GetComponent<Collider>();
        if (weaponCollider != null && playerCollider != null)
            Physics.IgnoreCollision(weaponCollider, playerCollider);
    }

    override public void Attack1()
    {
        canDealDamage = true;
    }

    override public void Attack2()
    {
        canDealDamage = true;
    }

    override public void StopAttack()
    {
        canDealDamage = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (canDealDamage)
        {
            ultCharge.ChargeUltDamage(damage, player);
            PlayerHealthComponent playerHealth = other.GetComponent<PlayerHealthComponent>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                canDealDamage = false; // Prevent multiple damage instances in one swing
            }
        }
    }
}
