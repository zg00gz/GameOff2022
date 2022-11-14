using UnityEngine;

namespace HeroStory
{

    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] Door m_DoorScript;
        [SerializeField] Material m_InitialMaterial;
        [SerializeField] Material m_CheckedMaterial;

        [SerializeField] bool m_IsChecked = false;
        [SerializeField] ParticleSystem m_Particles;

        void Start()
        {
            m_DoorScript.DoorOpened += OnDoorOpened;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player") && !m_DoorScript.IsChecked)
            {

                if (m_IsChecked)
                {
                    m_IsChecked = false;
                    m_DoorScript.ChangeNbChecked(-1);
                    GetComponent<MeshRenderer>().material = m_InitialMaterial;
                }
                else
                {
                    m_IsChecked = true;
                    m_DoorScript.ChangeNbChecked(1);
                    GetComponent<MeshRenderer>().material = m_CheckedMaterial;
                }
            }

        }

        private void OnDoorOpened()
        {
            if(m_Particles != null)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                m_Particles.Play();
                Destroy(gameObject, 5.0f);
            }
            else
            {
                Destroy(gameObject);
            }
        }

    }

}
