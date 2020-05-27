using System.Collections;
using A3.UserInterface;
using Agate.GlSim.Scene.Control.Map.Loader;
using Agate.Waskita.Responses;
using Agate.WaskitaInfra1.PlayerAccount;
using Agate.WaskitaInfra1.UserInterface.Display;
using Agate.WaskitaInfra1.UserInterface.Login;
using BackendIntegration;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using A3.AudioControl;
using UnityEngine.Audio;
using A3.AudioControl.Unity;

namespace Agate.WaskitaInfra1.SceneControl.Login
{
    public class TitleSceneControl : MonoBehaviour
    {
        [SerializeField]
        private LoginFormDisplay _loginForm = null;

        [SerializeField]
        private Button _startButton = null;

        [SerializeField]
        private Button _logOutButton = null;

        [SerializeField]
        private PopUpDisplay _popUpDisplay = default;

        [Header("Audio")]
        [SerializeField]
        private ScriptableAudioSpecification _bgm = default;

        [SerializeField]
        private ScriptableAudioSpecification _buttonInteraction = default;

        private bool loggedIn;

        private Main _main;
        private PlayerAccountControl _accountControl;
        private UiDisplaysSystem<GameObject> _displaysSystem;
        private GameplaySceneLoadControl _sceneLoadControl;
        private BackendIntegrationController _backendControl;
        private AudioSystem<AudioClip, AudioMixerGroup> _audioSystem;

        private IEnumerator Start()
        {
            _main = Main.Instance;
            _accountControl = Main.GetRegisteredComponent<PlayerAccountControl>();
            _backendControl = Main.GetRegisteredComponent<BackendIntegrationController>();
            _displaysSystem = Main.GetRegisteredComponent<UiDisplaysSystemBehavior>();
            _sceneLoadControl = Main.GetRegisteredComponent<GameplaySceneLoadControl>();
            _audioSystem = Main.GetRegisteredComponent<AudioSystemBehavior>();

            _audioSystem.PlayAudio(_bgm);


            if (!_accountControl.Data.IsEmpty())
                yield return StartCoroutine(_backendControl.AwaitValidateRequest(OnFailedValidate, OnFinishValidate));

            _loginForm.Init();
            _loginForm.LoginAction = OnLoginButton;
            _logOutButton.gameObject.SetActive(loggedIn);
            _logOutButton.onClick.AddListener(OnLogoutButton);
            _startButton.onClick.AddListener(OnStartButtonPress);
        }

        private void OnStartButtonPress()
        {
            _audioSystem.PlayAudio(_buttonInteraction);
            if (loggedIn) _main.StartGame();
            _startButton.gameObject.SetActive(false);
            _loginForm.ToggleDisplay(true);
        }

        private void OnLoginButton(string username, string password)
        {
            _audioSystem.PlayAudio(_buttonInteraction);
            if (!_main.IsOnline)
                _main.StartGame();
            else
                StartCoroutine(_backendControl.AwaitLoginRequest(username, password, OnSuccessLogin, OnFailedLogin));
        }

        private void OnSuccessLogin(LoginResponse response)
        {
            _displaysSystem.GetOrCreateDisplay<PopUpDisplay>(_popUpDisplay)
                .Open(response.message, null);
            _main.SaveAccountData();
            _main.StartGame();
        }

        private void OnDestroy()
        {
            if (Main.Instance == null) return;
            _audioSystem.StopAudio(_bgm);
        }

        private void OnFailedLogin(UnityWebRequest webReq)
        {
            _backendControl.OpenErrorResponsePopUp(webReq, () => Debug.Log("Pop up Closed"));
        }

        private void OnFailedValidate(UnityWebRequest webReq)
        {
            _backendControl.OpenErrorResponsePopUp(webReq, () => Debug.Log("Pop up Closed"));
        }

        private void OnFinishValidate()
        {
            loggedIn = true;
        }

        private void OnLogoutButton()
        {
            _audioSystem.PlayAudio(_buttonInteraction);
            _accountControl.ClearData();
            _main.RemoveAccountData();

            _sceneLoadControl.ChangeScene("Title");
        }
    }
}