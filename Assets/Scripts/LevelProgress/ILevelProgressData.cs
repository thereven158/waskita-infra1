using System.Collections.Generic;
using Agate.WaskitaInfra1.Level;

namespace Agate.WaskitaInfra1.LevelProgress
{
    public interface ILevelProgressData
    {
        uint LastCheckpoint { get; }
        uint CurrentDay { get; }
        uint TryCount { get; }
        List<QuizAnswer> Answers {get;}
        DayCondition Condition {get;}
        LevelData Level{get;}
        bool Equals(ILevelProgressData other);
    }
}