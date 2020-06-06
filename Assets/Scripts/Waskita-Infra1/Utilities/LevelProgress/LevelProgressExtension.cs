using Agate.WaskitaInfra1.LevelProgress;
using Agate.WaskitaInfra1.UserInterface.LevelState;
using Agate.WaskitaInfra1.UserInterface.QuestionList;
using System.Collections.Generic;

namespace Agate.WaskitaInfra1.Utilities
{
    public static class LevelProgressExtension
    {
        public static LevelState State(this ILevelProgressData progressData)
        {
            LevelState state = progressData.Level.State();
            state.Weather = progressData.Condition._weather;


            state.WindStrength = progressData.Condition._windStrength;
            return state;
        }

        private static QuestionListItemViewData QuestionListItemViewAt(this ILevelProgressData levelProgressData,
            int index)
        {
            return new QuestionListItemViewData()
            {
                Item = levelProgressData.Level.Questions[index],
                State = levelProgressData.Answers[index] != null
            };
        }

        private static IEnumerable<QuestionListItemViewData> QuestionListItemViewDatas(
            this ILevelProgressData levelProgressData)
        {
            List<QuestionListItemViewData> viewDatas = new List<QuestionListItemViewData>();
            for (int i = 0; i < levelProgressData.Level.Questions.Count; i++)
                viewDatas.Add(levelProgressData.QuestionListItemViewAt(i));
            return viewDatas;
        }

        public static IQuestionListViewData QuestionListViewData(this ILevelProgressData progressData)
        {
            return new QuestionListViewData()
            {
                ItemDatas = progressData.QuestionListItemViewDatas(),
                FinishButtonInteractable = progressData.IsChecklistDone(),
            };
        }

        public static QuestionListItemViewData CheckListViewAt(this LevelEvaluationData evaluationData, int index)
        {
            return new QuestionListItemViewData()
            {
                Item = evaluationData.Level.Questions[index],
                State = evaluationData.AnswerEvaluations[index]
            };
        }

        public static IEnumerable<QuestionListItemViewData> CheckListViewDatas(
            this LevelEvaluationData evalData)
        {
            List<QuestionListItemViewData> viewDatas = new List<QuestionListItemViewData>();
            for (int i = 0; i < evalData.Level.Questions.Count; i++)
                viewDatas.Add(evalData.CheckListViewAt(i));
            return viewDatas;
        }

        public static IQuestionListViewData QuestionListViewData(this LevelEvaluationData evalData)
        {
            return new QuestionListViewData()
            {
                ItemDatas = evalData.CheckListViewDatas(),
                FinishButtonInteractable = true
            };
        }
    }
}