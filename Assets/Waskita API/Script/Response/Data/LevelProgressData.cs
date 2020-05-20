using System.Collections.Generic;
using Agate.WaskitaInfra1.LevelProgress;

namespace Agate.Waskita.Responses.Data
{
    [System.Serializable]
    public class LevelProgressData
    {
        public int lastCheckpoint;
        public int currentDay;
        public int tryCount;
        public List<QuizAnswer> answer;
        public DayCondition dayCondition;
        public LevelData level;
        
    }
}