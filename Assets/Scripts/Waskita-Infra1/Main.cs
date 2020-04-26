using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Agate.SugiSuma
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
        private int _targetFPS = 30;

        #endregion


        #endregion

        #region Unity Event Function

        private void Awake()
        {
            if (!SetupInstance()) return;

            SetAppSystemSetting();
            if (!string.IsNullOrEmpty(_firstLoadedSceneName))
            {
                SceneManager.LoadScene(_firstLoadedSceneName, LoadSceneMode.Single);
                return;
            }
            
            Assert.IsNotNull(_firstSceneToLoad, "Variable _firstSceneToLoad is null.");

            SceneManager.LoadScene(_firstSceneToLoad, LoadSceneMode.Single);
        }


        #endregion

        #region Setup Methods
        
        private void SetAppSystemSetting()
        {
            Application.targetFrameRate = _targetFPS;
            QualitySettings.vSyncCount = 0;
        }
        
        #endregion

        #region Public Function / Method

        private static void Quit()
        {
            Application.Quit();
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
            if (Instance._component.ContainsKey(typeof(T))) return (T) Instance._component[typeof(T)];
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