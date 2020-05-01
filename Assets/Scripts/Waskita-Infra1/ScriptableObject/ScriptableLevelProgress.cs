using System.Collections.Generic;
using System.Linq;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using UnityEngine;

namespace Agate.WaskitaInfra1
{
    [CreateAssetMenu(fileName = "LevelProgressData", menuName = "WaskitaInfra1/LevelProgress", order = 0)]
    public class ScriptableLevelProgress : ScriptableObject, ILevelProgressData
    {
        [SerializeField]
        private uint _lastCheckpoint = default;
        [SerializeField]
        private uint _currentDay = default;
        [SerializeField]
        private uint _tryCount = default;
        [SerializeField]
        private List<Object> _answers = default;
        [SerializeField]
        private LevelDataScriptableObject _level = default;
        [SerializeField]
        private DayCondition _dayCondition = default;

        public uint LastCheckpoint => _lastCheckpoint;
        public uint CurrentDay => _currentDay;
        public uint TryCount => _tryCount;
        public List<object> Answers => _answers.Select(o => _answers as object).ToList();
        public DayCondition Condition => _dayCondition;
        public LevelData Level => _level;
        public bool Equals(ILevelProgressData other)
        {
            return base.Equals(other);
        }
    }
}