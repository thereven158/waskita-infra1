using A3.AudioControl;
using A3.AudioControl.Unity;
using A3.UserInterface;
using Agate.WaskitaInfra1.UserInterface;
using Agate.WaskitaInfra1.UserInterface.Display;
using Agate.WaskitaInfra1.UserInterface.LevelList;
using Agate.WaskitaInfra1.UserInterface.LevelState;
using Agate.WaskitaInfra1.UserInterface.QuestionList;
using Agate.WaskitaInfra1.UserInterface.Quiz;
using UnityEngine;
using UnityEngine.Audio;

namespace Agate.WaskitaInfra1.SceneControl
{
    public class UserInterfaceSceneControl : MonoBehaviour
    {
        private const string MIXER_BGM_PARAM = "BgmVolume";
        private const string MIXER_SFX_PARAM = "SfxVolume";
        [SerializeField]
        private QuizDisplay _quizDisplay = default;

        [SerializeField]
        private LevelStateDisplay _levelStateDisplay = default;

        [SerializeField]
        private LevelDataListDisplay _levelDataListDisplay = default;

        [SerializeField]
        private LevelDataDisplay _levelDataDisplay = default;

        [SerializeField]
        private QuestionListInteractionDisplay _checklistInteractionDisplay = default;

        [SerializeField]
        private UiDisplaysSystemBehavior _displaySystem = default;

        [SerializeField]
        private SettingDisplay _settingDisplay = default;

        [Header("Audio")]
        [SerializeField]
        private ScriptableAudioSpecification _buttonClick = default;
        [Header("Setting Configuration")]
        [SerializeField]
        private AudioMixer _audioMixer = default;

        [SerializeField]
        private ConfirmationPopUpDisplay _confirmationPopup = default;

        [SerializeField]
        [TextArea]
        private string _exitConfirmationText = default;

        [SerializeField]
        [TextArea]
        private string _logoutConfirmationText = default;

        private AudioSystem<AudioClip, AudioMixerGroup> _audioSystem;
        private UiDisplaysSystem<GameObject> DisplaySystem => _displaySystem;

        private void Start()
        {
            Main main = Main.Instance;
            _displaySystem.Init();
            _checklistInteractionDisplay.Init();
            _quizDisplay.Init();
            _audioSystem = Main.GetRegisteredComponent<AudioSystemBehavior>();
            Main.RegisterComponents(_quizDisplay, _levelDataListDisplay, _levelStateDisplay, _levelDataDisplay,
                _checklistInteractionDisplay, _displaySystem, _settingDisplay);
            _quizDisplay.Close();
            _levelStateDisplay.ToggleDisplay(false);
            _levelDataListDisplay.Close();
            _levelDataDisplay.ToggleDisplay(false);
            _checklistInteractionDisplay.Close();

            _quizDisplay.OnInteraction += () => _audioSystem.PlayAudio(_buttonClick);
            ConfigureSettings();

            main.UiLoaded = true;
        }

        private void ConfigureSettings()
        {
            _audioMixer.SetFloat(MIXER_BGM_PARAM, PlayerPrefs.GetFloat(MIXER_BGM_PARAM, 0));
            _settingDisplay.BgmToggleState = PlayerPrefs.GetFloat(MIXER_BGM_PARAM, 0) >= 0;
            _audioMixer.SetFloat(MIXER_SFX_PARAM, PlayerPrefs.GetFloat(MIXER_SFX_PARAM, 0));
            _settingDisplay.SfxToggleState = PlayerPrefs.GetFloat(MIXER_SFX_PARAM, 0) >= 0;

            _settingDisplay.OnInteraction += () => _audioSystem.PlayAudio(_buttonClick);
            _settingDisplay.OnBgmToggle += toggle => _audioMixer.SetFloat(MIXER_BGM_PARAM, toggle ? 0 : -80);
            _settingDisplay.OnBgmToggle += toggle => PlayerPrefs.SetFloat(MIXER_BGM_PARAM, toggle ? 0 : -80);
            _settingDisplay.OnSfxToggle += toggle => _audioMixer.SetFloat(MIXER_SFX_PARAM, toggle ? 0 : -80);
            _settingDisplay.OnSfxToggle += toggle => PlayerPrefs.SetFloat(MIXER_SFX_PARAM, toggle ? 0 : -80);

            void OnSettingExit()
            {
                DisplaySystem.GetOrCreateDisplay<ConfirmationPopUpDisplay>(_confirmationPopup)
                    .Open(new ConfirmationPopUpViewData()
                    {
                        MessageText = _exitConfirmationText,
                        ConfirmAction = Main.Quit,
                        CloseAction = null
                    });
            }
            void OnSettingLogout()
            {
                DisplaySystem.GetOrCreateDisplay<ConfirmationPopUpDisplay>(_confirmationPopup)
                    .Open(new ConfirmationPopUpViewData()
                    {
                        MessageText = _logoutConfirmationText,
                        ConfirmAction = Main.LogOut,
                        CloseAction = null
                    });
            }

            _settingDisplay.OnExitPress = OnSettingExit;
            _settingDisplay.OnLogOutPress = OnSettingLogout;
        }
    }
}