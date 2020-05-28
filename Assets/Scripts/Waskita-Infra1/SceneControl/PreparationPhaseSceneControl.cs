using A3.AudioControl;
using A3.AudioControl.Unity;
using A3.Quiz;
using A3.UserInterface;
using Agate.GlSim.Scene.Control.Map.Loader;
using Agate.WaskitaInfra1.GameProgress;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using Agate.WaskitaInfra1.UserInterface;
using Agate.WaskitaInfra1.UserInterface.Display;
using Agate.WaskitaInfra1.UserInterface.LevelList;
using Agate.WaskitaInfra1.UserInterface.LevelState;
using Agate.WaskitaInfra1.UserInterface.QuestionList;
using Agate.WaskitaInfra1.UserInterface.Quiz;
using Agate.WaskitaInfra1.Utilities;
using Agate.WaskitaInfra1.Backend.Integration;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.SceneControl
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
        private AudioSystem<AudioClip, AudioMixerGroup> _audioSystem;
        private BackendIntegrationController _backendControl;

        private Main _main;

        [SerializeField]
        private ConfirmationPopUpDisplay _confirmationPopUp = default;

        [SerializeField]
        private Button _settingButton = default;

        [TextArea]
        [SerializeField]
        private string _abortConfirmationMessage = default;

        [TextArea]
        [SerializeField]
        private string _finishConfirmationMessage = default;

        [Header("Audio")]
        [SerializeField]
        private ScriptableAudioSpecification _bgm = default;

        [SerializeField]
        private ScriptableAudioSpecification _buttonClick = default;

        private void Start()
        {
            _main = Main.Instance;
            _gameProgress = Main.GetRegisteredComponent<GameProgressControl>();
            _levelProgress = Main.GetRegisteredComponent<LevelProgressControl>();
            _levelControl = Main.GetRegisteredComponent<LevelControl>();
            _levelListDisplay = Main.GetRegisteredComponent<LevelDataListDisplay>();
            _audioSystem = Main.GetRegisteredComponent<AudioSystemBehavior>();
            _backendControl = Main.GetRegisteredComponent<BackendIntegrationController>();
            _sceneLoader = Main.GetRegisteredComponent<GameplaySceneLoadControl>();

            _levelDataDisplay = Main.GetRegisteredComponent<LevelDataDisplay>();
            _checklistInteractionDisplay = Main.GetRegisteredComponent<QuestionListInteractionDisplay>();
            _quizDisplay = Main.GetRegisteredComponent<QuizDisplay>();
            _levelStateDisplay = Main.GetRegisteredComponent<LevelStateDisplay>();
            _displaySystem = Main.GetRegisteredComponent<UiDisplaysSystemBehavior>();
            _settingDisplay = Main.GetRegisteredComponent<SettingDisplay>();

            _settingButton.onClick.AddListener(() => _settingDisplay.ToggleDisplay(true));
            Main.OnLogOut += OnLogOut;

            _audioSystem.PlayAudio(_bgm);

            if (_levelProgress.Data == null)
                OpenProjectList();
            else if (_levelProgress.Data.LastCheckpoint < 1)
                OpenCheckList();
            else
                GoToSimulation();
        }

        private void OnDestroy()
        {
            Main.OnLogOut -= OnLogOut;
        }

        private void OnLogOut()
        {
            _levelListDisplay.Close();
            _levelDataDisplay.ToggleDisplay(false);
            _checklistInteractionDisplay.Close();
            _levelStateDisplay.ToggleDisplay(false);
            _settingDisplay.ToggleDisplay(false);
        }

        private void OpenProjectList()
        {
            _audioSystem.PlayAudio(_buttonClick);
            _levelStateDisplay.ToggleDisplay(false);
            _checklistInteractionDisplay.Close();
            _levelListDisplay.OpenList(
                _levelControl.Levels,
                OpenProjectConfirmation,
                _gameProgress.Data.MaxCompletedLevelIndex + 2);
        }

        private void OpenProjectConfirmation(LevelData data)
        {
            _audioSystem.PlayAudio(_buttonClick);
            void OnConfirm(LevelData levelData)
            {
                _audioSystem.PlayAudio(_buttonClick);
                _levelProgress.StartLevel(levelData);
                if (!_main.IsOnline)
                    StartCoroutine(_backendControl.AwaitStartGameRequest(levelData, OnFinishReqStartGame));
                OpenCheckList();
            }

            _levelListDisplay.Close();
            _levelDataDisplay.OpenDisplay(
                data,
                OnConfirm,
                OpenProjectList);
        }

        private void OnFinishReqStartGame(UnityWebRequest webReq)
        {
            // do nothing
        }

        private void OpenCheckList()
        {
            _levelStateDisplay.OpenDisplay(_levelProgress.Data.Level.State());

            _checklistInteractionDisplay.Open(
                _levelProgress.Data.QuestionListViewData(),
                new QuestionListInteractionData()
                {
                    OnDataInteraction = OpenCheckListItem,
                    OnFinishButton = SimulationConfirmation,
                    OnAbortButton = AbortConfirmation
                });
        }

        private void OpenCheckListItem(IQuestion item)
        {
            _audioSystem.PlayAudio(_buttonClick);
            void OnAnswerQuiz(IQuiz quiz, object o)
            {
                _levelProgress.AnswerQuestion(item, o);
                if (!_main.IsOnline)
                    StartCoroutine(_backendControl.AwaitSaveGameRequest(_levelProgress.Data, OnFinishReqSaveData));
            }
            _quizDisplay.Display(
                item,
                OnAnswerQuiz,
                OpenCheckList,
                _levelProgress.Data.AnswerOf(item));
        }

        private void OnFinishReqSaveData(UnityWebRequest webReq)
        {
            // do nothing
        }

        private void SimulationConfirmation()
        {
            _audioSystem.PlayAudio(_buttonClick);
            _displaySystem
                .GetOrCreateDisplay<ConfirmationPopUpDisplay>(_confirmationPopUp)
                .Open(_finishConfirmationMessage,
                    GoToSimulation,
                    null);
        }

        private void AbortConfirmation()
        {
            void OnConfirm()
            {
                _audioSystem.PlayAudio(_buttonClick);
                _levelProgress.FinishLevel();
                OpenProjectList();
            }

            _displaySystem
                .GetOrCreateDisplay<ConfirmationPopUpDisplay>(_confirmationPopUp)
                .Open(_abortConfirmationMessage,
                    OnConfirm,
                    null);
        }

        private void GoToSimulation()
        {
            _audioSystem.PlayAudio(_buttonClick);
            _audioSystem.StopAudio(_bgm);
            _quizDisplay.Close();
            _levelStateDisplay.ToggleDisplay(false);
            _levelDataDisplay.ToggleDisplay(false);
            _checklistInteractionDisplay.Close();
            _sceneLoader.ChangeScene("SimulationPhase");
        }
    }
}