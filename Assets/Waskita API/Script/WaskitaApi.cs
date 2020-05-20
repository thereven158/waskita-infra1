using System;
using System.Text;
using Agate.Waskita.Request;
using Agate.Waskita.Responses;
using UnityEngine;
using UnityEngine.Networking;

namespace Agate.Waskita.API
{
    public class WaskitaApi
    {
        private string _ipadd = "https://gameserver-api-waskitainfra1-dev.gf.agatedev.net/";

        //private string _ipadd = "https://game-waskita-iptex-stag.agatedev.net/";
        private readonly string _waskitaAPI = "https://west.waskita.co.id/page/tlcc/apiwest/login.php?";
        private string _token = String.Empty;

        /* List Reqeust endpoint */

        #region Endpoint
        
        private const string _login = "Auth/Login";
        private const string _validate = "Auth/Validate";

        private const string _startGame = "GameLoop/StartGame";
        private const string _saveGame = "GameLoop/SaveGame";
        private const string _endGame = "GameLoop/EndGame";

        private const string _refreshToken = "Token/Refresh";
        
        #endregion

        public static BasicRequest BaseData = new BasicRequest()
        {
            deviceId = SystemInfo.deviceUniqueIdentifier,
            requestId = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-zzz")
        };
        public static ValidateRequest ValidateData = new ValidateRequest(BaseData)
        {
            gameVersion = 0,
            clientID = "BmwzQACRCmddGbSXdUJIGw==",
        };


        private UTF8Encoding _stringEncoder;

        private const int _timeOut = 60;
        /* End point */

        public WaskitaApi()
        {
            _stringEncoder = new UTF8Encoding();
        }

        public void SetAddress(string add)
        {
            _ipadd = add;
        }

        public void SetToken(string token)
        {
            _token = token;
        }

        public BasicResponse HandleError(UnityWebRequest error)
        {
            Debug.Log("ini error " + error.downloadHandler.text);
            BasicResponse response = new BasicResponse
            {
                error = new Error
                {
                    id = "3003",
                    code = "Unknown error, please check your intenet connection"
                }
            };
            switch (error.responseCode)
            {
                case 200:
                    response.error.id = "2017";
                    response.error.code = "Username & Password  not found";
                    break;

                case 400:
                    string text = error.downloadHandler.text;
                    response = JsonUtility.FromJson<BasicResponse>(text);
                    break;

                case 401:
                    response.error.id = "2018";
                    response.error.code = "Unauthorized, please re-login";
                    break;

                case 403:
                    response.error.id = "2019";
                    response.error.code = "Forbidden Please, re-login";
                    break;

                case 500:
                    response.error.id = "3002";
                    response.error.code = "Internal server error / server on maintenance";
                    break;
            }

            return response;
        }

        #region Request Auth

        public UnityWebRequest LoginUserRequest(LoginRequest request)
        {
            string requestJson = JsonUtility.ToJson(request);
            return PostRequest(
                _ipadd + _login,
                requestJson,
                false);
        }

        public UnityWebRequest ValidateRequest()
        {
            string param = JsonUtility.ToJson(ValidateData);
            return PostRequest(
                _ipadd + _validate,
                param,
                true);
        }

        #endregion

        #region Request Game Loop

        public UnityWebRequest StartGameRequest(StartGameRequest request, Action<BasicResponse> callbackSuccess,
            Action<BasicResponse> callbackError)
        {
            string param = JsonUtility.ToJson(request);
            return PostRequest(
                _ipadd + _startGame,
                param,
                true);
        }

        public UnityWebRequest SaveGame(SaveGameRequest request, Action<BasicResponse> callbackSuccess,
            Action<BasicResponse> callbackError)
        {
            string param = JsonUtility.ToJson(request);
            return PostRequest(_ipadd + _saveGame, param, true);
        }

        public UnityWebRequest EndGame(EndGameRequest request, Action<BasicResponse> callbackSuccess,
            Action<BasicResponse> callbackError)
        {
            string param = JsonUtility.ToJson(request);
            return PostRequest(_ipadd + _endGame, param, true);
        }

        #endregion

        #region Request Refresh Token

        public UnityWebRequest RefreshToken(BasicRequest request, Action<LoginResponse> callbackSuccess,
            Action<BasicResponse> callbackError)
        {
            string param = JsonUtility.ToJson(request);
            return PostRequest(_ipadd + _refreshToken, param, true);
        }

        #endregion

        #region Request

        private UnityWebRequest PostRequest(
            string url, string data, bool requireAuthentication)
        {
            return PostRequest(url, _stringEncoder.GetBytes(data), requireAuthentication);
        }

        private UnityWebRequest PostRequest(
            string url,
            byte[] data,
            bool requireauthentication = false)
        {
            UnityWebRequest uwr = new UnityWebRequest()
            {
                method = "POST",
                url = url,
                chunkedTransfer = false,
                uploadHandler = new UploadHandlerRaw(data),
                downloadHandler = new DownloadHandlerBuffer(),
                certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey(),
                timeout = _timeOut
            };
            uwr.uploadHandler.contentType = "application/json";
            if (requireauthentication) uwr.SetRequestHeader("Authorization", "Bearer " + _token);
            return uwr;
        }

        private UnityWebRequest GetRequest(string param)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(_waskitaAPI + param);
            webRequest.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
            webRequest.timeout = _timeOut;
            return webRequest;
        }

        #endregion
    }

    internal class AcceptAllCertificatesSignedWithASpecificKeyPublicKey : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

}