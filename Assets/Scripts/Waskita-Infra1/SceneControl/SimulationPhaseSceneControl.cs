using System.Collections.Generic;
using A3.DataDrivenEvent;
using A3.UserInterface;
using Agate.GlSim.Scene.Control.Map.Loader;
using Agate.WaskitaInfra1;
using Agate.WaskitaInfra1.GameProgress;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using Agate.WaskitaInfra1.UserInterface.ChecklistList;
using GameAction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInterface.Display;
using UserInterface.LevelState;

namespace SceneControl
{
    public class SimulationPhaseSceneControl : MonoBehaviour
    {
        private LevelControl _levelControl;
        private LevelProgressControl _levelProgress;
        private GameProgressControl _gameProgress;
        [SerializeField]
        private LevelStateDisplay _levelStateDisplay = default;
        private UiDisplaysSystem<GameObject> _displaysSystem;
        private GameplaySceneLoadControl _sceneLoader;

        private GameActionSystem _actionSystem;

        [SerializeField]
        private QuestionListInteractionDisplay _evaluationDisplay;

        [SerializeField]
        private TMP_Text _dayText = default;

        [SerializeField]
        private Button _nextDayButton = default;

        [SerializeField]
        private DayEventTriggerSystem _eventSystem = default;

        [SerializeField]
        private StormActionControl _stormControl = default;

        [SerializeField]
        private PopUpDisplay _popupDisplayPrefab;

        private PopUpDisplay _popupDisplay;

        private PopUpDisplay PopUpDisplay
        {
            get
            {
                _popupDisplay = _popupDisplay
                    ? _popupDisplay
                    : _displaysSystem.GetOrCreateDisplay<PopUpDisplay>(_popupDisplayPrefab);
                return _popupDisplay;
            }
        }

        private Queue<string> _evaluationMessages;

        private void Start()
        {
            if (!Main.Instance.UiLoaded) return;
            _levelProgress = Main.GetRegisteredComponent<LevelProgressControl>();
            _displaysSystem = Main.GetRegisteredComponent<UiDisplaysSystemBehavior>();
            _sceneLoader = Main.GetRegisteredComponent<GameplaySceneLoadControl>();
            _gameProgress = Main.GetRegisteredComponent<GameProgressControl>();
            _levelControl = Main.GetRegisteredComponent<LevelControl>();
            _actionSystem = new GameActionSystem();
            _stormControl.Init(_levelProgress,_displaysSystem);
                _actionSystem = new GameActionSystem();
            _actionSystem.Init(_stormControl);

            _eventSystem.Init(_actionSystem);
            foreach (IEventTriggerData<EventTriggerData> eventData in _levelProgress.Data.Level.Events)
                _eventSystem.RegisterEvent(eventData);
            _dayText.text = $"D-{_levelProgress.Data.CurrentDay:00}";
            _nextDayButton.onClick.AddListener(() => _levelProgress.NextDay(1));
            _levelProgress.OnDayChange += OnDayChange;
            _levelProgress.OnRetryToCheckpoint += OnRetry;
            _levelProgress.OnFinishLevel += OnLevelFinish;
        }

        private void OnDayChange(uint day)
        {
            _dayText.text = $"Hari ke {day:00}";
            _eventSystem.InvokeEvent(new EventTriggerData() {Day = day});
        }

        private void OnRetry()
        {
            _sceneLoader.ChangeScene("SimulationPhase");
        }

        private void OnDestroy()
        {
            if (Main.Instance == null) return;
            if (!Main.Instance.UiLoaded) return;
            _levelProgress.OnRetryToCheckpoint -= OnRetry;
            _levelProgress.OnDayChange -= OnDayChange;
            _levelProgress.OnFinishLevel -= OnLevelFinish;
        }

        private void OnLevelFinish(LevelEvaluationData data)
        {
            _evaluationMessages = data.EvaluationMessages();
            if (_evaluationMessages.Count < 1)
                _gameProgress.UpdateCompletedLevelIndex((short) _levelControl.IndexOf(data.Level));
            PopUpDisplay.Open(
                "Setelah 2 bulan project berjalan akhirnya project selesai dan akan di evaluasi",
                () => DisplayEvaluation(data));
        }

        private void DisplayEvaluation(LevelEvaluationData data)
        {
            QuestionListInteractionDisplay display =
                _displaysSystem.GetOrCreateDisplay<QuestionListInteractionDisplay>(_evaluationDisplay);

            display.Open(
                data,
                () =>
                {
                    if (_evaluationMessages.Count > 0)
                        CycleDisplayEvaluationMessage();
                    else
                        PopUpDisplay.Open("Project Success", LoadPrepScene);
                    display.Close();
                });
        }

        private void CycleDisplayEvaluationMessage()
        {
            if (_evaluationMessages.Count < 1)
            {
                LoadPrepScene();
                return;
            }

            PopUpDisplay.Open(
                _evaluationMessages.Dequeue(),
                CycleDisplayEvaluationMessage);
        }


        private void LoadPrepScene()
        {
            _sceneLoader.ChangeScene("PreparationPhase");
        }
    }
}