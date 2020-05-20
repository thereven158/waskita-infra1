using System;
using System.Collections;
using A3.UserInterface;
using Agate.Waskita.API;
using Agate.Waskita.Request;
using Agate.Waskita.Responses;
using Agate.WaskitaInfra1.PlayerAccount;
using Agate.WaskitaInfra1.UserInterface.Login;
using BackendIntegration;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UserInterface.Display;

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
        private BlockDisplay _blockDisplayPrefab;

        [SerializeField]
        private PopUpDisplay _popUpDisplay;

        [SerializeField]
        private ConfirmationPopUpDisplay _confirmationDisplay;

        [SerializeField]
        private YesNoPopUpDisplay _yesNoDisplay;

        private bool loggedIn;

        private Main _main;
        private WaskitaApi _api;
        private UiDisplaysSystem<GameObject> _displaysSystem;
        private PlayerAccountControl _accountControl;


        [SerializeField]
        [TextArea]
        private string _requestFailedMessage = "Request failed make sure you have stable internet connection";

        private IEnumerator Start()
        {
            _main = Main.Instance;
            _api = Main.GetRegisteredComponent<WaskitaApi>();
            _displaysSystem = Main.GetRegisteredComponent<UiDisplaysSystemBehavior>();
            _accountControl = Main.GetRegisteredComponent<PlayerAccountControl>();
            Debug.Log(_accountControl.Data.AuthenticationToken);
            if (!_accountControl.Data.IsEmpty())
                yield return StartCoroutine(AwaitValidateRequest());
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
                StartCoroutine(AwaitLoginRequest(username, password));
        }

        private IEnumerator AwaitLoginRequest(string username, string password)
        {
            BlockDisplay blockDisplay = _displaysSystem.GetOrCreateDisplay<BlockDisplay>(_blockDisplayPrefab);
            blockDisplay.Open();
            UnityWebRequest webReq = _api.LoginUserRequest(
                new LoginRequest(WaskitaApi.ValidateData)
                {
                    userId = username,
                    password = password
                });
            yield return webReq.SendWebRequest();
            blockDisplay.Close();
            if (webReq.isNetworkError)
            {
                OpenFailedRequestPopUp(
                    () => _loginForm.SubmitLoginForm(),
                    () => _loginForm.ResetField(3));
                yield break;
            }

            switch (webReq.responseCode)
            {
                case 200:
                    Debug.Log(webReq.downloadHandler.text);
                    LoginResponse response = JsonUtility.FromJson<LoginResponse>(webReq.downloadHandler.text);
                    _accountControl.SetData(response.AccountData());
                    _main.SaveAccountData();
                    _main.StartGame();
                    break;
                default:
                    BasicResponse errorResponse = _api.HandleError(webReq);
                    _displaysSystem.GetOrCreateDisplay<PopUpDisplay>(_popUpDisplay)
                        .Open(errorResponse.error.code, null);
                    break;
            }
        }

        private void OpenFailedRequestPopUp(Action onRetry, Action onAbort)
        {
            _displaysSystem.GetOrCreateDisplay<YesNoPopUpDisplay>(_yesNoDisplay).Open(new YesNoPopUpViewData()
            {
                MessageText = _requestFailedMessage,
                NoAction = onAbort,
                YesAction = onRetry,
                NoButtonText = "Close",
                YesButtonText = "Retry"
            });
        }

        private IEnumerator AwaitValidateRequest()
        {
            bool requestCompleted = false;
            bool requesting = true;
            BlockDisplay blockDisplay = _displaysSystem.GetOrCreateDisplay<BlockDisplay>(_blockDisplayPrefab);
            while (!requestCompleted)
            {
                blockDisplay.Open();
                UnityWebRequest webReq = _api.ValidateRequest();
                yield return webReq.SendWebRequest();
                requesting = false;
                blockDisplay.Close();
                if (webReq.isNetworkError)
                    OpenFailedRequestPopUp(
                        () => requesting = true,
                        () => requestCompleted = true);
                else
                {
                    Debug.Log($"{webReq.responseCode} : {webReq.downloadHandler.text}");
                    requestCompleted = true;
                    switch (webReq.responseCode)
                    {
                        case 200:
                            JsonUtility.FromJson<LoginResponse>(webReq.downloadHandler.text);
                            loggedIn = true;
                            break;
                        default:
                            BasicResponse errorResponse = _api.HandleError(webReq);
                            break;
                    }
                }

                yield return new WaitUntil(() => requestCompleted || requesting);
            }
        }

        private void OnLogoutButton()
        {
            Main.Quit();
            // if (Main.Instance.OfflineMode) Main.Instance.StartGame();
            // else StartCoroutine(_backendIntegration.LoginProcess(username, password, Main.Instance.StartGame, null));
        }
    }
}