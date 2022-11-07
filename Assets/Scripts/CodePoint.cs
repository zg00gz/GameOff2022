using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroStory
{

    public class CodePoint : MonoBehaviour
    {
        [SerializeField] Door m_DoorScript;
        [SerializeField] float m_RotationChecked;
        private bool m_IsChecked;
        private HeroController m_HeroController;

        
        void Start()
        {
            m_HeroController = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroController>();
        }

        public void PlayAction()
        {
           if(!m_DoorScript.IsOpened) StartCoroutine(PlayRotation());
        }

        IEnumerator PlayRotation()
        {
            m_HeroController.IsActionAvailable = false;

            float lerpDuration = 0.2f;
            float timeElapsed = 0;
            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 90, 0);
            
            while (timeElapsed < lerpDuration)
            {
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            transform.rotation = targetRotation;

            Debug.Log("PlayRotation - " + transform.eulerAngles.y + " - " + m_RotationChecked + " - " + m_RotationChecked);
            if (m_IsChecked && Mathf.RoundToInt(transform.eulerAngles.y) != m_RotationChecked) // Values inspector : 0, 90, ,-180, -90 => reals eulerAngles values => 0, 90, 180, 270
            {
                Debug.Log("Unchecked");
                m_IsChecked = false;
                m_DoorScript.ChangeNbChecked(-1);
            }
            else if (!m_IsChecked && Mathf.RoundToInt(transform.eulerAngles.y) == m_RotationChecked)
            {
                Debug.Log("Checked");
                m_IsChecked = true;
                m_DoorScript.ChangeNbChecked(1);
            }

            m_HeroController.IsActionAvailable = true;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                m_HeroController.TargetAction = transform;
                m_HeroController.IsActionAvailable = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                m_HeroController.TargetAction = null;
                m_HeroController.IsActionAvailable = false;
            }
        }
    }

}
