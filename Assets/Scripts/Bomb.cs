using System;
using System.Collections;
using UnityEngine;

namespace HeroStory
{

    public class Bomb : MonoBehaviour
    {
        [SerializeField] float m_ExplosionDamage = 50;
        [SerializeField] float m_ExplosionDelay = 2;
        [SerializeField] ParticleSystem m_ExplosionParticles;

        private bool m_IsArmed;
        [SerializeField] AudioSource m_AudioSource;

        public void Armed()
        {
            if(!m_IsArmed) StartCoroutine(ExplosionArmed());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && !m_IsArmed)
            {
                Armed();
            }
        }


        public float radius = 5.0F;
        public float power = 10.0F;

        IEnumerator ExplosionArmed()
        {
            m_IsArmed = true;
            // animation couleur clignotante

            yield return new WaitForSeconds(m_ExplosionDelay);

            Debug.Log("All around ! Boom !");
            //m_ExplosionParticles.transform.position = transform.position;
            m_ExplosionParticles.Play(true);
            m_AudioSource.Play();
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, 5.0f);

            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null && hit.tag == "Player")
                {
                    Debug.Log(hit.name);
                    HeroController.Instance.Health -= m_ExplosionDamage;
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
                }
                else if(hit.tag == "Zombi")
                {
                    hit.GetComponent<Health>().TakeDamage(1000);
                }
            }
        }

    }

}