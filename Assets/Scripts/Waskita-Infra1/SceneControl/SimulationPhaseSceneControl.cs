using A3.DataDrivenEvent;
using A3.UserInterface;
using Agate.GlSim.Scene.Control.Map.Loader;
using Agate.WaskitaInfra1;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using UnityEngine;
using UnityEngine.UI;
using UserInterface.Display;
using UserInterface.LevelState;

namespace SceneControl
{
    public class SimulationPhaseSceneControl : MonoBehaviour
    {
        private LevelProgressControl _levelProgress;
        private LevelStateDisplay _levelStateDisplay;
        private UiDisplaysSystem<GameObject> _displaysSystem;
        private GameplaySceneLoadControl _sceneLoader;
        [SerializeField]
        private PopUpDisplay _popUpDisplay = default;

        [SerializeField]
        private Text _dayText;

        [SerializeField]
        private Button _nextDayButton;

        [SerializeField]
        private DayEventTriggerSystem _eventSystem;

        private void Start()
        {
            _levelStateDisplay = Main.GetRegisteredComponent<LevelStateDisplay>();
            _levelStateDisplay.ToggleDisplay(true);
            _levelProgress = Main.GetRegisteredComponent<LevelProgressControl>();
            _displaysSystem = Main.GetRegisteredComponent<UiDisplaysSystemBehavior>();
            _sceneLoader = Main.GetRegisteredComponent<GameplaySceneLoadControl>();

            _eventSystem.Init(_levelProgress, _displaysSystem);
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
            _dayText.text = $"D-{day:00}";
            _eventSystem.InvokeEvent(new EventTriggerData() {Day = day});
        }

        private void OnRetry()
        {
            _sceneLoader.ChangeScene("SimulationPhase");
        }

        private void OnDestroy()
        {
            _levelProgress.OnRetryToCheckpoint -= OnRetry;
            _levelProgress.OnDayChange -= OnDayChange;
            _levelProgress.OnFinishLevel -= OnLevelFinish;
        }

        private void OnLevelFinish(LevelEvaluationData data)
        {
            _displaysSystem.GetOrCreateDisplay<PopUpDisplay>(_popUpDisplay).Open(
                "Project Telah selesai Evaluation Display Is Under Development",
                () => _sceneLoader.ChangeScene("PreparationPhase"));
        }
    }
}