using System;
using System.Collections.Generic;
using System.Linq;
using Agate.WaskitaInfra1.Level;

namespace Agate.WaskitaInfra1.LevelProgress
{
    public interface ILevelProgressData: IEquatable<ILevelProgressData>
    {
        uint LastCheckpoint { get; }
        uint CurrentDay { get; }
        uint TryCount { get; }
        List<object> Answers { get; }
        DayCondition Condition { get; }
        LevelData Level { get; }
    }

    public static class ProgressExtension
    {
        public static bool IsChecklistDone(this ILevelProgressData progressData)
        {
            return progressData.Answers.All(o => o != null);
        }

        public static object AnswerOf(this ILevelProgressData progressData, IQuestion question)
        {
            int questionIndex = progressData.Level.Questions.IndexOf(question);
            return questionIndex < 0 ? default : progressData.Answers[questionIndex];
        }
    }
}