using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroStory
{

    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] Door m_DoorScript;


        private Animator m_CheckedAnimation;
        private bool m_IsChecked = false;

        void Start()
        {
            m_CheckedAnimation = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !m_IsChecked)
            {
                m_IsChecked = true;
                m_DoorScript.ChangeNbChecked(1);
                m_CheckedAnimation.SetBool("isChecked", true);

            }
        }
    }

}
