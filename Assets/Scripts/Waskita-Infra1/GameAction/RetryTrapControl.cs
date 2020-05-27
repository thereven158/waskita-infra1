using A3.UserInterface;
using Agate.WaskitaInfra1.LevelProgress;
using UnityEngine;
using Agate.WaskitaInfra1.UserInterface.Display;
using System;
using A3.AudioControl.Unity;
using A3.AudioControl;
using UnityEngine.Audio;

namespace GameAction
{
    public class RetryTrapControl : MonoBehaviour
    {
        [SerializeField]
        private PopUpDisplay _information = default;

        [SerializeField]
        private ConfirmationPopUpDisplay _confirmation = default;

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
                _levelProgress.NextDay(1);
            };
            Action WrongAction = () =>
            {
                Interaction();
                FailurePopUps(data._failureMessage);
            };

            _audioSystem.PlayAudio(_rainWarning);
            _displaysSystem
                .GetOrCreateDisplay<ConfirmationPopUpDisplay>(_confirmation)
                .Open(new ConfirmationPopUpViewData()
                {
                    MessageText = data._warningMessage,
                    CloseAction = !data._isContinueCorrect ? CorrectAction : WrongAction,
                    CloseButtonText = "Tunda",
                    ConfirmAction = data._isContinueCorrect ? CorrectAction : WrongAction
                });
        }

        public bool Ready => _levelProgress != null && _displaysSystem != null;

        private void FailurePopUps(string message)
        {
            void onClose()
            {
                Interaction();
                _levelProgress.RetryFromCheckPoint();
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