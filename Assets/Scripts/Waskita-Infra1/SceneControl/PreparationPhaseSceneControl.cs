using A3.UserInterface;
using Agate.GlSim.Scene.Control.Map.Loader;
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
using UnityEngine.UI;
using UserInterface;
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
        private QuestionListInteractionDisplay _checklistInteractionDisplay;
        private QuizDisplay _quizDisplay;
        private UiDisplaysSystem<GameObject> _displaySystem;
        private GameplaySceneLoadControl _sceneLoader;
        private SettingDisplay _settingDisplay;

        [SerializeField]
        private ConfirmationPopUpDisplay _confirmationPopUp = default;

        [SerializeField]
        private Button _settingButton = default;

        [TextArea]
        [SerializeField]
        private string _confirmationMessage;
        
        private void Start()
        {
            _gameProgress = Main.GetRegisteredComponent<GameProgressControl>();
            _levelProgress = Main.GetRegisteredComponent<LevelProgressControl>();
            _levelControl = Main.GetRegisteredComponent<LevelControl>();
            _levelListDisplay = Main.GetRegisteredComponent<LevelDataListDisplay>();
            _levelDataDisplay = Main.GetRegisteredComponent<LevelDataDisplay>();
            _checklistInteractionDisplay = Main.GetRegisteredComponent<QuestionListInteractionDisplay>();
            _quizDisplay = Main.GetRegisteredComponent<QuizDisplay>();
            _levelStateDisplay = Main.GetRegisteredComponent<LevelStateDisplay>();
            _displaySystem = Main.GetRegisteredComponent<UiDisplaysSystemBehavior>();
            _sceneLoader = Main.GetRegisteredComponent<GameplaySceneLoadControl>();
            _settingDisplay = Main.GetRegisteredComponent<SettingDisplay>();
            _settingButton.onClick.AddListener(()=> _settingDisplay.gameObject.SetActive(true));
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
                _gameProgress.Data.MaxCompletedLevelIndex + 2);
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
            _checklistInteractionDisplay.Open(_levelProgress.Data, OpenCheckListItem, SimulationConfirmation);
        }

        private void OpenCheckListItem(IQuestion item)
        {
            _checklistInteractionDisplay.Close();
            _quizDisplay.Display(
                item, 
                (quiz, o) => _levelProgress.AnswerQuestion(item, o), 
                OpenCheckList,
                _levelProgress.Data.AnswerOf(item));
        }

        private void SimulationConfirmation()
        {
            _displaySystem
                .GetOrCreateDisplay<ConfirmationPopUpDisplay>(_confirmationPopUp)
                .Open(_confirmationMessage,
                    GoToSimulation,
                    null);
        }

        private void GoToSimulation()
        {
            _quizDisplay.Close();
            _levelStateDisplay.ToggleDisplay(false);
            _levelDataDisplay.ToggleDisplay(false);
            _checklistInteractionDisplay.Close();
            _sceneLoader.ChangeScene("SimulationPhase");
        }
    }
}