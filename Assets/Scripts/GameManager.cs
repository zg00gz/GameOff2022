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
        [SerializeField] TMPro.TextMeshPro m_GroupLevelTitle;
        [SerializeField] TMPro.TextMeshPro m_LevelTitle;
        [SerializeField] TMPro.TextMeshProUGUI m_GoWord;
        [SerializeField] UI_Level m_UI_Level;
        [SerializeField] Door m_LastDoorScript;

        private float m_TimerStartTime;
        private float m_TimerEndTime;
        public bool IsLevelDone;

        private int m_NextStep = 0;
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
            m_GroupLevelTitle.text = LevelValues.GroupLevelName;
            m_LevelTitle.text = LevelValues.LevelName;
            m_GoWord.text = LevelValues.GoWord;

            m_LastDoorScript.DoorChecked += OnLastDoorChecked;
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

        public void NextStep(float rotation)
        {
            if (LevelValues.SpawnPoints.Length > m_NextStep)
            {
                StartCoroutine(MoveToSpawnPoint(rotation));
            }
            else
            {
                Debug.Log("Fin du niveau");
            }
            m_NextStep++;
        }
        
        IEnumerator MoveToSpawnPoint(float rotation)
        {
            HeroController.Instance.IsInputBlocked = true;

            Vector3 startPos = HeroController.Instance.transform.position;
            //Debug.Log(startPos.x +" "+ startPos.y + " " + startPos.z);
            Vector3 endPos = LevelValues.SpawnPoints[m_NextStep];

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
                var rotationToward = Quaternion.RotateTowards(HeroController.Instance.transform.rotation, Quaternion.Euler(new Vector3(0, rotation, 0)), 50f * Time.deltaTime);

                //m_Rigidbody.MoveRotation(rotation);
                //TODO LookAt
                HeroController.Instance.transform.rotation = rotationToward; // rotation
                yield return null;
            }
            //HeroController.Instance.GetComponent<Animator>().SetFloat("Speed_Multiplier", 1.0f);
            HeroController.Instance.IsInputBlocked = false;

            /*
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
            */
        }
        
        public void Timer()
        {
            m_TimerStartTime = Time.time;
            string textTime = FormatTime(LevelValues.Time[m_NextStep - 1]);
            Debug.Log(textTime);
            m_UI_Level.DisplayTimer(textTime);
            StartCoroutine( UpdateTimer(LevelValues.Time[m_NextStep-1]) );
        }
        IEnumerator UpdateTimer(float timeRemaining)
        {
            while (timeRemaining > 0 && !IsLevelDone)
            {
                yield return new WaitForSeconds(1.0f);
                timeRemaining--;
                string time = FormatTime(timeRemaining);
                m_UI_Level.UpdateTimer(time);
            }

            if(!IsLevelDone)
            {
                // Failed
                m_UI_Level.ElapsedTimeScreen();
                HeroController.Instance.IsInputBlocked = true;
            }
        }
        private void OnLastDoorChecked()
        {
            m_TimerEndTime = Time.time;
            IsLevelDone = true;

            float time = m_TimerEndTime - m_TimerStartTime; // TODO tester avec Pause ?
            string displayTime = FormatTime(time, true);
            // TODO Persistent data : enregistrer la valeur texte + valeur float
            // dans un objet joueur (pour permettre multijoueur data, ou un fichier par joueur ?) => un fichier serait mieux
            // 1 fichier par user et  fichier global best score par niveau => si pas de nom renseign� => par d�faut : The player
            m_UI_Level.ElapsedTimeScreen(displayTime);
        }

        public string FormatTime(float timeToDisplay, bool isWithMs = false)
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            if(!isWithMs)
            {
                return string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                float milliSeconds = (timeToDisplay % 1) * 1000;
                return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds);
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
