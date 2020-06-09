using A3.UserInterface;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using Agate.WaskitaInfra1.Server.API;
using Agate.WaskitaInfra1.Server.Request;
using Agate.WaskitaInfra1.Server.Responses;
using Agate.WaskitaInfra1.Server.Responses.Data;
using Agate.WaskitaInfra1.UserInterface.Display;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Agate.WaskitaInfra1.Backend.Integration
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
        private LevelControl _levelControl;
        private UiDisplaysSystem<GameObject> _displaysSystem;
        private BlockDisplay _blockDisplay;

        [SerializeField]
        [TextArea]
        private string _requestFailedMessage = "Request failed make sure you have stable internet connection";

        public void Init(WaskitaApi api, LevelControl levelControl, UiDisplaysSystem<GameObject> uiDisplay)
        {
            _api = api;
            _levelControl = levelControl;
            _displaysSystem = uiDisplay;
            _blockDisplay = _displaysSystem.GetOrCreateDisplay<BlockDisplay>(_blockDisplayPrefab);
        }

        public LevelProgressIntegration ParseLevelProgress(ServerLevelProgressData data)
        {
            return _levelControl.LevelProgress(data);
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

            yield return StartCoroutine(AwaitRequest(loginWebRequest, HandleResponse, onFail.Invoke)
            );
        }

        public IEnumerator AwaitValidateRequest(Action<UnityWebRequest> onFinish, Action onAbort)
        {
            UnityWebRequest webReq = _api.ValidateRequest();
            yield return StartCoroutine(AwaitRequest(webReq, onFinish, onAbort));
        }

        public IEnumerator AwaitStartLevelRequest(LevelData level, Action<UnityWebRequest> onFinish)
        {
            UnityWebRequest startGameReq = _api.StartLevelRequest(_levelControl.StartLevelRequest(level));

            yield return StartCoroutine(AwaitRequest(startGameReq, onFinish));
        }

        public IEnumerator AwaitSaveLevelProgressRequest(ILevelProgressData data, Action<UnityWebRequest> onFinish)
        {
            UnityWebRequest saveGameRequest = _api.SaveLevelProgress(data.SaveRequest());

            yield return StartCoroutine(AwaitRequest(saveGameRequest, onFinish));
        }


        public IEnumerator AwaitEndLevelRequest(LevelEvaluationData data, Action<UnityWebRequest> onFinish)
        {
            UnityWebRequest endLevelRequest = _api.EndLevel(data.EndLevelRequest());

            yield return StartCoroutine(AwaitRequest(endLevelRequest, onFinish));
        }

        public IEnumerator AwaitAbortLevelRequest(ILevelProgressData data, Action<UnityWebRequest> onFinish)
        {
            UnityWebRequest endLevelRequest = _api.EndLevel(data.AbortLevelRequest());
            yield return StartCoroutine(AwaitRequest(endLevelRequest, onFinish));
        }

        #region Request Await Handling Abstraction

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
                onFail?.Invoke(webRequest);
            else
                onSuccess?.Invoke(webRequest);
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
                onFinish?.Invoke(successRequest);
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
                onFinish?.Invoke(successRequest);
            }

            void OnRequestFailed(UnityWebRequest failedRequest)
            {
                OpenErrorResponsePopUp(
                    failedRequest,
                    () => requesting = true);
            }

            while (!requestCompleted)
            {
                yield return StartCoroutine(AwaitRequest(webRequest, OnRequestSuccess, OnRequestFailed));
                requesting = false;

                yield return new WaitUntil(() => requestCompleted || requesting);
            }

        }

        #endregion

        public BasicResponse HandleError(UnityWebRequest error)
        {
            if (error.isNetworkError)
                return new BasicResponse()
                {
                    error =
                    new Error
                    {
                        code = "-1",
                        message = _requestFailedMessage
                    }
                };
            return _api.HandleError(error);
        }

        #region PopUp Helper

        public void OpenErrorResponsePopUp(UnityWebRequest webReq, Action onClose)
        {
            BasicResponse errorResponse = HandleError(webReq);
            _displaysSystem.GetOrCreateDisplay<PopUpDisplay>(_popUpDisplay)
                .Open(errorResponse.error.message, onClose);
        }

        private void OpenRetryRequestPopUp(UnityWebRequest webReq, Action onRetry, Action onAbort)
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