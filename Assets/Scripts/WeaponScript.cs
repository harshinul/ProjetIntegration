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




}

