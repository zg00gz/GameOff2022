using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace HeroStory
{

    public class Door : MonoBehaviour
    {
        [SerializeField] int m_NbRequired;
        [SerializeField] int m_UnlockRequired;

        private Animator m_CheckedAnimation;
        private int m_NbChecked = 0;
        private int m_NbUnlocked = 0;

        private GameManager m_GameManager;
        private PlayableDirector m_PlayableDirector;

        void Start()
        {
            m_CheckedAnimation = GetComponent<Animator>();
            m_PlayableDirector = GetComponent<PlayableDirector>();
            m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        public void ChangeNbChecked(int value)
        {
            m_NbChecked += value;
            if (m_NbChecked >= m_NbRequired )
            {
                Debug.Log("Door opened !");
                m_CheckedAnimation.SetBool("isChecked", true);
            }
        }
        public bool ChangeNbUnlocked(int value)
        {
            if (m_NbChecked >= m_NbRequired)
            {
                m_NbUnlocked += value;

                if(m_NbUnlocked >= m_UnlockRequired)
                {
                    Debug.Log("Door opened !");
                    m_CheckedAnimation.SetBool("isChecked", true);
                }
                return true;
            }
            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Level done !");

                m_PlayableDirector.Play();
                m_GameManager.NextStep();
            }
        }
    }
}
