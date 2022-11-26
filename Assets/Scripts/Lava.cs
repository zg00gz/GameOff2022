using System;
using System.Collections;
using UnityEngine;

namespace HeroStory
{

    public class Lava : MonoBehaviour
    {
        [SerializeField] float m_fireDamage = 10;
        [SerializeField] float m_FireRate = 1;
        [SerializeField] ParticleSystem m_Particles;

        private bool m_IsFireCooldown;

        // Audio clip - heroDamage ou feu

        [SerializeField] AudioSource m_AudioSource;

        private void OnTriggerStay(Collider other)
        {
            if(other.tag == "Player" && !m_IsFireCooldown && HeroController.Instance.Health > 0)
            {
                m_IsFireCooldown = true;
                HeroController.Instance.Health -= m_fireDamage;
                other.GetComponent<Rigidbody>().AddExplosionForce(100.0f, other.transform.position, 2.0f);
                //m_Particles.Play();
                //m_AudioSource.Play();
                StartCoroutine(StopCooldownAfterTime());
            }
        }

        IEnumerator StopCooldownAfterTime()
        {
            yield return new WaitForSeconds(m_FireRate);
            m_IsFireCooldown = false;
        }

    }

}