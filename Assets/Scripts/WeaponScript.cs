using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public GameObject player;
    public bool canDealDamage = false;
    public float damage = 10f;
    public bool isInPlayerEnemie= false;

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

            if(other != player.GetComponent<Collider>() && other.CompareTag("Player"))
            {
                Debug.Log("Weapon hit the player");
                if (canDealDamage)
              {
                    PlayerHealthComponent playerHealth = other.GetComponent<PlayerHealthComponent>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(damage);
                        canDealDamage = false; // Prevent multiple damage instances in one swing
                    }
                }
            }

        
           
    }
}
