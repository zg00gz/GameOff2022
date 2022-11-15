using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;


namespace HeroStory
{

    public class PlayerLocal : MonoBehaviour
    {
        public static PlayerLocal Instance;

        // Settings
        private int _playerID;
        public string PlayerName = "Hero";
        public Lang PlayerLanguage = Lang.EN;

        #region Language

        public class LevelText
        {
            public string Time;
            public string BestScore;
        }
        public LevelText GetLevelText(Lang _Language)
        {
            var levelText = new LevelText();

            switch (_Language)
            {
                case Lang.EN:
                    levelText.Time = "Time";
                    levelText.BestScore = "Best scores";
                    break;

                case Lang.FR:
                    levelText.Time = "Temps";
                    levelText.BestScore = "Meilleurs scores";
                    break;
            }

            return levelText;
        }

        #endregion

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("PlayerLocal instance destroy");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        #region Player 

        // P_IDplayer.json
        [System.Serializable]
        public class SaveData
        {
            public string PlayerName;
            public float TotalPlayedTime;
            public List<Score> Scores;
        }
        [System.Serializable]
        public class Score
        {
            public int LevelID;
            public float Time;
            public string DisplayTime;
            public DateTime Date;
        }
        
        public void Save(int levelID, float playedTime, float time, string displayTime)
        {
            Debug.Log("SaveScore");
            SaveData data = Load(levelID);
            data.Scores.Add(new Score()
            {
                LevelID = levelID,
                Time = time,
                DisplayTime = displayTime,
                Date = DateTime.Now
            });

            //data.Scores = data.Scores.OrderByDescending(s => s.Score).Take(10).ToList();
            string json = JsonUtility.ToJson(data);

            File.WriteAllText(Application.persistentDataPath + "/P"+ _playerID +".json", json);
        }

        public SaveData Load(int levelID)
        {
            string path = Application.persistentDataPath + "/savescores.json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                SaveData data = JsonUtility.FromJson<SaveData>(json);

                return data ?? new SaveData() { Scores = new List<Score>() };
            }
            return new SaveData() { Scores = new List<Score>() };
        }

        #endregion

        #region Levels

        // L_LevelID.json
        [System.Serializable]
        public class LevelSaveData
        {
            public List<LevelScore> scores;
        }

        [System.Serializable]
        public class LevelScore
        {
            public string player;
            public float time;
            public string displayTime;
        }

        public void SaveLevelScore(int levelID, float time, string displayTime)
        {
            Debug.Log("SaveLevelScore");
            LevelSaveData data = LoadScore();
            data.scores.Add(new LevelScore()
            {
                player = PlayerName,
                time = time, 
                displayTime = displayTime
            });
            data.scores = data.scores.OrderBy(s => s.time).Take(5).ToList();

            string json = JsonUtility.ToJson(data);

            File.WriteAllText(Application.persistentDataPath + "/L_"+ levelID +".json", json);
        }

        public LevelSaveData LoadScore()
        {
            string path = Application.persistentDataPath + "/savescores.json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                LevelSaveData data = JsonUtility.FromJson<LevelSaveData>(json);

                return data ?? new LevelSaveData() { scores = new List<LevelScore>() };
            }
            return new LevelSaveData() { scores = new List<LevelScore>() };
        }

        #endregion

        /*
        public string FormatTime(float timeToDisplay)
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            float milliSeconds = (timeToDisplay % 1) * 1000;
            return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds);
        }
        */

    }

}
