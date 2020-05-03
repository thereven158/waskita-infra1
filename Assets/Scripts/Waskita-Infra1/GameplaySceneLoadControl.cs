using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Agate.GlSim.Scene.Control.Map.Loader
{
    public class GameplaySceneLoadControl : MonoBehaviour
    {
        #region Fields / Attributes
        
        [SerializeField, HideInInspector]
        private string _titleSceneName = string.Empty;
        
        #region Editor

        #if UNITY_EDITOR
        [SerializeField]
        private SceneAsset _titleScene = null;

        protected void OnValidate()
        {
            _titleSceneName = !_titleScene ? string.Empty : _titleScene.name;
        }

        #endif

        #endregion
        
        private string _currentSceneName;

        public Action<float> OnLoadProgress;
        public Action<float> OnUnloadProgress;

        public AsyncOperation AsyncOperation { get; private set; }

        #endregion

        #region Scene Change Coroutine

        private IEnumerator ChangeSceneProcess(string sceneName)
        {
            yield return StartCoroutine(UnloadCurrentScene());
            yield return StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator UnloadCurrentScene()
        {
            if (string.IsNullOrEmpty(_currentSceneName)) yield break;
            AsyncOperation = SceneManager.UnloadSceneAsync(_currentSceneName);
            while (!AsyncOperation.isDone)
            {
                OnUnloadProgress?.Invoke(AsyncOperation.progress);
                yield return new WaitForEndOfFrame();
            }

            OnUnloadProgress?.Invoke(AsyncOperation.progress);
            _currentSceneName = string.Empty;
            OnUnloadProgress = null;
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!AsyncOperation.isDone)
            {
                OnLoadProgress?.Invoke(AsyncOperation.progress);
                yield return new WaitForEndOfFrame();
            }

            OnLoadProgress?.Invoke(AsyncOperation.progress);
            _currentSceneName = sceneName;
            OnLoadProgress = null;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_currentSceneName));
        }

        #endregion

        #region Public Methods

        public void ChangeScene(string sceneName)
        {
            StartCoroutine(ChangeSceneProcess(sceneName));
        }

        public void GoToTitle()
        {
            ChangeScene(_titleSceneName);
        }

        #endregion
    }
}