using UnityEngine;
using Agate.Waskita.Responses;
using Agate.Waskita.Request;
using UnityEngine.UI;
using Agate.Waskita.Request.Data;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Agate.Waskita.API.Example
{
    public class ExampleCoroutine : MonoBehaviour
    {
        public Button btnLogin, btnValidate, btnStartGame, btnSaveGame, BtnEndGame, btnRefreshToken;
        public InputField username, password, level, success;

        private WaskitaApi api;

        private string DummyUserId => username.text;

        private string Pass => password.text;

        private string Lvl => level.text;

        private string IsSuccess => success.text;

        private string token;

        private string deviceId;


        private void Awake()
        {
            api = new WaskitaApi();
            deviceId = SystemInfo.deviceUniqueIdentifier;

            #region Button Auth

            btnLogin.onClick.AddListener(delegate { StartCoroutine(Login()); });
            btnValidate.onClick.AddListener(delegate { StartCoroutine(Validate()); });

            #endregion

            #region Button Game Loop

            btnStartGame.onClick.AddListener(delegate { StartCoroutine(StartGame()); });
            btnSaveGame.onClick.AddListener(delegate { StartCoroutine(SaveGame()); });
            BtnEndGame.onClick.AddListener(delegate { StartCoroutine(EndGame()); });

            #endregion

            #region Button Refresh Token

            btnRefreshToken.onClick.AddListener(delegate { StartCoroutine(RefreshToken()); });

            #endregion


            api.SetAddress("https://gameserver-api-waskitainfra1-dev.gf.agatedev.net/");


            if (PlayerPrefs.GetString("token") != "") api.SetToken(PlayerPrefs.GetString("token"));
        }

        #region Auth EndPoint

        /// <summary>
        /// login request dengan parameter "username" "password" "deviceId" "requestId" "clientID" dan "Gameversion"
        /// akan return data player dan project yang sedang dikerjakan (jika ada)
        /// selain itu request ini akan return token dan durasi expire token, dimana token akan digunakan pada setiap hit API
        /// </summary>
        /// <returns></returns>
        private IEnumerator Login()
        {
            Debug.Log("Melakukan request login");
            LoginRequest request = new LoginRequest
            {
                userId = DummyUserId,
                password = Pass,
                clientID = "BmwzQACRCmddGbSXdUJIGw==",
                gameVersion = "1.0",
                deviceId = deviceId,
                requestId = Guid.NewGuid().ToString()
            };

            UnityWebRequest webRequest = api.LoginUserRequest(request);
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                HandleError(api.HandleError(webRequest));
                yield break;
            }

            LoginResponse response = JsonUtility.FromJson<LoginResponse>(webRequest.downloadHandler.text);
            Debug.Log("Menyimpan token dari hasil login");
            PlayerPrefs.SetString("token", response.token);
        }

        /// <summary>
        /// Validate request berfungsi untuk pengecekan apakah token sudah expired? server sedang online atw tidak?
        /// Selalu panggil fungsi ini jika user sudah pernah login sebelumnya
        /// pada dasarnya token akan expired dalam 30 hari, jika request ini return error Unauthorized maka token sudah expired
        /// </summary>
        /// <returns></returns>
        private IEnumerator Validate()
        {
            Debug.Log("Melakukan request validate");

            UnityWebRequest webRequest = api.ValidateRequest();

            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                HandleError(api.HandleError(webRequest));
                yield break;
            }

            Debug.Log(success.name);


            //Code setelah request selesai
        }

        #endregion

        #region GameLoop EndPoint

        /// <summary>
        /// StartGame request berfungsi untuk melakukan pengecekan pada save data
        /// Selalu panggil fungsi ini jika user akan memulai project
        /// pada dasarnya fungsi ini akan melakukan update status isDone dan membuat status isSuccess failed bila user berpindah project
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartGame()
        {
            Debug.Log("Melakukan request start game");
            StartGameRequest request = new StartGameRequest
            {
                level = Convert.ToInt32(Lvl),
                deviceId = deviceId,
                requestId = Guid.NewGuid().ToString()
            };

            UnityWebRequest webRequest = api.StartGameRequest(request);
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                HandleError(api.HandleError(webRequest));
                yield break;
            }

            //Code setelah request selesai
        }

        /// <summary>
        /// SaveGame request berfungsi untuk melakukan save data progress pada player
        /// </summary>
        /// <returns></returns>
        private IEnumerator SaveGame()
        {
            Debug.Log("Melakukan request save game");
            SaveGameRequest request = new SaveGameRequest()
            {
                lastCheckPoint = 1,
                currentDay = 1,
                tryCount = 3,
                dayCondition = new DayCondition
                {
                    weather = 1,
                    windStrength = 1,
                    soilCondition = 1
                },
                storedAnswers = new List<int>(),
                deviceId = deviceId,
                requestId = Guid.NewGuid().ToString()
            };

            UnityWebRequest webRequest = api.SaveGame(request);
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                HandleError(api.HandleError(webRequest));
                yield break;
            }

            //Code setelah request selesai
        }

        /// <summary>
        /// EndGame request berfungsi untuk melakukan save data
        /// Selalu panggil fungsi ini jika user sudah menyelesaikan project
        /// pada dasarnya fungsi ini akan melakukan update status isDone dan success pada project
        /// </summary>
        /// <returns></returns>
        private IEnumerator EndGame()
        {
            bool isRequesting = true;
            Debug.Log("Melakukan request End game");
            EndGameRequest request = new EndGameRequest
            {
                isSuccess = Convert.ToBoolean(IsSuccess),
                deviceId = deviceId,
                requestId = Guid.NewGuid().ToString()
            };

            api.EndGame(request, successResponse =>
            {
                isRequesting = false; //contoh 1 : rubah isRequesting setelah error / success muncul
                //ini merupakan ok request
            }, error =>
            {
                //ini merupakan bad request
                HandleError(error,
                    out isRequesting); //contoh 2 : rubah isRequesting setelah handle error / success selesai
            });

            yield return new WaitUntil(() => !isRequesting);

            //Code setelah request selesai
        }

        #endregion

        #region Refresh Token EndPoint

        /// <summary>
        /// Refresh Token request berfungsi untuk melakukan melakukan refresh pada token sebelum expire
        /// </summary>
        /// <returns></returns>
        private IEnumerator RefreshToken()
        {
            bool isRequesting = true;
            Debug.Log("Melakukan request Refresh Token");
            BasicRequest request = new BasicRequest
            {
                deviceId = deviceId,
                requestId = Guid.NewGuid().ToString()
            };

            api.RefreshToken(request, successResponse =>
            {
                isRequesting = false; //contoh 1 : rubah isRequesting setelah error / success muncul
                //ini merupakan ok request
                Debug.Log("Menyimpan token dari hasil Refresh");
                PlayerPrefs.SetString("token", successResponse.token);
            }, error =>
            {
                //ini merupakan bad request
                HandleError(error,
                    out isRequesting); //contoh 2 : rubah isRequesting setelah handle error / success selesai
            });

            yield return new WaitUntil(() => !isRequesting);

            //Code setelah request selesai
        }

        #endregion

        #region Helper Function

        /// <summary>
        /// handle error
        /// </summary>
        /// <param name="error"></param>
        /// <param name="isRequesting"></param>
        private static void HandleError(BasicResponse error, out bool isRequesting)
        {
            Debug.Log(error.error.code);
            Debug.Log(error.error.message);
            isRequesting = false;
        }

        private static void HandleError(BasicResponse error)
        {
            Debug.Log(error.error.code);
            Debug.Log(error.error.message);
        }

        #endregion
    }
}