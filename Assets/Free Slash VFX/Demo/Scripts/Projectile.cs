using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaykerStudio.Demo
{
    public class Projectile : MonoBehaviour
    {

        public float speed;
        public float distance;
        public float damage;
        private ParticleSystem mainParticle;
        private float timer = 0f;
        [SerializeField] float lifetime = 5f;
        Collider weaponCollider;
        Collider playerCollider;
        [SerializeField] bool isUltimate = false;

        private Vector3 initPosition;

        public GameObject player;
        private UltimateAbilityComponent ultCharge;
        private void Start()
        {
            ultCharge = player.GetComponent<UltimateAbilityComponent>();
            weaponCollider = GetComponent<Collider>();
            playerCollider = player.GetComponent<Collider>();
            if (weaponCollider != null && playerCollider != null)
                Physics.IgnoreCollision(weaponCollider, playerCollider);
        }

        public void Fire()
        {
            mainParticle = GetComponent<ParticleSystem>();

            mainParticle.Play(true);

            initPosition = transform.position;
            if(isUltimate)
                StartCoroutine(DeactivateColliderAfterTime(0.2f));
        }

        IEnumerator DeactivateColliderAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            weaponCollider.enabled = false;

        }

        private void Update()
        {
            timer += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime);
            if (mainParticle && mainParticle.isPlaying || timer >= lifetime)
            {

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
                if(!isUltimate)
                    Destroy(gameObject);
            }

        }
    }
}