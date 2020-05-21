using System.Collections.Generic;
using Agate.Waskita.Responses;
using Agate.Waskita.Responses.Data;
using Agate.WaskitaInfra1.GameProgress;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using Agate.WaskitaInfra1.PlayerAccount;
using DayCondition = Agate.WaskitaInfra1.LevelProgress.DayCondition;
using LevelData = Agate.WaskitaInfra1.Level.LevelData;

namespace BackendIntegration
{
    public static class DataIntegration
    {
        public static PlayerAccountData AccountData(this LoginResponse response)
        {
            return new PlayerAccountData()
            {
                Username = response.name,
                AuthenticationToken = response.token
            };
        }

        public static IGameProgressData GameData(this Agate.Waskita.Responses.Data.GameProgressData data)
        {
            return new GameProgress()
            {
                CompletionCount = (uint) data.completionCount,
                PlayTime = data.playTime.TotalSeconds,
                MaxCompletedLevelIndex = (short) data.maxCompleteLevelIndex
            };
        }

        public static LevelProgress LevelProgress(this LevelControl levelControl, LevelProgressData data)
        {
            return new LevelProgress()
            {
            };
        }
    }

    public class LevelProgress : ILevelProgressData
    {
        public uint LastCheckpoint { get; set; }
        public uint CurrentDay { get; set; }
        public uint TryCount { get; set; }
        public List<object> Answers { get; set; }
        public DayCondition Condition { get; set; }
        public LevelData Level { get; set; }

        public LevelProgress()
        {
        }

        public LevelProgress(ILevelProgressData data)
        {
            LastCheckpoint = data.LastCheckpoint;
            CurrentDay = data.CurrentDay;
        }
    }

    public class GameProgress : IGameProgressData
    {
        public short MaxCompletedLevelIndex { get; set; }
        public uint CompletionCount { get; set; }
        public double PlayTime { get; set; }
    }
}