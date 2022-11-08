using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HeroStory
{

    public class GameManager : MonoBehaviour
    {
        protected static GameManager s_Instance;
        public static GameManager Instance { get { return s_Instance; } }

        [SerializeField] float m_LerpSpeed = 4f;
        [SerializeField] LevelData m_LevelValues;
        [SerializeField] TMPro.TextMeshPro m_LevelTitle;

        private int m_LevelStep = 0;
        private bool m_Paused;

        public LevelData LevelValues => m_LevelValues;

        void Start()
        {
            if (s_Instance != null)
            {
                Debug.Log("GameManager - gameobject destroyed");
                Destroy(gameObject);
                return;
            }
            s_Instance = this;

            LevelValues.SetLanguage(Lang.FR);
            HeroController.Instance.IsShootEnabled = LevelValues.IsShootEnabled;
            m_LevelTitle.text = LevelValues.LevelName;

            StartCoroutine(PlayIntro());
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                ChangePaused();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                SceneManager.LoadScene("Hero-Home");
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
            HeroController.Instance.IsInputBlocked = true;

            Vector3 startPos = HeroController.Instance.transform.position;
            Debug.Log(startPos.x +" "+ startPos.y + " " + startPos.z);
            Vector3 endPos = LevelValues.SpawnPoints[m_LevelStep];

            float journeyLength = Vector3.Distance(startPos, endPos);
            float startTime = Time.time;
            float distanceCovered = (Time.time - startTime) * m_LerpSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            //HeroController.Instance.GetComponent<Animator>().SetFloat("Speed_Multiplier", 0.5f);
            while (fractionOfJourney < 1)
            {
                distanceCovered = (Time.time - startTime) * m_LerpSpeed;
                fractionOfJourney = distanceCovered / journeyLength;
                //HeroController.Instance.MoveAuto(endPos, fractionOfJourney);
                // https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html
                /*
                 transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
                 timeElapsed += Time.deltaTime;
                 */

                HeroController.Instance.transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);
                var rotation = Quaternion.RotateTowards(HeroController.Instance.transform.rotation, Quaternion.Euler(new Vector3(0, 90, 0)), 50f * Time.deltaTime);

                //m_Rigidbody.MoveRotation(rotation);
                //TODO LookAt
                HeroController.Instance.transform.rotation = rotation; // rotation
                yield return null;
            }
            //HeroController.Instance.GetComponent<Animator>().SetFloat("Speed_Multiplier", 1.0f);

            // TODO 3, 2, 1 Go !!!!! + animation Hero qui regarde le joueur
            int remainingTime = 3;
            //timeText.text = "Time: " + remainingTime;
            while (HeroController.Instance.IsInputBlocked)
            {
                Debug.Log(remainingTime);
                yield return new WaitForSeconds(1);
                remainingTime--;
                //timeText.text = "Time: " + remainingTime;
                //if (remainingTime == 1) Text Go !!!!! 
                if (remainingTime <= 0) HeroController.Instance.IsInputBlocked = false;
            }
        }

        void ChangePaused()
        {
            if (!m_Paused)
            {
                m_Paused = true;
                Debug.Log("Game paused !");
                //m_PauseScreen.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                m_Paused = false;
                Debug.Log("Game unpaused !");
                //m_PauseScreen.SetActive(false);
                Time.timeScale = 1;
            }
        }

    }
}
