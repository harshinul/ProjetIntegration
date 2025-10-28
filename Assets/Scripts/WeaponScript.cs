using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public GameObject player;
    public bool canDealDamage = false;
    public float damage = 10f;
    public bool isInPlayerEnemie = false;
    private UltimateAbilityComponent ultCharge;
    private void Start()
    {
        ultCharge = player.GetComponent<UltimateAbilityComponent>();
        Collider weaponCollider = GetComponent<Collider>();
        Collider playerCollider = player.GetComponent<Collider>();
        if (weaponCollider != null && playerCollider != null)
            Physics.IgnoreCollision(weaponCollider, playerCollider);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (canDealDamage && other.CompareTag("Player") && other.gameObject != player)
    //    {
    //        other.GetComponent<PlayerHealthComponent>()?.TakeDamage(damage);
    //        canDealDamage = false;
    //    }
    //}
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

