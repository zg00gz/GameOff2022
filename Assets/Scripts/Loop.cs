using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroStory
{

    public class Loop : MonoBehaviour
    {
        
        [SerializeField] string m_Malentendant_FR;
        [SerializeField] string m_Malentendant_EN;
        [SerializeField] TMPro.TextMeshPro m_text;
        [SerializeField] bool m_IsManagedByTrigger; // Play with local trigger - No controller required

        private bool m_IsHearingImpaired;
        private AudioSource m_Loop;

        void Start()
        {
            m_IsHearingImpaired = PlayerLocal.Instance.HeroData.Profile.HearingImpaired;
            m_Loop = GetComponent<AudioSource>();
        }

        public void DisplayText()
        {
            if(m_IsHearingImpaired)
            {
                m_text.text = PlayerLocal.Instance.HeroData.Profile.PlayerLanguage == Lang.EN ? m_Malentendant_EN : m_Malentendant_FR;
                m_text.enabled = true;
            }
        }
        public void HideText()
        {
            m_text.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (m_IsManagedByTrigger) m_Loop.Play();

                if (m_Loop.isPlaying || m_IsManagedByTrigger) DisplayText();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                HideText();
                if (m_IsManagedByTrigger) m_Loop.Stop();
            }
        }
    }

}
