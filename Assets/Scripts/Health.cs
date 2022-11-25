using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace HeroStory
{

    public class Health : MonoBehaviour
    {
        [SerializeField] float m_MaxHeath = 100;
        //[SerializeField] ParticleSystem m_DamageParticule;
        [SerializeField] AudioClip m_DamageSound;
        //[SerializeField] MeshRenderer m_ShootDamageQuad;
        //[SerializeField] Material[] m_ShootDamageMaterials;

        [SerializeField] AudioClip m_DestroySound;
        [SerializeField] ParticleSystem m_DestroyParticule;
        [SerializeField] Animator m_DestroyAnimation;
        [SerializeField] bool m_IsFriend;
        private bool m_IsDead;

        private Collider m_Collider;
        private Rigidbody m_Rb;
        private AudioSource m_AudioSource;

        // RespawnTime ?

        private void Start()
        {
            m_Collider = GetComponent<Collider>();
            m_Rb = GetComponent<Rigidbody>();
            m_AudioSource = GetComponent<AudioSource>();
        }

        public void TakeDamage(float damage)
        {
            m_MaxHeath -= damage;

            if(m_DamageSound) m_AudioSource.PlayOneShot(m_DamageSound);

            // => côté cible => modifier la texture en fonction de la vie
            // => exemple avec pot on peut mettre un quad devant avec une texture materiau qui évolue (se brise petit à petit).
            // Ou plus simple => une animation avec Sprite(quad) qui évolue en fonction de health en faisant apparaître des fissures et dommages :
            // cela limiterait le nombre de texture pour le même effet 
            //if (collision.relativeVelocity.magnitude > 2)
            //    audioSource.Play(); => Mettre sur other dans script health ou autre
            //m_DamageSound.Play();

            if (m_MaxHeath <= 0 && !m_IsDead)
            {
                m_IsDead = true;
                if (m_DestroySound) m_AudioSource.PlayOneShot(m_DestroySound);

                if (m_IsFriend) GameManager.Instance.OnFriendKilled();
                if (m_DestroyParticule) m_DestroyParticule.Play();

                m_Collider.enabled = false;
                if(m_Rb)
                {
                    m_Rb.isKinematic = false;
                    m_Rb.detectCollisions = false;
                }

                for (var i=0; i < gameObject.transform.childCount; i++)
                {
                    gameObject.transform.GetChild(i).gameObject.SetActive(false);
                }
                Destroy(gameObject, 5.0f);

            }
            else
            {
                // TODO play m_ShootAnimation + modification material du Quad
            }
        }

    }

}
