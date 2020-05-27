using A3.AudioControl;
using A3.AudioControl.Unity;
using A3.UserInterface;
using Agate.GlSim.Scene.Control.Map.Loader;
using Agate.Waskita.Responses;
using Agate.WaskitaInfra1.Backend.Integration;
using Agate.WaskitaInfra1.GameProgress;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using Agate.WaskitaInfra1.PlayerAccount;
using Agate.WaskitaInfra1.UserInterface.Display;
using Agate.WaskitaInfra1.UserInterface.Login;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using UnityEngine.UI;

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
        private GameProgressControl _gameProgressControl;
        private LevelProgressControl _levelProgressControl;
        private LevelControl _levelControl;
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

            _gameProgressControl = Main.GetRegisteredComponent<GameProgressControl>();
            _levelControl = Main.GetRegisteredComponent<LevelControl>();
            _levelProgressControl = Main.GetRegisteredComponent<LevelProgressControl>();

            if (!_accountControl.Data.IsEmpty())
            {
                yield return StartCoroutine(_backendControl.AwaitValidateRequest(OnFinishValidate, OnAbortValidate));
            }

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
            _accountControl.SetData(response.AccountData());
            Debug.Log(response.token);
            _main.SaveAccountData();
            StartCoroutine(_backendControl.AwaitValidateRequest(OnFinishValidate, OnAbortValidate));
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

        private void OnAbortValidate()
        {
            Debug.Log("Abort Validate");
        }

        private void OnFinishValidate(UnityWebRequest webReq)
        {
            Debug.Log("Validate Success");
            LoginResponse response = JsonUtility.FromJson<LoginResponse>(webReq.downloadHandler.text);
            Agate.Waskita.Responses.Data.GameProgressData data = JsonUtility.FromJson<Agate.Waskita.Responses.Data.GameProgressData>(webReq.downloadHandler.text);
            if (response == null)
            {
                Debug.Log("Response null");
                loggedIn = false;
            }
            else
            {
                loggedIn = true;
                _gameProgressControl.SetData(data.GameData());
                _levelProgressControl.LoadData(_levelControl.LevelProgress(response.levelProgress));
            }
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