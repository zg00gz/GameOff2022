using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace HeroStory
{

    public class Shoot : MonoBehaviour
    {
        [SerializeField] float m_Damage;
        [SerializeField] float m_FireRate;
        [SerializeField] ParticleSystem m_ParticlesFire;
        [SerializeField] ParticleSystem m_ParticlesFireCollision;
        [SerializeField] AudioClip m_FireSound;

        private AudioSource m_AudioSource;

        public List<ParticleCollisionEvent> CollisionEvents;
        public CinemachineBrain Cam;

        public GameObject ExplosionPrefab;

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
            m_AudioSource.PlayOneShot(m_FireSound);
            m_ParticlesFire.Emit(nbEmit);
            HeroController.Instance.FireBalls--;
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

            if (other.CompareTag("BigTarget"))
            {
                Cam.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            }
            if (other.GetComponent<Health>() != null)
            {
                var health = other.GetComponent<Health>();
                health.TakeDamage(m_Damage);
            }
            else if(other.name == "Carotte")
            {
                var health = other.GetComponent<CarotteController>().Health;

                switch(GameManager.Instance.CurrentStep)
                {
                    case 2:
                        // Step0
                        if(health > 1400) other.GetComponent<CarotteController>().Health -= m_Damage;
                        break;
                    case 3:
                        // Step1
                        if (health > 1100) other.GetComponent<CarotteController>().Health -= m_Damage + 5;
                        break;
                    case 4:
                        // Step2
                        if (health > 800) other.GetComponent<CarotteController>().Health -= m_Damage + 5;
                        break;
                    case 5:
                        // Step3
                        if (health > 500) other.GetComponent<CarotteController>().Health -= m_Damage + 10;
                        break;
                    case 6:
                        // Step4
                        if (health > 0) other.GetComponent<CarotteController>().Health -= m_Damage + 10;
                        break;
                }
            }
            else if(other.name.StartsWith("Goose"))
            {
                other.GetComponent<Goose>().Touched();
            }

            if (other.GetComponent<Rigidbody>() != null)
            {
                Vector3 force = CollisionEvents[0].velocity * 10;
                other.GetComponent<Rigidbody>().AddForce(force);
            }

        }
    }
}
