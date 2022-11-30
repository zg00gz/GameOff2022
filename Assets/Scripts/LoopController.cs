using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroStory
{

    public class LoopController : MonoBehaviour
    {
        private Animator m_ControllerAnimation;
        [SerializeField] bool m_IsToggle;
        [SerializeField] bool m_IsToggleOpen;
        [SerializeField] Door m_DoorScript;
        [SerializeField] int m_CheckValue; // 1 for good loop or -1 for bad loop

        [SerializeField] AudioSource m_loop;

        private ParticleSystem m_notes;

        void Start()
        {
            m_ControllerAnimation = GetComponent<Animator>();
            m_notes = GetComponent<ParticleSystem>();
        }

        public void PlayAction()
        {
            //Debug.Log("LoopController - PlayAction");
            if (!m_DoorScript.IsOpened) StartCoroutine(PlayMoveAndClip());
        }
        IEnumerator PlayMoveAndClip()
        {
            HeroController.Instance.IsActionAvailable = false;

            yield return new WaitForSeconds(0.4f); // Fight animation
            HeroController.Instance.GetComponent<FightController>().PlayPunchSounds();

            if (!m_IsToggle && !m_loop.isPlaying)
            {
                //Debug.Log("LoopController - PlayOneShot");
                m_ControllerAnimation.SetTrigger("animControl");
                m_loop.GetComponent<Loop>().DisplayText();
                m_loop.PlayOneShot(m_loop.clip);
                m_notes.Play();
            }
            else if(!m_IsToggleOpen && !m_loop.isPlaying)
            {
                //Debug.Log("LoopController - Play");
                m_ControllerAnimation.SetTrigger("animControl");
                m_loop.GetComponent<Loop>().DisplayText();
                m_loop.Play();
                m_notes.Play();
            }
            else if (m_IsToggleOpen && m_loop.isPlaying)
            {
                //Debug.Log("LoopController - Stop");
                m_ControllerAnimation.SetTrigger("animBack");
            }
        }

        private void CheckAction()
        {
            //Debug.Log("CheckAction");
            if (!m_IsToggle)
            {
                if (m_DoorScript) m_DoorScript.ChangeNbUnlocked(1);
                m_ControllerAnimation.SetTrigger("animBack");
            }
            else
            {
                if(m_DoorScript) m_DoorScript.ChangeNbChecked(m_CheckValue);
                m_IsToggleOpen = true;
            }
            HeroController.Instance.IsActionAvailable = true;
        }
        private void BackAction()
        {
            if (m_IsToggle)
            {
                m_loop.GetComponent<Loop>().HideText();
                m_loop.Stop();
            }
            m_notes.Stop();

            if (m_DoorScript && m_IsToggle) 
                m_DoorScript.ChangeNbChecked(-m_CheckValue);
            else if(m_DoorScript && !m_IsToggle) 
                m_DoorScript.ChangeNbUnlocked(-1);

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
