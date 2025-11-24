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

    // Se déclenche si l'arme est DÉJÀ dans l'ennemi quand on l'active
    private void OnTriggerStay(Collider other)
    {
        TryDealDamage(other);
    }

    private void TryDealDamage(Collider other)
    {
        // On vérifie d'abord si on a le droit de faire des dégats
        if (!canDealDamage) return;

        PlayerHealthComponent playerHealth = other.GetComponent<PlayerHealthComponent>();

        // On vérifie si c'est bien un ennemi (a de la vie)
        if (playerHealth != null)
        {
            // On applique les dégats
            playerHealth.TakeDamage(damage);

            // On charge l'ultime SEULEMENT si on a touché un ennemi valide
            if (ultCharge != null)
                ultCharge.ChargeUltDamage(damage, player);

            // On désactive les dégats pour ne pas tuer l'ennemi en 1 frame
            canDealDamage = false;
        }
    }
}
