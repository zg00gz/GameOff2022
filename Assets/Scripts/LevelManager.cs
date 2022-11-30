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
        [SerializeField] AudioSource[] m_AudioSources;

        [SerializeField] CarotteController m_CarotteController;
        [SerializeField] Goose[] m_GoosesGardian;

        private bool m_IsStepRunning;
        private bool m_IsBeforeStepRunning;
        private bool m_IsCoroutineRunningPlayer;
        private bool m_IsCoroutineRunningAll;
        private bool m_IsCoroutineRunningSky;

        private bool m_IsGameOver;


        void Start()
        {
            m_Doors[0].DoorOpened += Step0_DoorOpened;
            m_Doors[1].DoorOpened += Step1_DoorOpened;
            m_Doors[2].DoorOpened += Step2_DoorOpened;
            m_Doors[3].DoorOpened += Step3_DoorOpened;
        }

        public void PlayAudioSources()
        {
            m_AudioSources[0].Play();
        }
        IEnumerator ChangeMusicLoop(int index)
        {
            m_AudioSources[index - 1].loop = false;
            yield return new WaitWhile(() => m_AudioSources[index-1].isPlaying);
            m_AudioSources[index].Play();
            //Debug.Log("Play " + index);
        }

        IEnumerator StopMusicLoop(int index)
        {
            m_AudioSources[index].loop = false;
            yield return new WaitWhile(() => m_AudioSources[index].isPlaying);
            m_AudioSources[index].Stop();
            //Debug.Log("Stop " + index);
        }


        #region step0
        public void Step0()
        {
            m_IsStepRunning = true;
            m_Steps[0].SetActive(true);
        }
        private void Step0_DoorOpened()
        {
            m_IsStepRunning = false;
            PlatformAnimation.SetBool("isUp", false);
            StartCoroutine(BeforeStep1());
        }
        IEnumerator BeforeStep1()
        {
            m_IsBeforeStepRunning = true;
            StartCoroutine(PlayCarotteShootAll());

            yield return new WaitUntil( () => m_CarotteController.Health <= 1400);
            Destroy(m_Steps[0]);
            m_IsBeforeStepRunning = false;
            yield return new WaitUntil(() => !m_IsCoroutineRunningAll);

            GameManager.Instance.NextStep(0);
            PlatformAnimation.SetBool("isUp",true);
            yield return new WaitUntil( () => !HeroController.Instance.IsInputBlocked);
            Step1();
        }

        #endregion

        #region step1

        public void Step1()
        {
            m_IsStepRunning = true;
            m_Steps[1].SetActive(true);
            StartCoroutine(PlayCarotteShootPlayer(4.0f));
        }
        private void Step1_DoorOpened()
        {
            m_IsStepRunning = false;
            PlatformAnimation.SetBool("isUp", false);
            StartCoroutine(BeforeStep2());
        }
        IEnumerator BeforeStep2()
        {
            yield return new WaitUntil(() => !m_IsCoroutineRunningPlayer);

            m_IsBeforeStepRunning = true;
            StartCoroutine(PlayCarotteShootPlayer());

            yield return new WaitUntil(() => m_CarotteController.Health <= 1100);
            Destroy(m_Steps[1]);
            m_IsBeforeStepRunning = false;
            yield return new WaitUntil(() => !m_IsCoroutineRunningPlayer);

            StartCoroutine(ChangeMusicLoop(1));

            GameManager.Instance.NextStep(0);
            PlatformAnimation.SetBool("isUp", true);
            yield return new WaitUntil(() => !HeroController.Instance.IsInputBlocked);

            Step2();
        }

        #endregion

        #region step2

        public void Step2()
        {
            m_IsStepRunning = true;
            m_Steps[2].SetActive(true);
            StartCoroutine(PlayCarotteShootSky(4.0f));
        }
        private void Step2_DoorOpened()
        {
            m_IsStepRunning = false;
            PlatformAnimation.SetBool("isUp", false);
            StartCoroutine(BeforeStep3());
        }
        IEnumerator BeforeStep3()
        {
            yield return new WaitUntil(() => !m_IsCoroutineRunningSky);

            m_IsBeforeStepRunning = true;
            StartCoroutine(PlayCarotteShootSky());

            yield return new WaitUntil(() => m_CarotteController.Health <= 800);
            Destroy(m_Steps[2]);
            m_IsBeforeStepRunning = false;
            yield return new WaitUntil(() => !m_IsCoroutineRunningSky);

            StartCoroutine(ChangeMusicLoop(2));

            GameManager.Instance.NextStep(0);
            PlatformAnimation.SetBool("isUp", true);
            yield return new WaitUntil(() => !HeroController.Instance.IsInputBlocked);

            Step3();
        }

        #endregion

        #region step3

        public void Step3()
        {
            m_IsStepRunning = true;
            m_Steps[3].SetActive(true);
            StartCoroutine(PlayCarotteShootAll());
        }
        private void Step3_DoorOpened()
        {
            m_IsStepRunning = false;
            PlatformAnimation.SetBool("isUp", false);
            StartCoroutine(BeforeStep4());
        }
        IEnumerator BeforeStep4()
        {
            yield return new WaitUntil(() => !m_IsCoroutineRunningAll);

            m_IsBeforeStepRunning = true;
            StartCoroutine(PlayCarotteShootAll());

            yield return new WaitUntil(() => m_CarotteController.Health <= 500);
            Destroy(m_Steps[3]);
            m_IsBeforeStepRunning = false;
            yield return new WaitUntil(() => !m_IsCoroutineRunningAll);

            StartCoroutine(ChangeMusicLoop(3));

            GameObject.Find("ColliderGoose").GetComponent<Collider>().isTrigger = true;
            m_GoosesGardian[0].WalkToPlayer();
            m_GoosesGardian[1].WalkToPlayer();

            GameManager.Instance.NextStep(0);
            PlatformAnimation.SetBool("isUp", true);
            yield return new WaitUntil(() => !HeroController.Instance.IsInputBlocked);

            Step4();
        }

        #endregion


        #region step4

        public void Step4()
        {
            StartCoroutine(BeforeStepEnd());
        }
        IEnumerator BeforeStepEnd()
        {
            m_IsStepRunning = false;
            m_IsBeforeStepRunning = true;

            yield return new WaitUntil(() => m_GoosesGardian[0] == null);
            yield return new WaitUntil(() => m_GoosesGardian[1] == null);
            GameObject.Find("ColliderGoose").GetComponent<Collider>().isTrigger = false;

            StartCoroutine(ChangeMusicLoop(4));
            yield return new WaitForSeconds(10.0f);

            PlatformAnimation.SetBool("isUp", false);
            StartCoroutine(PlayCarotteShootPlayer());

            yield return new WaitUntil(() => m_CarotteController.Health <= 0);
            m_IsGameOver = true;
            m_IsBeforeStepRunning = false;
            m_CarotteController.StopShooting();
            StopCoroutine(PlayCarotteShootPlayer());
            yield return new WaitUntil(() => !m_IsCoroutineRunningPlayer);
            m_CarotteController.StopShooting();

            //StartCoroutine(StopMusicLoop(4));

            GameManager.Instance.GameOver();
            PlatformAnimation.SetBool("isUp", true);

            StepEnd();
        }

        public void StepEnd()
        {
            m_Steps[4].SetActive(true);
        }

        #endregion

        IEnumerator PlayCarotteShootPlayer(float initialDelay = 0.0f)
        {
            if(!m_IsGameOver)
            {
                //Debug.Log("PlayCarotteShootPlayer");
                yield return new WaitUntil(() => !m_IsCoroutineRunningPlayer);
                m_IsCoroutineRunningPlayer = true;
                //Debug.Log("PlayCarotteShootPlayer true");

                yield return new WaitForSeconds(initialDelay);

                yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));

                m_CarotteController.ShootPlayer();
                yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
                m_CarotteController.StopShooting();

                m_IsCoroutineRunningPlayer = false;
                //Debug.Log("PlayCarotteShootPlayer false");
                if (m_IsStepRunning || m_IsBeforeStepRunning)
                    StartCoroutine(PlayCarotteShootPlayer());
            }
        }

        IEnumerator PlayCarotteShootAll()
        {
            //Debug.Log("PlayCarotteShootAll");
            yield return new WaitUntil(() => !m_IsCoroutineRunningAll);
            m_IsCoroutineRunningAll = true;

            yield return new WaitForSeconds(Random.Range(0.5f, 2.0f));

            m_CarotteController.ShootAll();
            yield return new WaitForSeconds(4.0f);

            m_IsCoroutineRunningAll = false;
            if (m_IsStepRunning || m_IsBeforeStepRunning)
                StartCoroutine(PlayCarotteShootAll());
        }

        IEnumerator PlayCarotteShootSky(float initialDelay = 0.0f)
        {
            //Debug.Log("PlayCarotteShootSky");
            yield return new WaitUntil(() => !m_IsCoroutineRunningSky);
            m_IsCoroutineRunningSky = true;
            //Debug.Log("PlayCarotteShootSky true");

            yield return new WaitForSeconds(initialDelay);

            yield return new WaitForSeconds(Random.Range(3.0f, 5.0f));
            m_CarotteController.ShootTheSky();

            m_IsCoroutineRunningSky = false;
            //Debug.Log("PlayCarotteShootSky false");
            if (m_IsStepRunning || m_IsBeforeStepRunning)
                StartCoroutine(PlayCarotteShootSky());
        }
    }


}
