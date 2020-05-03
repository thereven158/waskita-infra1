using A3.UserInterface;
using Agate.WaskitaInfra1;
using Agate.WaskitaInfra1.GameProgress;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using Agate.WaskitaInfra1.UserInterface;
using Agate.WaskitaInfra1.UserInterface.ChecklistList;
using Agate.WaskitaInfra1.UserInterface.LevelList;
using Agate.WaskitaInfra1.UserInterface.Quiz;
using Agate.WaskitaInfra1.Utilities;
using UnityEngine;
using UserInterface.Display;
using UserInterface.LevelState;

namespace SceneControl
{
    public class PreparationPhaseSceneControl : MonoBehaviour
    {
        private GameProgressControl _gameProgress;
        private LevelProgressControl _levelProgress;
        private LevelControl _levelControl;
        private LevelDataListDisplay _levelListDisplay;
        private LevelDataDisplay _levelDataDisplay;
        private LevelStateDisplay _levelStateDisplay;
        private LevelProgressCheckListDisplay _checklistDisplay;
        private QuizDisplay _quizDisplay;
        private UiDisplaysSystem<GameObject> _displaySystem;
        [SerializeField]
        private ConfirmationPopUpDisplay _confirmationPopUp = default;

        private void Start()
        {
            _gameProgress = Main.GetRegisteredComponent<GameProgressControl>();
            _levelProgress = Main.GetRegisteredComponent<LevelProgressControl>();
            _levelControl = Main.GetRegisteredComponent<LevelControl>();
            _levelListDisplay = Main.GetRegisteredComponent<LevelDataListDisplay>();
            _levelDataDisplay = Main.GetRegisteredComponent<LevelDataDisplay>();
            _checklistDisplay = Main.GetRegisteredComponent<LevelProgressCheckListDisplay>();
            _quizDisplay = Main.GetRegisteredComponent<QuizDisplay>();
            _levelStateDisplay = Main.GetRegisteredComponent<LevelStateDisplay>();
            _displaySystem = Main.GetRegisteredComponent<UiDisplaysSystemBehavior>();
            if (_levelProgress.Data == null)
                OpenProjectList();
            else
                OpenCheckList();
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
            _levelStateDisplay.OpenDisplay(_levelProgress.Data.Level.State());
            _checklistDisplay.Open(_levelProgress.Data, OpenCheckListItem, SimulationConfirmation);
        }

        private void OpenCheckListItem(IChecklistItem item)
        {
            _checklistDisplay.Close();
            _quizDisplay.Display(item.Quiz, (quiz, o) => _levelProgress.AnswerQuestion(item, o), OpenCheckList);
        }

        private void SimulationConfirmation()
        {
            _displaySystem.GetOrCreateDisplay<ConfirmationPopUpDisplay>(_confirmationPopUp)
                .Open("anda yakin ?", () => Debug.Log("GO TO SIM SCENE"), null);
        }
    }
}