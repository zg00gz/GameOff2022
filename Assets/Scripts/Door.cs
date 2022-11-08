using UnityEngine;
using UnityEngine.Playables;

namespace HeroStory
{

    public class Door : MonoBehaviour
    {
        [SerializeField] int m_NbRequired;
        [SerializeField] int m_UnlockRequired;

        private Animator m_CheckedAnimation;
        private int m_NbChecked = 0;
        private int m_NbUnlocked = 0;

        private PlayableDirector m_PlayableDirector;

        public bool IsChecked;
        public bool IsOpened;

        void Start()
        {
            m_CheckedAnimation = GetComponent<Animator>();
            m_PlayableDirector = GetComponent<PlayableDirector>();
        }

        public void ChangeNbChecked(int value)
        {
            m_NbChecked += value;
            if (m_NbChecked >= m_NbRequired)
            {
                IsChecked = true;
                if(m_NbUnlocked >= m_UnlockRequired)
                {
                    Debug.Log("Door opened !");
                    m_CheckedAnimation.SetBool("isChecked", true);
                }
            }
            Debug.Log("m_NbChecked" + m_NbChecked);
        }
        public void ChangeNbUnlocked(int value)
        {
            if (m_NbChecked >= m_NbRequired)
            {
                m_NbUnlocked += value;
                Debug.Log("m_NbUnlocked" + m_NbUnlocked);
                if (m_NbUnlocked >= m_UnlockRequired)
                {
                    Debug.Log("Door opened !");
                    IsOpened = true;
                    m_CheckedAnimation.SetBool("isChecked", true);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Level done !");
                gameObject.GetComponent<Collider>().isTrigger = false;

                m_PlayableDirector.Play();
                GameManager.Instance.NextStep();
            }
        }

    }

}
