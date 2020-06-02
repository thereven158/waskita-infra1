using Agate.WaskitaInfra1.Level;
using System.Collections.Generic;

namespace Agate.WaskitaInfra1.LevelProgress
{
    internal class LevelProgressData : ILevelProgressData
    {
        public LevelProgressData(ILevelProgressData data)
        {
            CurrentDay = data.CurrentDay;
            LastCheckpoint = data.LastCheckpoint;
            TryCount = data.TryCount;
            Answers = new List<object>(data.Answers);
            Condition = data.Condition;
            Level = data.Level;
        }

        public LevelProgressData(LevelData level)
        {
            CurrentDay = 1;
            TryCount = 1;
            LastCheckpoint = 0;
            Answers = new List<object>(level.Questions.Count);
            Condition = new DayCondition();
            Level = level;
            for (int i = 0; i < level.Questions.Count; i++)
                Answers.Add(null);
        }

        public uint LastCheckpoint { get; set; }
        public uint CurrentDay { get; set; }
        public uint TryCount { get; set; }
        public List<object> Answers { get; }
        public DayCondition Condition { get; }
        public LevelData Level { get; }

    }
}