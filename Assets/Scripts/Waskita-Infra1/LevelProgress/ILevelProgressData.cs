using System.Collections.Generic;
using System.Linq;
using Agate.WaskitaInfra1.Level;

namespace Agate.WaskitaInfra1.LevelProgress
{
    public interface ILevelProgressData
    {
        uint LastCheckpoint { get; }
        uint CurrentDay { get; }
        uint TryCount { get; }
        List<object> Answers {get;}
        DayCondition Condition {get;}
        LevelData Level{get;}
        bool Equals(ILevelProgressData other);
    }

    public static class ProgressExtension
    {
        public static bool IsChecklistDone(this ILevelProgressData progressData)
        {
            return progressData.Answers.All(o => o != null);
        }
    }
}