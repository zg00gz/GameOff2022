using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroStory
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField] HeroController m_HeroController;
        [SerializeField] float m_LerpSpeed = 4f;
        [SerializeField] LevelData LevelValues;
        [SerializeField] TMPro.TextMeshPro LevelTitle;

        private int m_LevelStep = 0;
        private bool m_Paused;

        void Start()
        {
            LevelValues.SetLanguage(Lang.FR);
            Debug.Log(LevelValues.LevelName);
            LevelTitle.text = LevelValues.LevelName;

            StartCoroutine(PlayIntro());
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                ChangePaused();
            }
        }

        public void NextStep()
        {
            m_LevelStep++;

            if (LevelValues.SpawnPoints.Length > m_LevelStep)
            {
                StartCoroutine(PlayIntro());
            }
            else
            {
                Debug.Log("Fin du niveau");
            }
        }

        IEnumerator PlayIntro()
        {
            m_HeroController.IsInputBlocked = true;

            Vector3 startPos = m_HeroController.transform.position;
            Debug.Log(startPos.x +" "+ startPos.y + " " + startPos.z);
            Vector3 endPos = LevelValues.SpawnPoints[m_LevelStep];

            float journeyLength = Vector3.Distance(startPos, endPos);
            float startTime = Time.time;
            float distanceCovered = (Time.time - startTime) * m_LerpSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            while (fractionOfJourney < 1)
            {
                distanceCovered = (Time.time - startTime) * m_LerpSpeed;
                fractionOfJourney = distanceCovered / journeyLength;
                m_HeroController.transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);
                yield return null;
            }

            int remainingTime = 3;
            while (m_HeroController.IsInputBlocked)
            {
                Debug.Log(remainingTime);
                yield return new WaitForSeconds(1);
                remainingTime--;
                if (remainingTime <= 0) m_HeroController.IsInputBlocked = false;
            }
        }

        void ChangePaused()
        {
            if (!m_Paused)
            {
                m_Paused = true;
                Debug.Log("Game paused !");
                Time.timeScale = 0;
            }
            else
            {
                m_Paused = false;
                Debug.Log("Game unpaused !");
                Time.timeScale = 1;
            }
        }

    }
}
