using Agate.WaskitaInfra1;
using Agate.WaskitaInfra1.GameProgress;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using Agate.WaskitaInfra1.UserInterface;
using Agate.WaskitaInfra1.UserInterface.ChecklistList;
using Agate.WaskitaInfra1.UserInterface.LevelList;
using Agate.WaskitaInfra1.UserInterface.Quiz;
using UnityEngine;

namespace SceneControl
{
    public class PreparationPhaseSceneControl : MonoBehaviour
    {
        private GameProgressControl _gameProgress;
        private LevelProgressControl _levelProgress;
        private LevelControl _levelControl;
        private LevelDataListDisplay _levelListDisplay;
        private LevelDataDisplay _levelDataDisplay;
        private LevelProgressCheckListDisplay _checklistDisplay;
        private QuizDisplay _quizDisplay;
        
        private void Start()
        {
            _gameProgress = Main.GetRegisteredComponent<GameProgressControl>();
            _levelProgress = Main.GetRegisteredComponent<LevelProgressControl>();
            _levelControl = Main.GetRegisteredComponent<LevelControl>();
            _levelListDisplay = Main.GetRegisteredComponent<LevelDataListDisplay>();
            _levelDataDisplay = Main.GetRegisteredComponent<LevelDataDisplay>();
            _checklistDisplay = Main.GetRegisteredComponent<LevelProgressCheckListDisplay>();
            _quizDisplay = Main.GetRegisteredComponent<QuizDisplay>();
            if (_levelProgress.Data == null)
            {
                OpenProjectList();
            }
        }

        private void OpenProjectList()
        {
            _levelListDisplay.OpenList(
                _levelControl.Levels,
                OpenProjectConfirmation,
                _gameProgress.Data.MaxCompletedLevelIndex + 1);
        }

        private void OpenProjectConfirmation(LevelData data)
        {
            _levelListDisplay.Close();
            _levelDataDisplay.OpenDisplay(
                data, 
                levelData =>
                {
                    _levelProgress.StartLevel(levelData);
                    OpenCheckList();
                }, 
                OpenProjectList);
        }

        private void OpenCheckList()
        {
            _checklistDisplay.Open(_levelProgress.Data, OpenCheckListItem);
        }

        private void OpenCheckListItem(IChecklistItem item)
        {
            _checklistDisplay.Close();
            _quizDisplay.Display(item.Quiz,(quiz, o) => _levelProgress.AnswerQuestion(item,o), OpenCheckList);
        }
    }
}