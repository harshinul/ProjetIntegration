using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class DamageOverTime : MonoBehaviour
{
    [SerializeField] float damagePerSecond = 20f;
    public GameObject player;

    List<PlayerHealthComponent> affectedPlayers = new List<PlayerHealthComponent>();
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealthComponent playerHealth = other.GetComponent<PlayerHealthComponent>();
        if (playerHealth != null && other.gameObject != player)
        {
            affectedPlayers.Add(playerHealth);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        affectedPlayers.Remove(other.GetComponent<PlayerHealthComponent>());
    }

    private void Update()
    {
        foreach (var playerHealth in affectedPlayers)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }
}
