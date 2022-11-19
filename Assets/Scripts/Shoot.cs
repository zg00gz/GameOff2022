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

        public List<ParticleCollisionEvent> CollisionEvents;
        public CinemachineBrain Cam;

        public GameObject ExplosionPrefab;

        private bool m_IsFireCooldown;

        void Start()
        {
            CollisionEvents = new List<ParticleCollisionEvent>();
        }

        public void Fire(int nbEmit)
        {
            if (m_IsFireCooldown) return;
            m_IsFireCooldown = true;
            StartCoroutine(StopCooldownAfterTime());
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

            if (other.CompareTag("BigTarget"))
            {
                Cam.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
                // TODO : variation de impulse pour de plus petite target ? Ou si surpuissance du gun
            }

            if (other.GetComponent<Health>() != null)
            {
                var health = other.GetComponent<Health>();
                health.TakeDamage(m_Damage);
            }

            if (other.GetComponent<Rigidbody>() != null)
            {
                Vector3 force = CollisionEvents[0].velocity * 10;
                other.GetComponent<Rigidbody>().AddForce(force);
            }

        }
    }
}
