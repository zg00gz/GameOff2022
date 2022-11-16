using System;
using UnityEngine;

namespace HeroStory
{
    
    public enum Lang
    {
        EN,
        FR
    }

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableData/Level", order = 1)]
    public class LevelData : ScriptableObject
    {
        [SerializeField]
        private int _LevelID;
        [SerializeField]
        private bool _IsShootEnabled;
        [SerializeField]
        private Vector3[] _SpawnPoints;
        [SerializeField]
        private float[] _Time;
        [SerializeField]
        private int _RunIndex;
        [SerializeField]
        private float[] _RunCupTime;

        public int LevelID => _LevelID;
        public Vector3[] SpawnPoints => _SpawnPoints;
        public float[] Time => _Time;
        public bool IsShootEnabled => _IsShootEnabled;
        public int RunIndex => _RunIndex;
        public float[] RunCupTime => _RunCupTime;


        #region Language

        [Serializable]
        public class EN
        {
            public string GroupLevelName;
            public string LevelName;
            public string GoWord;
            public string EndWord;
        }
        [SerializeField]
        private EN _English;

        [Serializable]
        public class FR
        {
            public string GroupLevelName;
            public string LevelName;
            public string GoWord;
            public string EndWord;
        }
        [SerializeField]
        private FR _French;

        private string _GroupLevelName;
        private string _LevelName;
        private string _GoWord;
        private string _EndWord;

        public string GroupLevelName => _GroupLevelName;
        public string LevelName => _LevelName;
        public string GoWord => _GoWord;
        public string EndWord => _EndWord;

        public void SetLanguage(Lang _Language)
        {
            switch (_Language)
            {
                case Lang.EN:
                    _GroupLevelName = _English.GroupLevelName;
                    _LevelName = _English.LevelName;
                    _GoWord = _English.GoWord;
                    _EndWord = _English.EndWord;
                    break;

                case Lang.FR:
                    _GroupLevelName = _French.GroupLevelName;
                    _LevelName = _French.LevelName;
                    _GoWord = _French.GoWord;
                    _EndWord = _French.EndWord;
                    break;
            }
        }

        #endregion

    }
}