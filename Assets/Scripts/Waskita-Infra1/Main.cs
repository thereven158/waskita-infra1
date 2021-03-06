﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using A3.Utilities;
using A3.UserInterface;
using Agate.GlSim.Scene.Control.Map.Loader;
using Agate.WaskitaInfra1.Server.API;
using Agate.WaskitaInfra1.GameProgress;
using Agate.WaskitaInfra1.Level;
using Agate.WaskitaInfra1.LevelProgress;
using Agate.WaskitaInfra1.PlayerAccount;
using Agate.WaskitaInfra1.Backend.Integration;
using UnityEngine;
using UnityEngine.SceneManagement;
using A3.AudioControl.Unity;
using Agate.WaskitaInfra1.UserInterface;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Agate.WaskitaInfra1
{
    public class Main : MonoBehaviour
    {
        #region Singleton

        public static Main Instance { get; private set; }

        private bool SetupInstance()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                return true;
            }

            Destroy(gameObject);
            return false;
        }

        #endregion

        #region Game Component

        private PlayerAccountControl _playerAccount;
        private GameProgressControl _gameProgress;
        private LevelProgressControl _levelProgress;
        private WaskitaApi _api;
        private UiDisplaysSystem<GameObject> _displaySystem;

        [SerializeField]
        private AudioSystemBehavior _audioSystem = default;

        [SerializeField]
        private GameplaySceneLoadControl _sceneLoader = default;

        [SerializeField]
        private LevelControl _levelControl = default;

        [SerializeField]
        private BackendIntegrationController _backendIntegrationControl = default;

        [SerializeField]
        [Tooltip("Location is relative to Persistent Data Path")]
        private string _playerDataFilepath = default;

        [SerializeField]
        private LoadingDisplay _loadingDisplayPrefab = default;

        public string PlayerDataFilepath => Path.Combine(Application.persistentDataPath, _playerDataFilepath);

        private LoadingDisplay _loadingDisplay;
        private LoadingDisplay LoadingDisplay
        {
            get
            {
                _loadingDisplay = _loadingDisplay ?? _displaySystem.GetOrCreateDisplay<LoadingDisplay>(_loadingDisplayPrefab);
                return _loadingDisplay;
            }
        }

        #endregion

        #region Fields / Attributes

        #region Scene Settings

        /// <summary>
        /// temp storage to store first loaded scene name. used in case main is not loaded first
        /// </summary>
        private static string _firstLoadedSceneName = string.Empty;

        /// <summary>
        /// serializable storage to store scene that need to be loaded after main behavior is ready.
        /// </summary>
        [SerializeField, HideInInspector]
        private string _firstSceneToLoad = string.Empty;

        #endregion

        #region Game Setting

        [SerializeField]
        private ScriptablePlayerGameData _testPlayerData = default;

        [SerializeField]
        private int _targetFPS = 30;

        #endregion

        [HideInInspector]
        public bool UiLoaded;

        [SerializeField]
        private bool _isOnline = default;

        public bool IsOnline => _isOnline;

        private IPlayerGameData _gameData;

        public static event Action OnLogOut;

        #endregion

        #region Unity Event Function

        private void Awake()
        {
            if (!SetupInstance()) return;
            SetAppSystemSetting();

#if ONLINE
                _isOnline = true;
#endif

            _playerAccount = new PlayerAccountControl();
            _gameProgress = new GameProgressControl();
            _levelProgress = new LevelProgressControl();
            _api = new WaskitaApi();
            _audioSystem.Init();
            RegisterComponents(_playerAccount, _gameProgress, _levelProgress, _levelControl, _sceneLoader, _api, _backendIntegrationControl, _audioSystem);
            SceneManager.LoadScene("UserInterface", LoadSceneMode.Additive);
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => UiLoaded);

            _displaySystem = GetRegisteredComponent<UiDisplaysSystemBehavior>();
            _sceneLoader.OnChangeScene += OnSceneChanges;
            _backendIntegrationControl.Init(
                _api,
                _levelControl,
                _displaySystem);

            if (!IsOnline)
                LoadDummyData();
            else
                LoadAccountData();

            LoadFirstScene();
        }

        #endregion

        #region Setup Methods

        private void OnSceneChanges()
        {
            LoadingDisplay.Open();
            SceneManager.sceneLoaded += (scene, loadMode) => LoadingDisplay.Close();
            _sceneLoader.OnLoadProgress = (progress) =>
            {
                LoadingDisplay.UpdateDisplay(progress);
            };
        }

        private void SetAppSystemSetting()
        {
            Application.targetFrameRate = _targetFPS;
            QualitySettings.vSyncCount = 0;
        }

        private void LoadDummyData()
        {
            _gameData = _testPlayerData;
            _playerAccount.SetData(_gameData.GetAccountData());
            _gameProgress.SetData(_gameData.GetProgressData());
            _levelProgress.LoadData(_gameData.LevelProgressData());
        }

        private void LoadFirstScene()
        {
            if (!string.IsNullOrEmpty(_firstLoadedSceneName))
            {
                _sceneLoader.ChangeScene(_firstLoadedSceneName);

                return;
            }

            _sceneLoader.ChangeScene(_firstSceneToLoad);
        }

        #endregion

        #region Public Function / Method

        public static void Quit()
        {
            Application.Quit();
        }
        public static void LogOut()
        {
            Instance._api.SetToken(string.Empty);
            Instance._playerAccount.ClearData();
            Instance._gameProgress.ClearData();
            Instance._levelProgress.ClearData();
            Instance.RemoveAccountData();
            OnLogOut?.Invoke();
            Instance._sceneLoader.ChangeScene(Instance._firstSceneToLoad);

        }

        public void StartGame()
        {
            if (!IsOnline && _playerAccount.Data.IsEmpty()) LoadDummyData();
            _sceneLoader.ChangeScene("PreparationPhase");
        }

        public void SaveAccountData()
        {
            FileOperation.WriteFile(PlayerDataFilepath, _playerAccount.Data);
            SetAuthToken();
        }
        private void LoadAccountData()
        {
            _playerAccount.SetData(FileOperation.ReadFile<PlayerAccountData>(PlayerDataFilepath));
            SetAuthToken();
        }

        public void RemoveAccountData()
        {
            FileOperation.DeleteFile(PlayerDataFilepath);
        }

        private void SetAuthToken()
        {
            _api.SetToken(_playerAccount.Data.AuthenticationToken);
        }
        #endregion

        #region Editor

#if UNITY_EDITOR
        [SerializeField]
        private SceneAsset _firstSceneAssetToLoad = null;

        protected void OnValidate()
        {
            _firstSceneToLoad = !_firstSceneAssetToLoad ? string.Empty : _firstSceneAssetToLoad.name;
        }

#endif

        #endregion

        #region RuntimeInitializeOnLoad Method

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoadRuntimeMethod()
        {
            Debug.Log("Before first Scene loaded");

            if (SceneManager.GetActiveScene().name == "Main") return;
            _firstLoadedSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnAfterSceneLoadRuntimeMethod()
        {
            Debug.Log("After first Scene loaded");
        }

        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeMethodLoad()
        {
            Debug.Log("RuntimeMethodLoad: After first Scene loaded");
        }

        #endregion

        #region Component Holder

        private Dictionary<Type, object> _component;

        public static void RegisterComponents(params object[] cmpnts)
        {
            foreach (object component in cmpnts)
                RegisterComponent(component);
        }

        public static T GetRegisteredComponent<T>() where T : class
        {
            if (Instance._component == null) return null;
            if (Instance._component.ContainsKey(typeof(T))) return (T)Instance._component[typeof(T)];
            foreach (KeyValuePair<Type, object> keyValuePair in Instance._component)
                if (keyValuePair.Value is T value)
                    return value;
            return null;
        }

        public static void RegisterComponent(object cmpn)
        {
            if (cmpn == null) return;
            RegisterComponent(cmpn.GetType(), cmpn);
        }

        private static void RegisterComponent(Type T, object cmpn)
        {
            if (cmpn == null) return;
            if (Instance._component == null) Instance._component = new Dictionary<Type, object>();
            if (Instance._component.ContainsKey(T)) return;
            Instance._component.Add(T, cmpn);
        }

        #endregion
    }
}