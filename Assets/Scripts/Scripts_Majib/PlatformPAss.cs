using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlatformPAss : MonoBehaviour
{
    private Collider platformCollider;

    void Awake()
    {
        platformCollider = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // vérifie que c'est le joueur
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.rigidbody;
            if (rb != null)
            {
                // si le joueur vient du dessous
                if (collision.contacts[0].normal.y < 0)
                {
                    // ignore la collision temporairement
                    Physics.IgnoreCollision(platformCollider, collision.collider, true);
                    StartCoroutine(RestoreCollision(collision.collider, 0.3f));
                }
            }
        }
    }

    private System.Collections.IEnumerator RestoreCollision(Collider playerCollider, float delay)
    {
        yield return new WaitForSeconds(delay);
        Physics.IgnoreCollision(platformCollider, playerCollider, false);
    }
}