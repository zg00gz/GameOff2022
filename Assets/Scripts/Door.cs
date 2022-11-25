using System;
using UnityEngine;
using UnityEngine.Playables;

namespace HeroStory
{

    public class Door : MonoBehaviour
    {
        [SerializeField] int m_NbRequired;
        [SerializeField] int m_UnlockRequired;
        [SerializeField] PlayableDirector m_PlayableDirector;

        [SerializeField] AudioClip m_OpenSound;
        [SerializeField] AudioClip m_CloseSound;
        private AudioSource m_AudioSource;
        private Animator m_CheckedAnimation;
        [SerializeField] int m_NbChecked;
        [SerializeField] int m_NbUnlocked;

        [SerializeField] bool m_IsAlwaysOpen;

        public bool IsChecked;
        public bool IsOpened;
        public event Action DoorChecked;
        public event Action DoorOpened;
        public event Action DoorExit;


        void Start()
        {
            m_CheckedAnimation = GetComponent<Animator>();
            m_AudioSource = GetComponent<AudioSource>();
        }

        public void ChangeNbChecked(int value)
        {
            m_NbChecked += value;
            //Debug.Log("m_NbChecked = " + m_NbChecked);

            if (m_NbChecked >= m_NbRequired)
            {
                IsChecked = true;
                DoorChecked?.Invoke();

                if (m_NbUnlocked >= m_UnlockRequired)
                {
                    SetDoorOpened();
                }
            }
        }
        public void ChangeNbUnlocked(int value)
        {
            if (m_NbChecked >= m_NbRequired)
            {
                m_NbUnlocked += value;
                //Debug.Log("m_NbUnlocked = " + m_NbUnlocked);

                if (m_NbUnlocked >= m_UnlockRequired)
                {
                    SetDoorOpened();
                }
            }
        }

        private void SetDoorOpened()
        {
            Debug.Log("Door opened !");
            IsOpened = true;
            if (m_IsAlwaysOpen) GetComponent<Collider>().enabled = false;

            m_CheckedAnimation.SetTrigger("open");
            DoorOpened?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Door closed !");
                gameObject.GetComponent<Collider>().isTrigger = false;
                m_CheckedAnimation.SetTrigger("close");

                if(m_PlayableDirector != null) m_PlayableDirector.Play();
                DoorExit?.Invoke();
            }
        }

        // Animation event
        private void PlaySound()
        {
            if(m_AudioSource)
            {
                if (m_CheckedAnimation.GetCurrentAnimatorStateInfo(0).speed >= 0)
                    m_AudioSource.PlayOneShot(m_OpenSound);
                else
                    m_AudioSource.PlayOneShot(m_CloseSound);
            }
            
        }

    }

}
