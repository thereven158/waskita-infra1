using System;
using Agate.WaskitaInfra1.GameProgress;

namespace Agate.WaskitaInfra1.Server.Responses.Data
{
    [System.Serializable]
    public class ServerGameProgressData : IGameProgressData
    {
        public int maxCompleteLevelIndex;
        public int completionCount;
        public TimeSpan playTime;
        public short MaxCompletedLevelIndex => (short) maxCompleteLevelIndex;
        public uint CompletionCount => (uint) completionCount;
        public double PlayTime => playTime.TotalSeconds;
    }
}