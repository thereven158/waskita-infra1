using System.Collections.Generic;

namespace Agate.Waskita.Responses.Data
{
    [System.Serializable]
    public class LevelProgressData
    {
        public int lastCheckpoint;
        public int currentDay;
        public int tryCount;
        public DayCondition dayCondition;
        public int level;
        public List<int> storedAnswers;

    }
}