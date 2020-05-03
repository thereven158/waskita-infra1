using System.Collections.Generic;
using Agate.WaskitaInfra1.Level;

namespace Agate.WaskitaInfra1.LevelProgress
{
    public class LevelEvaluationData
    {
        public LevelEvaluationData(ILevelProgressData progressData)
        {
            Level = progressData.Level;
            AnswerEvaluations = new List<bool>();
            DayFinished = progressData.CurrentDay;
            TryCount = progressData.TryCount;
            for (int i = 0; i < progressData.Level.Quizzes.Count; i++)
                AnswerEvaluations.Add(progressData.Level.Quizzes[i].Quiz.IsCorrect(progressData.Answers[i]));
        }
        public LevelData Level;
        public uint TryCount;
        public List<bool> AnswerEvaluations;
        public uint DayFinished;

    }
}