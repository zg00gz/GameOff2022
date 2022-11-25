using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroStory
{

    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] Door m_DoorScript;
        [SerializeField] Material m_InitialMaterial;
        [SerializeField] Material m_CheckedMaterial;
        [SerializeField] AudioClip m_CheckSound;
        [SerializeField] AudioClip m_DestroySound;
        private AudioSource m_AudioSource;

        [SerializeField] bool m_IsChecked = false;
        [SerializeField] ParticleSystem m_Particles;
        [SerializeField] Light m_CheckedLight;
        [SerializeField] bool m_IsLightEnable;
        [SerializeField] bool m_IsLightInvert; // To manage BadCheckpoint

        void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
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
                    if(m_CheckedLight && m_IsLightEnable) m_CheckedLight.enabled = m_IsLightInvert;
                }
                else
                {
                    m_IsChecked = true;
                    m_DoorScript.ChangeNbChecked(1);
                    GetComponent<MeshRenderer>().material = m_CheckedMaterial;
                    if (m_CheckedLight && m_IsLightEnable) m_CheckedLight.enabled = !m_IsLightInvert;
                }

                m_AudioSource.PlayOneShot(m_CheckSound);
            }
            else if (other.CompareTag("TheBall") && !m_DoorScript.IsChecked)
            {
                if (!m_IsChecked)
                {
                    m_IsChecked = true;
                    m_DoorScript.ChangeNbChecked(1);
                    GetComponent<MeshRenderer>().material = m_CheckedMaterial;
                    if (m_CheckedLight && m_IsLightEnable) m_CheckedLight.enabled = !m_IsLightInvert;
                }

                m_AudioSource.PlayOneShot(m_CheckSound);
            }

        }

        private void OnDoorOpened()
        {
            Destroy(m_CheckedLight.gameObject);

            if (m_Particles != null)
            {
                gameObject.GetComponent<Collider>().enabled = false;
                StartCoroutine(PlayDestroy());
                Destroy(gameObject, 7.0f);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        IEnumerator PlayDestroy()
        {
            yield return new WaitForSeconds(Random.Range(0.0f, 2.0f));
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            m_AudioSource.PlayOneShot(m_DestroySound);
            m_Particles.Play();
        }

    }

}
