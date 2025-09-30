using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public GameObject player;
    public bool canDealDamage = false;
    public float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(other != player.GetComponent<Collider>())
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
}
