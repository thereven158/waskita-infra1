using A3.UserInterface;
using Agate.WaskitaInfra1.LevelProgress;
using UnityEngine;
using Agate.WaskitaInfra1.UserInterface.Display;

namespace GameAction
{
    public class StormActionControl : MonoBehaviour
    {
        [SerializeField]
        private PopUpDisplay _information = default;

        [SerializeField]
        private ConfirmationPopUpDisplay _confirmation = default;

        [SerializeField]
        [TextArea]
        private string _stormWarningMessage = default;

        [SerializeField]
        [TextArea]
        private string _retryFromCheckpointMessage = default;

        private UiDisplaysSystem<GameObject> _displaysSystem;
        private LevelProgressControl _levelProgress;


        public void Init(LevelProgressControl levelProgressControl, UiDisplaysSystem<GameObject> uiDisplaysSystem)
        {
            _levelProgress = levelProgressControl;
            _displaysSystem = uiDisplaysSystem;
        }

        public void Invoke()
        {
            _displaysSystem
                .GetOrCreateDisplay<ConfirmationPopUpDisplay>(_confirmation)
                .Open(new ConfirmationPopUpViewData()
                {
                    MessageText = _stormWarningMessage,
                    CloseAction = () => _levelProgress.NextDay(1),
                    CloseButtonText = "Tunda",
                    ConfirmAction = FailurePopUps
                });
        }

        public bool Ready => _levelProgress != null && _displaysSystem != null;

        private void FailurePopUps()
        {
            _displaysSystem.GetOrCreateDisplay<PopUpDisplay>(_information).Open(
                _retryFromCheckpointMessage,
                _levelProgress.RetryFromCheckPoint);
        }
    }
}