using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroStory
{

    public class LevelManager : MonoBehaviour
    {
        [SerializeField] GameObject[] m_Steps;
        [SerializeField] Door[] m_Doors;
        [SerializeField] Animator PlatformAnimation;

        [SerializeField] CarotteController m_CarotteController;
        private bool m_IsStepRunning;
        private int m_CurrentStep;


        void Start()
        {
            m_Doors[0].DoorOpened += Step0_DoorOpened;
            m_Doors[1].DoorOpened += Step1_DoorOpened;
        }


        #region step0
        public void Step0()
        {
            m_IsStepRunning = true;
            m_CurrentStep = 0;
            m_Steps[0].SetActive(true);
            StartCoroutine(PlayCarotteShootPlayer());
        }
        private void Step0_DoorOpened()
        {
            m_IsStepRunning = false;
            StartCoroutine(BeforeStep1());
        }
        IEnumerator BeforeStep1()
        {
            m_IsStepRunning = true;
            StartCoroutine(PlayCarotteShootAll());

            yield return new WaitUntil( () => m_CarotteController.Health < 1300);
            m_IsStepRunning = false;
            PlatformAnimation.SetTrigger("up");
            Step1();
        }

        #endregion

        #region step1

        public void Step1()
        {
            m_IsStepRunning = true;
            m_CurrentStep = 1;
            m_Steps[1].SetActive(true);
            StartCoroutine(PlayCarotteShootPlayer());
        }
        private void Step1_DoorOpened()
        {
            m_IsStepRunning = false;
            StartCoroutine(BeforeStep2());
        }
        IEnumerator BeforeStep2()
        {
            m_IsStepRunning = true;
            StartCoroutine(PlayCarotteShootAll());

            yield return new WaitUntil(() => m_CarotteController.Health < 1000);
            m_IsStepRunning = false;
            PlatformAnimation.SetTrigger("up");
            //Step2();
        }

        #endregion

        IEnumerator PlayCarotteShootPlayer()
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 3.0f));

            m_CarotteController.ShootPlayer();
            yield return new WaitForSeconds(Random.Range(1.5f, 3.0f));
            m_CarotteController.StopShooting();

            if (m_IsStepRunning)
                StartCoroutine(PlayCarotteShootPlayer());
        }

        IEnumerator PlayCarotteShootAll()
        {
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));

            m_CarotteController.ShootAll();
            yield return new WaitForSeconds(10.0f);

            if (m_IsStepRunning)
                StartCoroutine(PlayCarotteShootPlayer());
        }

    }


}
