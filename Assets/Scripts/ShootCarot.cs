using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroStory
{

    public class ShootCarot : MonoBehaviour
    {
        [SerializeField] float m_Damage;
        [SerializeField] float m_FireRate;
        [SerializeField] ParticleSystem m_ParticlesFire;
        [SerializeField] ParticleSystem m_ParticlesFireCollision;
        [SerializeField] AudioClip m_FireSound;

        private AudioSource m_AudioSource;

        public List<ParticleCollisionEvent> CollisionEvents;

        private bool m_IsFireCooldown;

        void Start()
        {
            CollisionEvents = new List<ParticleCollisionEvent>();
            m_AudioSource = GetComponent<AudioSource>();
        }

        public void Fire(int nbEmit)
        {
            if (m_IsFireCooldown) return;
            m_IsFireCooldown = true;
            StartCoroutine(StopCooldownAfterTime());
            if(m_AudioSource) m_AudioSource.PlayOneShot(m_FireSound);
            m_ParticlesFire.Emit(nbEmit);
        }
        IEnumerator StopCooldownAfterTime()
        {
            yield return new WaitForSeconds(m_FireRate);
            m_IsFireCooldown = false;
        }

        void OnParticleCollision(GameObject other)
        {
            int numCollisionEvents = m_ParticlesFire.GetCollisionEvents(other, CollisionEvents);

            m_ParticlesFireCollision.transform.position = CollisionEvents[0].intersection;
            m_ParticlesFireCollision.Play();

            if (other.tag == "Player")
            {
                float distance = Vector3.Distance(other.transform.position, GameObject.Find("Carotte").transform.position);
                if (distance < 1) distance = 1;

                HeroController.Instance.Health -= m_Damage + 5 / distance;
                Vector3 force = CollisionEvents[0].velocity * 10;
                other.GetComponent<Rigidbody>().AddForce(force);
            }
            else if (other.GetComponent<Health>() != null)
            {
                var health = other.GetComponent<Health>();
                health.TakeDamage(m_Damage);
            }
        }
    }
}
