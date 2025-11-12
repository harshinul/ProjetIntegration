using System;
using UnityEngine;

public abstract class WeaponScript : MonoBehaviour
{
    public GameObject player;
    public bool canDealDamage = false;
    protected UltimateAbilityComponent ultCharge;
    public float damage = 10f;

    virtual public void Attack1()
    {
    }

    virtual public void Attack2()
    {
    }

    virtual public void StopAttack()
    {
    }
    //public bool isInPlayerEnemie = false;

    //private void OnTriggerStay(Collider other)
    //{
    //    if (canDealDamage && other.CompareTag("Player") && other.gameObject != player)
    //    {
    //        other.GetComponent<PlayerHealthComponent>()?.TakeDamage(damage);
    //        canDealDamage = false;
    //    }
    //}


    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle Collision with " + other.name);
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

