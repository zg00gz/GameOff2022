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
        [SerializeField] Animator m_AlertHealthDecrease;

        private bool m_IsFireCooldown;

        // Audio clip - heroDamage ou feu

        [SerializeField] AudioSource m_AudioSource;

        private void OnTriggerStay(Collider other)
        {
            if(other.tag == "Player" && !m_IsFireCooldown && HeroController.Instance.Health > 0)
            {
                m_IsFireCooldown = true;
                HeroController.Instance.Health -= m_fireDamage;
                m_AlertHealthDecrease.SetTrigger("touch");
                other.GetComponent<Rigidbody>().AddExplosionForce(100.0f, other.transform.position, 2.0f);
                //other.GetComponent<Rigidbody>().AddExplosionForce(500.0f, new Vector3(other.transform.position.x, other.transform.position.y, 0), 5.0f);
                //other.GetComponent<Rigidbody>().AddForce(Vector3.forward, ForceMode.Impulse);
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