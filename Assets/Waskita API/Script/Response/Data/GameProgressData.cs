using System;
using Agate.WaskitaInfra1.GameProgress;

namespace Agate.Waskita.Responses.Data
{
    [System.Serializable]
    public class GameProgressData : IGameProgressData
    {
        public int maxCompleteLevelIndex;
        public int completionCount;
        public TimeSpan playTime;
        public short MaxCompletedLevelIndex => (short) maxCompleteLevelIndex;
        public uint CompletionCount => (uint) completionCount;
        public double PlayTime => playTime.TotalSeconds;
    }
}