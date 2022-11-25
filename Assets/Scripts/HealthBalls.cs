using UnityEngine;

namespace HeroStory
{

    public class HealthBalls : MonoBehaviour
    {
        [SerializeField] float m_Healthballs = 100;
        [SerializeField] ParticleSystem m_Particles;
        [SerializeField] AudioSource m_AudioSource;
        
        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                HeroController.Instance.Health += m_Healthballs;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.GetComponent<Collider>().enabled = false;
                m_Particles.Play();
                m_AudioSource.Play();
                Destroy(gameObject, 5.0f);
            }
        }

    }

}
