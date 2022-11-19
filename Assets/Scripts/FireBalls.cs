using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroStory
{

    public class FireBalls : MonoBehaviour
    {
        [SerializeField] float m_fireballs = 100;
        [SerializeField] ParticleSystem m_Particles;
        
        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                HeroController.Instance.FireBalls += m_fireballs;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.GetComponent<Collider>().enabled = false;
                m_Particles.Play();
                Destroy(gameObject, 5.0f);
            }
        }

    }

}
