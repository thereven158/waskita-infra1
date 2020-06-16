using A3.AudioControl;
using A3.AudioControl.Unity;
using A3.UserInterface;
using Agate.GlSim.Scene.Control.Map.Loader;
using Agate.WaskitaInfra1.Backend.Integration;
using Agate.WaskitaInfra1.GameProgress;
using Agate.WaskitaInfra1.LevelProgress;
using Agate.WaskitaInfra1.PlayerAccount;
using Agate.WaskitaInfra1.Server.Responses;
using Agate.WaskitaInfra1.UserInterface;
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

        [SerializeField]
        private string FailedValidationMessage = "Validasi Gagal, tolong lakukan login ulang";

        [SerializeField]
        private string SuccessLoginMessage = "Selamat datang ";


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
        private SettingDisplay _settingDisplay;
        private BackendIntegrationController _backendControl;
        private AudioSystem<AudioClip, AudioMixerGroup> _audioSystem;

        private IEnumerator Start()
        {
            _main = Main.Instance;
            _accountControl = Main.GetRegisteredComponent<PlayerAccountControl>();
            _gameProgressControl = Main.GetRegisteredComponent<GameProgressControl>();
            _levelProgressControl = Main.GetRegisteredComponent<LevelProgressControl>();
            _sceneLoadControl = Main.GetRegisteredComponent<GameplaySceneLoadControl>();

            _displaysSystem = Main.GetRegisteredComponent<UiDisplaysSystemBehavior>();
            _settingDisplay = Main.GetRegisteredComponent<SettingDisplay>();

            _audioSystem = Main.GetRegisteredComponent<AudioSystemBehavior>();

            _backendControl = Main.GetRegisteredComponent<BackendIntegrationController>();

            _audioSystem.PlayAudio(_bgm);
            _loginForm.Init();
            _logOutButton.gameObject.SetActive(false);

            _loginForm.LoginAction = OnLoginButton;
            _logOutButton.onClick.AddListener(OnLogoutButton);
            _startButton.onClick.AddListener(OnStartButtonPress);

            if(!_main.IsOnline)
                loggedIn = true;
            else if (!_accountControl.Data.IsEmpty())
                yield return StartCoroutine(_backendControl.AwaitValidateRequest(OnFinishValidate, OnAbortValidate));

            _logOutButton.gameObject.SetActive(loggedIn);
        }


        private void OnStartButtonPress()
        {
            _audioSystem.PlayAudio(_buttonInteraction);
            if (loggedIn) _main.StartGame();
            _startButton.gameObject.SetActive(false);
            if (loggedIn) return;
            _loginForm.ToggleDisplay(true);
        }

        private void OnLoginButton(string username, string password)
        {
            _audioSystem.PlayAudio(_buttonInteraction);
            if (username == "" || password == "")
            {
                _displaysSystem.GetOrCreateDisplay<PopUpDisplay>(_popUpDisplay)
                .Open("NIK dan Password Harus Diisi", null);
                return;
            }

            if (!_main.IsOnline)
                _main.StartGame();
            else
                StartCoroutine(_backendControl.AwaitLoginRequest(username, password, OnSuccessLogin, OnFailedLogin));
        }

        private void OnSuccessLogin(LoginResponse response)
        {
            _displaysSystem.GetOrCreateDisplay<PopUpDisplay>(_popUpDisplay)
                .Open(SuccessLoginMessage + response.name + ".", null);
            _accountControl.SetData(response.AccountData());
            _settingDisplay.NikText = response.name;
            _main.SaveAccountData();
            SetDataProgress(response);
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
            if (webReq.responseCode != 200)
            {
                _displaysSystem.GetOrCreateDisplay<PopUpDisplay>(_popUpDisplay).Open(FailedValidationMessage, null);
                Main.Instance.RemoveAccountData();
                return;
            }

            LoginResponse response = JsonUtility.FromJson<LoginResponse>(webReq.downloadHandler.text);
            if (response == null)
            {
                Debug.Log("Response null");
                loggedIn = false;
            }
            else
            {
                _settingDisplay.NikText = response.name;
                loggedIn = true;
                SetDataProgress(response);
            }
        }

        private void SetDataProgress(LoginResponse response)
        {
            _gameProgressControl.SetData(response.gameProgress);
            _levelProgressControl.LoadData(_backendControl.ParseLevelProgress(response.levelProgress));
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