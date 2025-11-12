using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaykerStudio.Demo
{
    public class Projectile : MonoBehaviour
    {

        public float speed = 5011;
        public float distance = 30;
        public float damage;
        private ParticleSystem mainParticle;

        private Vector3 initPosition;

        public GameObject player;
        private UltimateAbilityComponent ultCharge;
        private void Start()
        {
            ultCharge = player.GetComponent<UltimateAbilityComponent>();
            Collider weaponCollider = GetComponent<Collider>();
            Collider playerCollider = player.GetComponent<Collider>();
            if (weaponCollider != null && playerCollider != null)
                Physics.IgnoreCollision(weaponCollider, playerCollider);
        }

        public void Fire()
        {
            mainParticle = GetComponent<ParticleSystem>();

            mainParticle.Play(true);

            initPosition = transform.position;
        }

        private void Update()
        {
            if (mainParticle && mainParticle.isPlaying)
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime);

                if (Vector3.Distance(initPosition, transform.position) > distance)
                {
                    mainParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    Destroy(this);
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            ultCharge.ChargeUltDamage(damage, player);
            PlayerHealthComponent playerHealth = other.GetComponent<PlayerHealthComponent>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

        }
    }
}
