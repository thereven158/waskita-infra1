using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Agate.Waskita.API;
using Agate.Waskita.Request;
using Agate.Waskita.Responses;
using UserInterface.Display;
using Agate.WaskitaInfra1.PlayerAccount;
using A3.UserInterface;


namespace BackendIntegration
{
    public class BackendIntegrationController : MonoBehaviour
    {
        [SerializeField]
        private BlockDisplay _blockDisplayPrefab;

        [SerializeField]
        private PopUpDisplay _popUpDisplay;

        [SerializeField]
        private YesNoPopUpDisplay _yesNoDisplay;

        private WaskitaApi _api;
        private UiDisplaysSystem<GameObject> _displaysSystem;
        private PlayerAccountControl _accountControl;
        private BlockDisplay _blockDisplay;
        
        [SerializeField]
        [TextArea]
        private string _requestFailedMessage = "Request failed make sure you have stable internet connection";

        public void Init(WaskitaApi api, PlayerAccountControl accountControl, UiDisplaysSystem<GameObject> uiDisplay)
        {
            _api = api;
            _accountControl = accountControl;
            _displaysSystem = uiDisplay;
            _blockDisplay = _displaysSystem.GetOrCreateDisplay<BlockDisplay>(_blockDisplayPrefab);
        }

        public IEnumerator AwaitLoginRequest(string username, string password, Action<LoginResponse> onSuccess,
            Action<UnityWebRequest> onFail)
        {
            _blockDisplay.Open();
            UnityWebRequest webReq = _api.LoginUserRequest(
                new LoginRequest(WaskitaApi.ValidateData)
                {
                    userId = username,
                    password = password
                });
            yield return webReq.SendWebRequest();
            _blockDisplay.Close();
            if (webReq.isNetworkError)
            {
                onFail.Invoke(webReq);
                yield break;
            }

            switch (webReq.responseCode)
            {
                case 200:
                    LoginResponse response = JsonUtility.FromJson<LoginResponse>(webReq.downloadHandler.text);
                    _accountControl.SetData(response.AccountData());
                    onSuccess.Invoke(response);
                    break;
                default:
                    onFail.Invoke(webReq);
                    break;
            }
        }

        public IEnumerator AwaitValidateRequest(Action<UnityWebRequest> onFailed, Action onFinish)
        {
            bool requestCompleted = false;
            bool requesting = true;
            while (!requestCompleted)
            {
                _blockDisplay.Open();
                UnityWebRequest webReq = _api.ValidateRequest();
                yield return webReq.SendWebRequest();
                requesting = false;
                _blockDisplay.Close();
                if (webReq.isNetworkError)
                    OpenFailedRequestPopUp(
                        () => requesting = true,
                        () => requestCompleted = true);
                else
                {
                    requestCompleted = true;
                    switch (webReq.responseCode)
                    {
                        case 200:
                            JsonUtility.FromJson<LoginResponse>(webReq.downloadHandler.text);
                            onFinish.Invoke();
                            Debug.Log("Validate Success");
                            break;
                        default:
                            onFailed.Invoke(webReq);
                            break;
                    }
                }

                yield return new WaitUntil(() => requestCompleted || requesting);
            }
        }

        public void OpenPopupErrorResponse(UnityWebRequest webReq, Action onClose)
        {
            BasicResponse errorResponse = _api.HandleError(webReq);
            _displaysSystem.GetOrCreateDisplay<PopUpDisplay>(_popUpDisplay)
                .Open(errorResponse.error.code, onClose);
        }

        public void OpenFailedRequestPopUp(Action onRetry, Action onAbort)
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
    }
}