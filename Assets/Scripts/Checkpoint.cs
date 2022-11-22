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
        [SerializeField] Light m_CheckedLight;

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
                    if(m_CheckedLight) m_CheckedLight.enabled = false;
                }
                else
                {
                    m_IsChecked = true;
                    m_DoorScript.ChangeNbChecked(1);
                    GetComponent<MeshRenderer>().material = m_CheckedMaterial;
                    if (m_CheckedLight) m_CheckedLight.enabled = true;
                }
            }
            else if (other.CompareTag("TheBall") && !m_DoorScript.IsChecked)
            {
                if (!m_IsChecked)
                {
                    m_IsChecked = true;
                    m_DoorScript.ChangeNbChecked(1);
                    GetComponent<MeshRenderer>().material = m_CheckedMaterial;
                    if (m_CheckedLight) m_CheckedLight.enabled = true;
                }
            }

        }

        private void OnDoorOpened()
        {
            if (m_CheckedLight) m_CheckedLight.enabled = false;

            if (m_Particles != null)
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
