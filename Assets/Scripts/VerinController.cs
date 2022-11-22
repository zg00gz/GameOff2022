using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroStory
{

    public class VerinController : MonoBehaviour
    {
        private Animator m_ControllerAnimation;

        [SerializeField] Animator m_VerinAnimation;
        [SerializeField] Verin m_Verin;

        [SerializeField] bool m_IsAuto;
        [SerializeField] bool m_IsToggle;
        [SerializeField] bool m_IsToggleOpen;


        void Start()
        {
            m_ControllerAnimation = GetComponent<Animator>();
        }

        public void PlayAction()
        {
            Debug.Log("VerinController - PlayAction");
            StartCoroutine(PlayMove());
        }
        IEnumerator PlayMove()
        {
            HeroController.Instance.IsActionAvailable = false;

            yield return new WaitForSeconds(0.4f); // Fight animation
            HeroController.Instance.GetComponent<FightController>().PlayPunchSounds();
            m_ControllerAnimation.SetTrigger("animControl");

            if( (!m_IsToggleOpen || m_IsAuto) && m_Verin.IsInteractable)
            {
                Debug.Log("VerinController - Open");
                m_VerinAnimation.SetTrigger("open");
            }
            else if( m_IsToggleOpen && m_Verin.IsInteractable)
            {
                Debug.Log("VerinController - Close");
                m_VerinAnimation.SetTrigger("close");
            }
        }

        private void CheckAction()
        {
            Debug.Log("CheckAction");
            m_ControllerAnimation.SetTrigger("animBack");

            if (m_IsAuto)
            {
                Debug.Log("VerinController - Close AUTO");
                m_VerinAnimation.SetTrigger("close");
            }
            else
            {
                m_IsToggleOpen = true;
            }
            HeroController.Instance.IsActionAvailable = true;
        }
        private void BackAction()
        {
            m_IsToggleOpen = false;
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
