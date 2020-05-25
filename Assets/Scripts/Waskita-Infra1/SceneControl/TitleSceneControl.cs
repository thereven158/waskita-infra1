using System;
using System.Collections;
using A3.UserInterface;
using Agate.Waskita.Responses;
using Agate.WaskitaInfra1.PlayerAccount;
using Agate.WaskitaInfra1.UserInterface.Login;
using BackendIntegration;
using UnityEngine;
using UnityEngine.UI;
using UserInterface.Display;
using UnityEngine.Networking;

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
        private PopUpDisplay _popUpDisplay;

        [SerializeField]
        private ConfirmationPopUpDisplay _confirmationDisplay;

        [SerializeField]
        private YesNoPopUpDisplay _yesNoDisplay;

        private bool loggedIn;

        private Main _main;
        private PlayerAccountControl _accountControl;
        private UiDisplaysSystem<GameObject> _displaysSystem;

        private BackendIntegrationController _backendControl;

        private IEnumerator Start()
        {
            _main = Main.Instance;
            _accountControl = Main.GetRegisteredComponent<PlayerAccountControl>();
            _backendControl = Main.GetRegisteredComponent<BackendIntegrationController>();
            _displaysSystem = Main.GetRegisteredComponent<UiDisplaysSystemBehavior>();

            _accountControl.ClearData();
            Debug.Log(_accountControl.Data.AuthenticationToken);
            if (!_accountControl.Data.IsEmpty()) {
                yield return StartCoroutine(_backendControl.AwaitValidateRequest(OnFailedValidate, OnFinishValidate));
            }
                
            Debug.Log(loggedIn);
            _loginForm.Init();
            _loginForm.LoginAction = OnLoginButton;
            _logOutButton.gameObject.SetActive(loggedIn);
            _logOutButton.onClick.AddListener(OnLogoutButton);
            _startButton.onClick.AddListener(OnStartButtonPress);
        }

        private void OnStartButtonPress()
        {
            if (loggedIn) _main.StartGame();
            _startButton.gameObject.SetActive(false);
            _loginForm.ToggleDisplay(true);
        }

        private void OnLoginButton(string username, string password)
        {
            if (!_main.IsOnline)
                _main.StartGame();
            else
                StartCoroutine(_backendControl.AwaitLoginRequest(username, password, OnSuccessLogin, OnFailedLogin));
        }

        private void OnSuccessLogin(LoginResponse response)
        {
            _displaysSystem.GetOrCreateDisplay<PopUpDisplay>(_popUpDisplay)
                .Open(response.message, null);
            _main.StartGame();
        }

        private void OnFailedLogin(UnityWebRequest webReq)
        {
            _backendControl.OpenPopupErrorResponse(webReq, () => Debug.Log("Pop up Closed"));
        }

        private void OnFailedValidate(UnityWebRequest webReq)
        {
            _backendControl.OpenPopupErrorResponse(webReq, () => Debug.Log("Pop up Closed"));
        }

        private void OnFinishValidate()
        {
            loggedIn = true;
        }

        private void OnLogoutButton()
        {
            _accountControl.ClearData();
            //Main.Quit();
            // if (Main.Instance.OfflineMode) Main.Instance.StartGame();
            // else StartCoroutine(_backendIntegration.LoginProcess(username, password, Main.Instance.StartGame, null));
        }

    }
}