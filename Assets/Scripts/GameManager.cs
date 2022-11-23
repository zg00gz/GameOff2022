using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


namespace HeroStory
{

    public class GameManager : MonoBehaviour
    {
        protected static GameManager s_Instance;
        public static GameManager Instance { get { return s_Instance; } }

        [SerializeField] LevelData m_LevelValues;
        [SerializeField] AudioMixer m_MasterMixer;

        [SerializeField] TMPro.TextMeshPro m_Text_GroupLevelTitle;
        [SerializeField] TMPro.TextMeshPro m_Text_LevelTitle;
        [SerializeField] TMPro.TextMeshPro m_Text_Time;
        [SerializeField] TMPro.TextMeshPro m_Text_Best;
        [SerializeField] TMPro.TextMeshPro[] m_Text_TimeCups;
        [SerializeField] TMPro.TextMeshPro[] m_Text_TimeBest;
        [SerializeField] TMPro.TextMeshProUGUI m_Text_GoWord;
        [SerializeField] TMPro.TextMeshProUGUI m_Text_KillFriend;

        [SerializeField] UI_Level m_UI_Level;
        [SerializeField] Door m_LastDoorScript;
        [SerializeField] int m_FriendKillLimit = 2;


        private float m_LevelStart;

        private float m_TimerStartTime;
        private float m_TimerEndTime;
        public bool IsLevelDone;
        private int FriendKilled;

        private int m_NextStep = 0;
        public  int CurrentStep;

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
            
            m_LevelStart = Time.time;
            LevelValues.SetLanguage(PlayerLocal.Instance.HeroData.Profile.PlayerLanguage);
            m_Text_GroupLevelTitle.text = LevelValues.GroupLevelName;
            m_Text_LevelTitle.text = LevelValues.LevelName;
            m_Text_GoWord.text = LevelValues.GoWord;
            m_Text_KillFriend.text = LevelValues.KillFriend;

            m_MasterMixer.SetFloat("musicVol", PlayerLocal.Instance.HeroData.Profile.MusicVolume);
            m_MasterMixer.SetFloat("soundVol", PlayerLocal.Instance.HeroData.Profile.SoundVolume);

            PlayerLocal.LevelText levelText = PlayerLocal.Instance.GetLevelText();
            m_Text_Time.text = levelText.Time;
            m_Text_Best.text = levelText.BestScore;

            // Text times
            //float timeRun = LevelValues.Time[LevelValues.RunIndex];
            m_Text_TimeCups[0].text = PlayerLocal.Instance.FormatTime(LevelValues.RunCupTime[0]);
            m_Text_TimeCups[1].text = PlayerLocal.Instance.FormatTime(LevelValues.RunCupTime[1]);
            m_Text_TimeCups[2].text = PlayerLocal.Instance.FormatTime(LevelValues.RunCupTime[2]);

            // Text level scores
            PlayerLocal.LevelSaveData levelScores = PlayerLocal.Instance.LoadScore(LevelValues.LevelID);
            string levelScoreTimes = "";
            string levelScorePlayers = "";

            for(var i = 0; i < 5; i++)
            {
                if(levelScores.Scores != null && levelScores.Scores.Count > i)
                {
                    levelScoreTimes += levelScores.Scores[i].DisplayTime + "<br>";
                    levelScorePlayers += levelScores.Scores[i].Player + "<br>";
                }
                else
                {
                    levelScoreTimes += "--:--:---<br>";
                    levelScorePlayers += "--------------------<br>";
                }
            }
            m_Text_TimeBest[0].text = levelScoreTimes;
            m_Text_TimeBest[1].text = levelScorePlayers;

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
                if(m_Paused) ChangePaused();
                SaveTotalPlayTime();
                SceneManager.LoadScene("Hero-Home");
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (m_Paused) ChangePaused();
                SaveTotalPlayTime();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        public void NextStep(float targetRotation)
        {
            Debug.Log("NextStep " + m_NextStep);
            CurrentStep = m_NextStep;
            HeroController.Instance.MoveAuto(LevelValues.SpawnPoints[m_NextStep], targetRotation);

            m_NextStep++;
        }
        
        public void Timer()
        {
            m_TimerStartTime = Time.time;
            string textTime = PlayerLocal.Instance.FormatTime(LevelValues.Time[m_NextStep - 1]);
            m_UI_Level.DisplayTimer(textTime);
            StartCoroutine( UpdateTimer(LevelValues.Time[m_NextStep-1]) );
        }
        IEnumerator UpdateTimer(float timeRemaining)
        {
            while (timeRemaining > 0 && !IsLevelDone)
            {
                yield return new WaitForSeconds(1.0f);
                timeRemaining--;
                string time = PlayerLocal.Instance.FormatTime(timeRemaining);
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
            string displayTime = PlayerLocal.Instance.FormatTime(time, true);
            
            PlayerLocal.Instance.SaveLevel(
                PlayerLocal.Instance.HeroData.Profile.PlayerID,
                LevelValues.LevelID,
                Time.time - m_LevelStart,
                time,
                displayTime
            );
            
            m_UI_Level.ElapsedTimeScreen(time, displayTime);
        }

        public void OnFriendKilled()
        {
            FriendKilled++;

            if(FriendKilled >= m_FriendKillLimit)
            {
                // Failed
                HeroController.Instance.IsInputBlocked = true;
                m_UI_Level.KillFriendScreen();
            }
            else
            {
                StartCoroutine(DisplayTextKillFriend());
            }
        }
        IEnumerator DisplayTextKillFriend()
        {
            m_Text_KillFriend.gameObject.SetActive(true);
            yield return new WaitForSeconds(5);
            m_Text_KillFriend.gameObject.SetActive(false);
        }

        void ChangePaused()
        {
            if (!m_Paused)
            {
                m_Paused = true;
                m_UI_Level.DisplayPause();
                Time.timeScale = 0;
            }
            else
            {
                m_Paused = false;
                m_UI_Level.HidePause();
                Time.timeScale = 1;
            }
        }


        public void GoHome()
        {
            SaveTotalPlayTime();
            SceneManager.LoadScene("Hero-Home");
        }

        public void SaveTotalPlayTime()
        {
            PlayerLocal.Instance.HeroData.Profile.TotalPlayedTime += Time.time - m_LevelStart;
            PlayerLocal.Instance.SaveProfile(PlayerLocal.Instance.HeroData.Profile);
        }
    }
}
