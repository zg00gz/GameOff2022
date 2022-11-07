using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroStory
{

    public class ControllerPoint : MonoBehaviour
    {
        [SerializeField] Door m_DoorScript;
        private bool m_IsUnlocked;
        private Animator m_ControllerAnimation;

        void Start()
        {
            m_ControllerAnimation = GetComponent<Animator>();
        }

        public void PlayAction()
        {
            if (!m_DoorScript.IsOpened && !m_IsUnlocked)
            {
                HeroController.Instance.IsActionAvailable = false;
                m_ControllerAnimation.SetTrigger("animControl");
            }
        }
        private void CheckAction()
        {
            Debug.Log("CheckAction");
            if (m_DoorScript.IsChecked)
            {
                m_DoorScript.ChangeNbUnlocked(1);
                m_IsUnlocked = true;
                HeroController.Instance.IsActionAvailable = true;
            }
            else
            {
                m_ControllerAnimation.SetTrigger("animBack");
            }
        }
        private void BackAction()
        {
            Debug.Log("DoneAction");
            HeroController.Instance.IsActionAvailable = true;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                HeroController.Instance.TargetAction = transform;
                HeroController.Instance.IsActionAvailable = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                HeroController.Instance.TargetAction = null;
                HeroController.Instance.IsActionAvailable = false;
            }
        }
    }

}