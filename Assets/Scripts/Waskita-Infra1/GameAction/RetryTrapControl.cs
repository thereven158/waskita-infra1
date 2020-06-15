using A3.AudioControl;
using A3.AudioControl.Unity;
using A3.UserInterface;
using Agate.WaskitaInfra1.LevelProgress;
using Agate.WaskitaInfra1.UserInterface.Display;
using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Agate.WaskitaInfra1.GameAction
{
    public class RetryTrapControl : MonoBehaviour
    {
        [SerializeField]
        private PopUpDisplay _information = default;

        [SerializeField]
        private YesNoPopUpDisplay _yesNoPopUp = default;

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

        public Action<bool> PauseCommand;
        public Action OnFinish;

        public void Init(
            LevelProgressControl levelProgressControl,
            UiDisplaysSystem<GameObject> uiDisplaysSystem,
            AudioSystem<AudioClip, AudioMixerGroup> audioSystem)
        {
            _levelProgress = levelProgressControl;
            _displaysSystem = uiDisplaysSystem;
            _audioSystem = audioSystem;
        }

        public void Invoke(RetryTrap data)
        {
            Action CorrectAction = () =>
            {
                Interaction();
                PauseCommand?.Invoke(false);
            };
            Action WrongAction = () =>
            {
                Interaction();
                FailurePopUps(data._failureMessage);
            };

            _audioSystem.PlayAudio(_rainWarning);
            PauseCommand?.Invoke(true);
            _displaysSystem
                .GetOrCreateDisplay<YesNoPopUpDisplay>(_yesNoPopUp)
                .Open(new YesNoPopUpViewData()
                {
                    MessageText = data._warningMessage,
                    NoAction = (!data._isContinueCorrect)? CorrectAction: WrongAction,
                    NoButtonText = "Tunda",
                    YesAction = data._isContinueCorrect ? CorrectAction : WrongAction,
                    YesButtonText = "Lanjut"
                });
        }

        public bool Ready => _levelProgress != null && _displaysSystem != null;

        private void FailurePopUps(string message)
        {
            void onClose()
            {
                Interaction();
                _levelProgress.RetryFromCheckPoint();
                PauseCommand?.Invoke(false);
            }
            _audioSystem.PlayAudio(_failAudio);
            _displaysSystem.GetOrCreateDisplay<PopUpDisplay>(_information).Open(
                message,
                onClose);
        }
        private void Interaction()
        {
            _audioSystem.PlayAudio(_buttonClick);
        }
    }
}