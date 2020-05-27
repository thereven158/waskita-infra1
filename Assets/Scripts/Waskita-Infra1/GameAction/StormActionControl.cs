using A3.UserInterface;
using Agate.WaskitaInfra1.LevelProgress;
using UnityEngine;
using Agate.WaskitaInfra1.UserInterface.Display;
using A3.AudioControl;
using UnityEngine.Audio;
using A3.AudioControl.Unity;

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

        [Header("Audio")]
        [SerializeField]
        private ScriptableAudioSpecification _rainWarning = default;

        [SerializeField]
        private ScriptableAudioSpecification _failAudio = default;

        [SerializeField]
        private ScriptableAudioSpecification _buttonClick = default;


        private UiDisplaysSystem<GameObject> _displaysSystem;
        private LevelProgressControl _levelProgress;
        private AudioSystem<AudioClip, AudioMixerGroup> _audioSystem;



        public void Init(LevelProgressControl levelProgressControl, UiDisplaysSystem<GameObject> uiDisplaysSystem, AudioSystem<AudioClip, AudioMixerGroup> audioSystem)
        {
            _levelProgress = levelProgressControl;
            _displaysSystem = uiDisplaysSystem;
            _audioSystem = audioSystem;
        }

        public void Invoke()
        {
            _audioSystem.PlayAudio(_rainWarning);
            void onClose()
            {
                Interaction();
                _levelProgress.NextDay(1);
            }
            void onConfirm()
            {
                Interaction();
                FailurePopUps();
            }
            _displaysSystem
                .GetOrCreateDisplay<ConfirmationPopUpDisplay>(_confirmation)
                .Open(new ConfirmationPopUpViewData()
                {
                    MessageText = _stormWarningMessage,
                    CloseButtonText = "Tunda",
                    CloseAction = onClose,
                    ConfirmAction = onConfirm
                });
        }

        public bool Ready => _levelProgress != null && _displaysSystem != null;

        private void FailurePopUps()
        {
            _audioSystem.PlayAudio(_failAudio);

            void onClose()
            {
                Interaction();
                _levelProgress.RetryFromCheckPoint();
            }
            _displaysSystem.GetOrCreateDisplay<PopUpDisplay>(_information).Open(
                _retryFromCheckpointMessage,
                onClose);
        }

        private void Interaction()
        {
            _audioSystem.PlayAudio(_buttonClick);
        }
    }
}