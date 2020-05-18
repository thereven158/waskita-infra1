using UnityEngine;
using Agate.Waskita.API;
using Agate.Waskita.Responses;
using Agate.Waskita.Request;
using UnityEngine.UI;
using Agate.Waskita.Request.Data;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ExampleCoroutine : MonoBehaviour
{
    public Button btnLogin, btnValidate, btnStartGame, btnSaveGame, BtnEndGame, btnRefreshToken;
    public InputField username, password, level, success;

    private WaskitaApi api;

    private string dummyUserID
    {
        get { return username.text; }
    }

    private string pass
    {
        get { return password.text; }
    }

    private string lvl
    {
        get { return level.text; }
    }

    private string isSuccess
    {
        get { return success.text; }
    }

    private string token;

    private string deviceId;

    //private string requestId;
    private int readID = 0;
    private int projectID = 0;


    void Awake()
    {
        var test = GetComponent<MonoBehaviour>();
        api = new WaskitaApi();
        //requestId = SystemInfo.deviceUniqueIdentifier;
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


        //api.SetAddress("https://gameiptex.waskita.co.id/");
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
    public IEnumerator Login()
    {
        bool isRequesting = true;
        Debug.Log("Melakukan request login");
        LoginRequest request = new LoginRequest
        {
            userId = dummyUserID,
            password = pass,
            clientID = "BmwzQACRCmddGbSXdUJIGw==",
            gameVersion = 0,
            deviceId = deviceId,
            requestId = Guid.NewGuid().ToString()
        };

        UnityWebRequest webRequest = api.LoginUserRequest(request);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            HandleError(api.HandleError(webRequest), out isRequesting);
            yield break;
        }

        isRequesting = false;
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
    public IEnumerator Validate()
    {
        bool isRequesting = true;
        Debug.Log("Melakukan request validate");
        ValidateRequest request = new ValidateRequest
        {
            clientID = "BmwzQACRCmddGbSXdUJIGw==",
            gameVersion = 0,
            deviceId = deviceId,
            requestId = Guid.NewGuid().ToString()
        };

        UnityWebRequest webRequest = api.ValidateRequest(request);

        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            HandleError(api.HandleError(webRequest), out isRequesting);
            yield break;
        }

        isRequesting = false; //contoh 1 : rubah isRequesting setelah error / success muncul
        //ini merupakan ok request
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
    public IEnumerator StartGame()
    {
        bool isRequesting = true;
        Debug.Log("Melakukan request start game");
        StartGameRequest request = new StartGameRequest
        {
            level = Convert.ToInt32(lvl),
            deviceId = deviceId,
            requestId = Guid.NewGuid().ToString()
        };

        api.StartGameRequest(request, success =>
        {
            isRequesting = false; //contoh 1 : rubah isRequesting setelah error / success muncul
            //ini merupakan ok request
        }, error =>
        {
            //ini merupakan bad request
            HandleError(error, out isRequesting); //contoh 2 : rubah isRequesting setelah handle error / success selesai
        });

        yield return new WaitUntil(() => !isRequesting);

        //Code setelah request selesai
    }

    /// <summary>
    /// SaveGame request berfungsi untuk melakukan save data progress pada player
    /// </summary>
    /// <returns></returns>
    public IEnumerator SaveGame()
    {
        bool isRequesting = true;
        Debug.Log("Melakukan request save game");
        var answer = new List<QuizAnswer>();
        answer.Add(new QuizAnswer
        {
            question = 0,
            answer = 1,
            isTrue = true,
        });
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
            answer = answer,
            deviceId = deviceId,
            requestId = Guid.NewGuid().ToString()
        };

        api.SaveGame(request, success =>
        {
            isRequesting = false; //contoh 1 : rubah isRequesting setelah error / success muncul
            //ini merupakan ok request
        }, error =>
        {
            //ini merupakan bad request
            HandleError(error, out isRequesting); //contoh 2 : rubah isRequesting setelah handle error / success selesai
        });

        yield return new WaitUntil(() => !isRequesting);

        //Code setelah request selesai
    }

    /// <summary>
    /// EndGame request berfungsi untuk melakukan save data
    /// Selalu panggil fungsi ini jika user sudah menyelesaikan project
    /// pada dasarnya fungsi ini akan melakukan update status isDone dan success pada project
    /// </summary>
    /// <returns></returns>
    public IEnumerator EndGame()
    {
        bool isRequesting = true;
        Debug.Log("Melakukan request End game");
        EndGameRequest request = new EndGameRequest
        {
            isSuccess = Convert.ToBoolean(isSuccess),
            deviceId = deviceId,
            requestId = Guid.NewGuid().ToString()
        };

        api.EndGame(request, success =>
        {
            isRequesting = false; //contoh 1 : rubah isRequesting setelah error / success muncul
            //ini merupakan ok request
        }, error =>
        {
            //ini merupakan bad request
            HandleError(error, out isRequesting); //contoh 2 : rubah isRequesting setelah handle error / success selesai
        });

        yield return new WaitUntil(() => !isRequesting);

        //Code setelah request selesai
    }

    #endregion

    #region Refresh Token EndPoint

    /// <summary>
    /// Regresh Token request berfungsi untuk melakukan melakukan refresh pada token sebelum expire
    /// </summary>
    /// <returns></returns>
    public IEnumerator RefreshToken()
    {
        bool isRequesting = true;
        Debug.Log("Melakukan request Refresh Token");
        BasicRequest request = new BasicRequest
        {
            deviceId = deviceId,
            requestId = Guid.NewGuid().ToString()
        };

        api.RefreshToken(request, success =>
        {
            isRequesting = false; //contoh 1 : rubah isRequesting setelah error / success muncul
            //ini merupakan ok request
            Debug.Log("Menyimpan token dari hasil Refresh");
            PlayerPrefs.SetString("token", success.token);
        }, error =>
        {
            //ini merupakan bad request
            HandleError(error, out isRequesting); //contoh 2 : rubah isRequesting setelah handle error / success selesai
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
    public void HandleError(BasicResponse error, out bool isRequesting)
    {
        Debug.Log(error.error.id);
        Debug.Log(error.error.code);
        isRequesting = false;
    }

    #endregion
}