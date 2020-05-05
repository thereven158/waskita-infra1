using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;

namespace Agate.WaskitaInfra1.UserInterface.ChecklistList
{
    public struct CheckListViewData
    {
        public IChecklistItem Item;
        public bool State;
    }

    public static class ProgressExtension
    {
        public static CheckListViewData CheckListViewAt(this ILevelProgressData levelProgressData, int index)
        {
            return new CheckListViewData()
            {
                Item = levelProgressData.Level.Quizzes[index],
                State = levelProgressData.Answers[index] != null
            };
        }
    }
}