using A3.AudioControl;
using A3.AudioControl.Unity;
using A3.DataDrivenEvent;
using A3.UserInterface;
using Agate.GlSim.Scene.Control.Map.Loader;
using Agate.WaskitaInfra1.Backend.Integration;
using Agate.WaskitaInfra1.GameAction;
using Agate.WaskitaInfra1.GameProgress;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using Agate.WaskitaInfra1.UserInterface;
using Agate.WaskitaInfra1.UserInterface.Display;
using Agate.WaskitaInfra1.UserInterface.LevelState;
using Agate.WaskitaInfra1.UserInterface.QuestionList;
using Agate.WaskitaInfra1.Utilities;
using GameAction;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.SceneControl
{
    public class SimulationPhaseSceneControl : MonoBehaviour
    {
        private LevelControl _levelControl;
        private LevelProgressControl _levelProgress;
        private GameProgressControl _gameProgress;
        private GameplaySceneLoadControl _sceneLoader;
        private BackendIntegrationController _backendIntegration;

        private AudioSystem<AudioClip, AudioMixerGroup> _audioSystem;
        private UiDisplaysSystem<GameObject> _displaysSystem;
        private SettingDisplay _settingDisplay;

        private GameActionSystem _actionSystem;

        [SerializeField]
        private LevelStateDisplay _levelStateDisplay = default;

        [SerializeField]
        private List<RectTransform> _settingButtonPositions = default;

        [SerializeField]
        private Button _settingButton = default;

        [SerializeField]
        private QuestionListInteractionDisplay _evaluationDisplay = default;

        [SerializeField]
        private TMP_Text _dayText = default;

        [SerializeField]
        private Button _nextDayButton = default;

        [SerializeField]
        private DayEventTriggerSystem _eventSystem = default;

        [SerializeField]
        private RetryTrapControl _stormControl = default;

        [SerializeField]
        private PopUpDisplay _popupDisplayPrefab = default;

        [TextArea]
        [SerializeField]
        private string _finishProjectMessage = default;

        [Header("Audio")]
        [SerializeField]
        private ScriptableAudioSpecification _bgm = default;
        [SerializeField]
        private ScriptableAudioSpecification _buttonClick = default;
        [SerializeField]
        private ScriptableAudioSpecification _ambience = default;
        [SerializeField]
        private ScriptableAudioSpecification _successSfx = default;
        [SerializeField]
        private ScriptableAudioSpecification _failureSfx = default;



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
            _backendIntegration = Main.GetRegisteredComponent<BackendIntegrationController>();
            _settingDisplay = Main.GetRegisteredComponent<SettingDisplay>();
            _audioSystem = Main.GetRegisteredComponent<AudioSystemBehavior>();
            _actionSystem = new GameActionSystem();

            _stormControl.Init(_levelProgress, _displaysSystem, _audioSystem);
            _actionSystem.Init(_stormControl);
            _eventSystem.Init(_actionSystem);

            _audioSystem.PlayAudio(_ambience);
            _settingButton.onClick.AddListener(() => _settingDisplay.ToggleDisplay(true));

            foreach (IEventTriggerData<EventTriggerData> eventData in _levelProgress.Data.Level.Events)
                _eventSystem.RegisterEvent(eventData);

            _dayText.text = $"Hari {_levelProgress.Data.CurrentDay:00}";
            _levelStateDisplay.OpenDisplay(_levelProgress.Data.Level.State());
            _nextDayButton.onClick.AddListener(() => _levelProgress.NextDay(1));

            _levelProgress.OnDayChange += OnDayChange;
            _levelProgress.OnRetryToCheckpoint += OnRetry;
            _levelProgress.OnFinishLevel += OnLevelFinish;
            _levelProgress.OnCheckPointUpdate += OnCheckPointUpdate;
            Main.OnLogOut += OnLogOut;

            if (_levelProgress.Data.CurrentDay > 0) return;
            _levelProgress.NextDay(1);
            _levelProgress.UpdateCheckPoint();
        }

        private void OnLogOut()
        {
            _settingDisplay.ToggleDisplay(false);
            _displaysSystem.GetOrCreateDisplay<QuestionListInteractionDisplay>(_evaluationDisplay).Close();
        }

        private void OnDayChange(uint day)
        {
            _dayText.text = $"Hari {day:00}";
            _eventSystem.InvokeEvent(new EventTriggerData() { Day = day });
        }

        private void OnRetry()
        {
            void Action()
            {
                _sceneLoader.ChangeScene("SimulationPhase");
            }

            void OnFinish(UnityWebRequest finishedRequest)
            {
                
                switch (finishedRequest.responseCode)
                {
                    case 200:
                        Action();
                        break;
                    default:
                        OnDoubleLogin(finishedRequest);
                        break;
                }
            }

            if (!Main.Instance.IsOnline)
                Action();
            else
                StartCoroutine(_backendIntegration.AwaitSaveLevelProgressRequest(_levelProgress.Data, OnFinish));
        }

        private void OnDestroy()
        {
            Main.OnLogOut -= OnLogOut;
            if (Main.Instance == null) return;
            if (!Main.Instance.UiLoaded) return;
            _levelProgress.OnRetryToCheckpoint -= OnRetry;
            _levelProgress.OnDayChange -= OnDayChange;
            _levelProgress.OnFinishLevel -= OnLevelFinish;
            _levelProgress.OnCheckPointUpdate -= OnCheckPointUpdate;
        }

        private void OnCheckPointUpdate(uint data)
        {
            if (!Main.Instance.IsOnline) return;
            void OnFinish(UnityWebRequest finishedRequest)
            {
                switch (finishedRequest.responseCode)
                {
                    case 200:
                        //do nothing
                        break;
                    default:
                        OnDoubleLogin(finishedRequest);
                        break;
                }
                
            }

            StartCoroutine(_backendIntegration.AwaitSaveLevelProgressRequest(_levelProgress.Data, OnFinish));
        }

        private void OnDoubleLogin(UnityWebRequest webReq)
        {
            _backendIntegration.OpenErrorResponsePopUp(webReq, () => {
                Main.LogOut();
                OnLogOut();
                _sceneLoader.ChangeScene("Title");
            });
        }

        private void OnLevelFinish(LevelEvaluationData data)
        {
            _evaluationMessages = data.EvaluationMessages();
            void OnFinishRequest(UnityWebRequest webReq)
            {
                //do nothing
            }
            if (Main.Instance.IsOnline)
                StartCoroutine(_backendIntegration.AwaitEndLevelRequest(data, OnFinishRequest));
            void OnClose()
            {
                _settingButton.GetComponent<RectTransform>().SetPosition(_settingButtonPositions[1]);
                _audioSystem.PlayAudio(_buttonClick);
                _audioSystem.PlayAudio(_bgm);
                _audioSystem.StopAudio(_ambience);
                DisplayEvaluation(data);
            }
            if (_evaluationMessages.Count < 1)
                _gameProgress.UpdateCompletedLevelIndex((short)_levelControl.IndexOf(data.Level));
            PopUpDisplay.Open(_finishProjectMessage, OnClose);
        }


        private void DisplayEvaluation(LevelEvaluationData data)
        {
            QuestionListInteractionDisplay display =
                _displaysSystem.GetOrCreateDisplay<QuestionListInteractionDisplay>(_evaluationDisplay);

            display.Open(
                data.QuestionListViewData(),
                new QuestionListInteractionData()
                {
                    OnFinishButton = () =>
                    {
                        _audioSystem.PlayAudio(_buttonClick);
                        OnFinishEvaluation();
                        display.Close();
                    }
                });
        }

        private void OnFinishEvaluation()
        {
            _settingButton.GetComponent<RectTransform>().SetPosition(_settingButtonPositions[0]);
            if (_evaluationMessages.Count > 0)
            {
                _audioSystem.PlayAudio(_failureSfx);
                CycleDisplayEvaluationMessage();
            }
            else
            {
                _audioSystem.PlayAudio(_successSfx);
                PopUpDisplay.Open("Project Success", LoadPrepScene);
            }
        }

        private void CycleDisplayEvaluationMessage()
        {
            if (_evaluationMessages.Count < 1)
            {
                LoadPrepScene();
                return;
            }
            void onInteraction()
            {
                _audioSystem.PlayAudio(_buttonClick);
                CycleDisplayEvaluationMessage();
            }
            PopUpDisplay.Open(
                _evaluationMessages.Dequeue(),
                onInteraction);
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