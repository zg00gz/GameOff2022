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


        private Animator m_CheckedAnimation;
        private bool m_IsChecked = false;

        void Start()
        {
            //m_CheckedAnimation = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !m_IsChecked)
            {
                m_IsChecked = true;
                m_DoorScript.ChangeNbChecked(1);
                GetComponent<MeshRenderer>().material = m_CheckedMaterial;
                //GetComponent<MeshRenderer>().material = m_CheckedMaterial;
                //GetComponent<MeshRenderer>().sharedMaterial = m_CheckedMaterial;
                //m_CheckedAnimation.SetBool("isChecked", true);
            }
            /*
            var particlesSystem = Instantiate(particles, transform);
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            particlesSystem.Play();
            Destroy(gameObject, particlesSystem.main.duration);
            */
        }
    }

}
