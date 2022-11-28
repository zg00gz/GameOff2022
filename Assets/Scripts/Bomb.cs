using System;
using System.Collections;
using UnityEngine;

namespace HeroStory
{

    public class Bomb : MonoBehaviour
    {
        [SerializeField] float m_ExplosionDamage = 50;
        public float ExplosionDelay = 2;
        [SerializeField] ParticleSystem m_ExplosionParticles;
        [SerializeField] GameObject m_Meche;

        private bool m_IsArmed;
        public float radius = 5.0f;
        public float power = 50.0f;

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

        IEnumerator ExplosionArmed()
        {
            m_IsArmed = true;

            yield return new WaitForSeconds(ExplosionDelay);
            
            m_ExplosionParticles.Play(true);
            m_AudioSource.Play();
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            Destroy(m_Meche);
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