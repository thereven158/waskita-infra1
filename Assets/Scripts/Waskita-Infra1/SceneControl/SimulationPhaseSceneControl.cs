using System.Collections.Generic;
using A3.DataDrivenEvent;
using A3.UserInterface;
using Agate.GlSim.Scene.Control.Map.Loader;
using Agate.WaskitaInfra1;
using Agate.WaskitaInfra1.GameProgress;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using Agate.WaskitaInfra1.UserInterface.ChecklistList;
using Agate.WaskitaInfra1.Utilities;
using GameAction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInterface;
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

        [SerializeField]
        private List<RectTransform> _settingButtonPositions = default;

        [SerializeField]
        private Button _settingButton = default;

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

        [TextArea]
        [SerializeField]
        private string _finishProjectMessage = default;

        private PopUpDisplay _popupDisplay;

        private SettingDisplay _settingDisplay;

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
            _settingDisplay = Main.GetRegisteredComponent<SettingDisplay>();
            _actionSystem = new GameActionSystem();
            _stormControl.Init(_levelProgress, _displaysSystem);
            _actionSystem = new GameActionSystem();
            _actionSystem.Init(_stormControl);
            _settingButton.onClick.AddListener(() => _settingDisplay.gameObject.SetActive(true));

            _eventSystem.Init(_actionSystem);
            foreach (IEventTriggerData<EventTriggerData> eventData in _levelProgress.Data.Level.Events)
                _eventSystem.RegisterEvent(eventData);
            _dayText.text = $"Hari {_levelProgress.Data.CurrentDay:00}";
            _levelStateDisplay.OpenDisplay(_levelProgress.Data.Level.State());
            _nextDayButton.onClick.AddListener(() => _levelProgress.NextDay(1));
            _levelProgress.OnDayChange += OnDayChange;
            _levelProgress.OnRetryToCheckpoint += OnRetry;
            _levelProgress.OnFinishLevel += OnLevelFinish;
        }

        private void OnDayChange(uint day)
        {
            _dayText.text = $"Hari {day:00}";
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
            PopUpDisplay.Open(_finishProjectMessage,
                () =>
                {
                    _settingButton.GetComponent<RectTransform>().SetPosition(_settingButtonPositions[1]);
                    DisplayEvaluation(data);
                });
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

    public static class RectTransformExtension
    {
        public static void SetPosition(this RectTransform transform1, RectTransform transform2)
        {
            transform1.anchorMin = transform2.anchorMin;
            transform1.anchorMax = transform2.anchorMax;
            transform1.anchoredPosition = transform2.anchoredPosition;
            transform1.sizeDelta = transform2.sizeDelta;
        }
    }
}