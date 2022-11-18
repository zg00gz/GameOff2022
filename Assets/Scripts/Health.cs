using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace HeroStory
{

    public class Health : MonoBehaviour
    {
        [SerializeField] float m_MaxHeath = 100;
        [SerializeField] ParticleSystem m_DamageParticule;
        [SerializeField] AudioClip m_DamageSound;
        [SerializeField] MeshRenderer m_ShootDamageQuad;
        [SerializeField] Material[] m_ShootDamageMaterials;

        [SerializeField] ParticleSystem m_DestroyParticule;
        [SerializeField] Animator m_DestroyAnimation;
        [SerializeField] bool m_IsFriend;
        // RespawnTime ?

        public void TakeDamage(float damage)
        {
            m_MaxHeath -= damage;

            // => côté cible => modifier la texture en fonction de la vie
            // => exemple avec pot on peut mettre un quad devant avec une texture materiau qui évolue (se brise petit à petit).
            // Ou plus simple => une animation avec Sprite(quad) qui évolue en fonction de health en faisant apparaître des fissures et dommages :
            // cela limiterait le nombre de texture pour le même effet 
            //if (collision.relativeVelocity.magnitude > 2)
            //    audioSource.Play(); => Mettre sur other dans script health ou autre
            //m_DamageSound.Play();

            if (m_MaxHeath <= 0)
            {
                // TODO
                // if (m_IsFriend) GameManager.FriendKilled(); // Ecran failed avec message ami et bouton retry + inputblocked 

                // TODO play death animation
                Destroy(gameObject);
            }
            else
            {
                // TODO play m_ShootAnimation + modification material du Quad
            }
        }

    }

}
