using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Agate.Waskita.API;
using Agate.Waskita.Request;
using Agate.Waskita.Responses;
using Agate.WaskitaInfra1.UserInterface.Display;
using A3.UserInterface;


namespace BackendIntegration
{
    public class BackendIntegrationController : MonoBehaviour
    {
        [SerializeField]
        private BlockDisplay _blockDisplayPrefab = default;

        [SerializeField]
        private PopUpDisplay _popUpDisplay = default;

        [SerializeField]
        private YesNoPopUpDisplay _yesNoDisplay = default;

        private WaskitaApi _api;
        private UiDisplaysSystem<GameObject> _displaysSystem;
        private BlockDisplay _blockDisplay;

        [SerializeField]
        [TextArea]
        private string _requestFailedMessage = "Request failed make sure you have stable internet connection";

        public void Init(WaskitaApi api, UiDisplaysSystem<GameObject> uiDisplay)
        {
            _api = api;
            _displaysSystem = uiDisplay;
            _blockDisplay = _displaysSystem.GetOrCreateDisplay<BlockDisplay>(_blockDisplayPrefab);
        }

        public IEnumerator AwaitLoginRequest(string username, string password, Action<LoginResponse> onSuccess,
            Action<UnityWebRequest> onFail)
        {
            UnityWebRequest loginWebRequest = _api.LoginUserRequest(
                new LoginRequest(WaskitaApi.ValidateData)
                {
                    userId = username,
                    password = password
                });

            void HandleResponse(UnityWebRequest unityWebRequest)
            {
                switch (unityWebRequest.responseCode)
                {
                    case 200:
                        onSuccess?.Invoke(
                            JsonUtility.FromJson<LoginResponse>(loginWebRequest.downloadHandler.text));
                        break;
                    default:
                        onFail?.Invoke(unityWebRequest);
                        break;
                }
            }

            yield return StartCoroutine(
                AwaitRequest(
                    loginWebRequest,
                    HandleResponse,
                    onFail.Invoke)
            );
        }

        /// <summary>
        /// handle web request dispatch without any additional doodad behind it
        /// </summary>
        /// <param name="webRequest">web request to sent</param>
        /// <param name="onSuccess">callback when request is sent without any network problem</param>
        /// <param name="onFail"></param>
        /// <returns></returns>
        public IEnumerator AwaitRequest(UnityWebRequest webRequest, Action<UnityWebRequest> onSuccess,
            Action<UnityWebRequest> onFail)
        {
            _blockDisplay.Open();
            yield return webRequest.SendWebRequest();
            _blockDisplay.Close();
            if (webRequest.isNetworkError)
                onFail.Invoke(webRequest);
            else
                onSuccess.Invoke(webRequest);
        }

        /// <summary>
        /// handle web request dispatch that must be completed with an option to abort
        /// </summary>
        /// <param name="webRequest">web request to be sent</param>
        /// <param name="onFinish">callback when request finished without any network error</param>
        /// <param name="onAbort">callback when request is aborted upon encountering network error</param>
        /// <returns></returns>
        public IEnumerator AwaitRequest(UnityWebRequest webRequest, Action<UnityWebRequest> onFinish, Action onAbort)
        {
            bool requestCompleted = false;
            bool requesting = true;

            void OnRequestSuccess(UnityWebRequest successRequest)
            {
                requestCompleted = true;
                onFinish.Invoke(successRequest);
            }

            void OnRequestFailed(UnityWebRequest failedRequest)
            {
                OpenRetryRequestPopUp(failedRequest,
                    () => requesting = true,
                    () => requestCompleted = true);
            }

            while (!requestCompleted)
            {
                yield return StartCoroutine(
                    AwaitRequest(
                        webRequest,
                        OnRequestSuccess,
                        OnRequestFailed
                        )
                    );
                requesting = false;

                yield return new WaitUntil(() => requestCompleted || requesting);
            }

            if (webRequest.isNetworkError) onAbort?.Invoke();
        }

        /// <summary>
        /// handle web request Dispatch that must be completed without any option to abort
        /// </summary>
        /// <param name="webRequest">web request to be sent</param>
        /// <param name="onFinish">callback when request finished without any network error</param>
        /// <returns></returns>
        public IEnumerator AwaitRequest(UnityWebRequest webRequest, Action<UnityWebRequest> onFinish)
        {
            bool requestCompleted = false;
            bool requesting = true;
            void OnRequestSuccess(UnityWebRequest successRequest)
            {
                requestCompleted = true;
                onFinish.Invoke(successRequest);
            }

            void OnRequestFailed(UnityWebRequest failedRequest)
            {
                OpenErrorResponsePopUp(
                    failedRequest,
                    () => requesting = true);
            }

            while (!requestCompleted)
            {
                yield return StartCoroutine(
                    AwaitRequest(
                        webRequest,
                        OnRequestSuccess,
                        OnRequestFailed
                    )
                );
                requesting = false;

                yield return new WaitUntil(() => requestCompleted || requesting);
            }

        }

        public IEnumerator AwaitValidateRequest(Action<UnityWebRequest> onFinish, Action onAbort)
        {
            UnityWebRequest webReq = _api.ValidateRequest();
            yield return StartCoroutine(AwaitRequest(webReq, onFinish, onAbort));
        }
        
        public BasicResponse HandleError(UnityWebRequest error)
        {
            BasicResponse response = new BasicResponse
            {
                error = new Error
                {
                    code = "3003",
                    message = "Unknown error, please check your internet connection"
                }
            };
            if (error.isNetworkError)
            {
                response.error.code = "-1";
                response.error.message = _requestFailedMessage;
            }
            switch (error.responseCode)
            {
                case 200:
                    response.error.code = "2017";
                    response.error.message = "Username & Password  not found";
                    break;

                case 400:
                    string text = error.downloadHandler.text;
                    response = JsonUtility.FromJson<BasicResponse>(text);
                    break;

                case 401:
                    response.error.code = "2018";
                    response.error.message = "Unauthorized, please re-login";
                    break;

                case 403:
                    response.error.code = "2019";
                    response.error.message = "Forbidden Please, re-login";
                    break;

                case 500:
                    response.error.code = "3002";
                    response.error.message = "Internal server error / server on maintenance";
                    break;
            }

            return response;
        }
        
        #region PopUp Helper

        public void OpenErrorResponsePopUp(UnityWebRequest webReq, Action onClose)
        {
            BasicResponse errorResponse = HandleError(webReq);
            _displaysSystem.GetOrCreateDisplay<PopUpDisplay>(_popUpDisplay)
                .Open(errorResponse.error.message, onClose);
        }

        private void OpenRetryRequestPopUp(UnityWebRequest webReq,Action onRetry, Action onAbort)
        {
            BasicResponse errorResponse = HandleError(webReq);
            _displaysSystem.GetOrCreateDisplay<YesNoPopUpDisplay>(_yesNoDisplay).Open(new YesNoPopUpViewData()
            {
                MessageText = errorResponse.error.message,
                NoAction = onAbort,
                YesAction = onRetry,
                NoButtonText = "Close",
                YesButtonText = "Retry"
            });
        }
        
        #endregion

    }
}