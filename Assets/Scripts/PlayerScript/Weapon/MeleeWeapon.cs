using UnityEngine;

public class MeleeWeapon : WeaponScript
{
    Collider weaponCollider;
    private void Start()
    {
        ultCharge = player.GetComponent<UltimateAbilityComponent>();
        weaponCollider = GetComponent<Collider>();
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
        TryDealDamage(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryDealDamage(other);
    }

    private void TryDealDamage(Collider other)
    {
        if (!canDealDamage) return;

        PlayerHealthComponent playerHealth = other.GetComponent<PlayerHealthComponent>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);

            if (ultCharge != null)
                ultCharge.ChargeUltDamage(damage, player);

            canDealDamage = false;
        }
    }
}
