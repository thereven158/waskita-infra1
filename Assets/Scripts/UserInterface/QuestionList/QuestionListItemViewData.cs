using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;

namespace Agate.WaskitaInfra1.UserInterface.ChecklistList
{
    public struct QuestionListItemViewData
    {
        public IQuestion Item;
        public bool State;
    }

    public static class ProgressExtension
    {
        public static QuestionListItemViewData CheckListViewAt(this ILevelProgressData levelProgressData, int index)
        {
            return new QuestionListItemViewData()
            {
                Item = levelProgressData.Level.Questions[index],
                State = levelProgressData.Answers[index] != null
            };
        }
    }
    public static class EvaluationExtension
    {
        public static QuestionListItemViewData CheckListViewAt(this LevelEvaluationData evaluationData, int index)
        {
            return new QuestionListItemViewData()
            {
                Item = evaluationData.Level.Questions[index],
                State = evaluationData.AnswerEvaluations[index]
            };
        }
    }
}