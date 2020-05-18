using System;

namespace Agate.Waskita.Responses.Data
{
    [System.Serializable]
    public class GameProgressData
    {
        public int maxCompleteLevelIndex;
        public int completionCount;
        public TimeSpan playTime;
    }
}