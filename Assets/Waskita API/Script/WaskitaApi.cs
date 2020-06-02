using System;
using System.Text;
using Agate.WaskitaInfra1.Server.Request;
using Agate.WaskitaInfra1.Server.Responses;
using UnityEngine;
using UnityEngine.Networking;

namespace Agate.WaskitaInfra1.Server.API
{
    public class WaskitaApi
    {
        private string _ipadd = "https://gameserver-api-waskitainfra1-dev.gf.agatedev.net/";
        private const string WASKITA_API = "https://west.waskita.co.id/page/tlcc/apiwest/login.php?";
        private string _token = string.Empty;

        /* List Request endpoint */

        #region Endpoint
        
        private const string _login = "Auth/Login";
        private const string _validate = "Auth/Validate";
        private const string _fetchGameProgress = "Game/GameProgress";
        private const string _fetchLevelProgress = "Game/LevelProgress";
        private const string _startLevel = "GameLoop/StartLevel";
        private const string _saveLevel = "GameLoop/SaveLevelProgress";
        private const string _endLevel = "GameLoop/EndLevel";
        private const string _refreshToken = "Token/Refresh";
        
        #endregion

        private static BasicRequest BaseData => new BasicRequest()
        {
            deviceId = SystemInfo.deviceUniqueIdentifier,
            requestId = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-zzz")
        };
        public static ValidateRequest ValidateData => new ValidateRequest(BaseData)
        {
            gameVersion = "1.0",
            clientID = "BmwzQACRCmddGbSXdUJIGw==",
        };


        private readonly UTF8Encoding _stringEncoder;

        private const int TIME_OUT = 60;
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
            BasicResponse response = new BasicResponse
            {
                error = new Error
                {
                    code = "3003",
                    message = "Unknown error, please check your internet connection"
                }
            };
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

        public UnityWebRequest StartLevelRequest(StartGameRequest request)
        {
            request.Set(BaseData);
            string param = JsonUtility.ToJson(request);
            return PostRequest(
                _ipadd + _startLevel,
                param,
                true);
        }

        public UnityWebRequest SaveLevelProgress(SaveGameRequest request)
        {
            request.Set(WaskitaApi.BaseData);
            string param = JsonUtility.ToJson(request);
            return PostRequest(_ipadd + _saveLevel, param, true);
        }

        public UnityWebRequest EndLevel(EndGameRequest request)
        {
            request.Set(WaskitaApi.BaseData);
            string param = JsonUtility.ToJson(request);
            return PostRequest(_ipadd + _endLevel, param, true);
        }

        #endregion

        #region Request Refresh Token

        public UnityWebRequest RefreshToken(BasicRequest request)
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
                timeout = TIME_OUT
            };
            uwr.uploadHandler.contentType = "application/json";
            if (requireauthentication) uwr.SetRequestHeader("Authorization", "Bearer " + _token);
            return uwr;
        }

        public UnityWebRequest GetRequest(string param)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(WASKITA_API + param);
            webRequest.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
            webRequest.timeout = TIME_OUT;
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