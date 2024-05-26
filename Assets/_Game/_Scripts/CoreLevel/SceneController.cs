using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LuisLabs.CoreLevel
{
    public class SceneController : MonoBehaviour, ISceneController
    {
        private readonly List<string> _additiveScenes = new();
        private string _currentScene;

        public Action OnLoadingStart { get; set; }
        public Action OnLoadingEnd { get; set; }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void AddScene(string scene)
        {
            StartCoroutine(LoadAdditiveSceneAsync(scene));
        }

        public void AddCurrentScene(string name)
        {
            _currentScene = name;
        }

        public void SwitchScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public void SwitchSceneAsync(string scene)
        {
            if (_currentScene == null)
            {
                StartCoroutine(SingleOperationAsync(scene));
            }
            else
            {
                StartCoroutine(SwitchOperationAsync(scene));
            }
        }

        IEnumerator SingleOperationAsync(string scene)
        {
            yield return null;
            OnLoadingStart?.Invoke();
            yield return LoadSceneAsync(scene);
            OnLoadingEnd?.Invoke();
            yield return null;
        }

        IEnumerator SwitchOperationAsync(string scene)
        {
            yield return null;
            OnLoadingStart?.Invoke();
            yield return UnloadSceneAsync(_currentScene);
            yield return LoadSceneAsync(scene);
            OnLoadingEnd?.Invoke();
            yield return null;
        }

        IEnumerator UnloadSceneAsync(string scene)
        {
            yield return null;
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(scene);
            while (!asyncOperation.isDone)
            {
                Debug.Log($"Unloading {scene} progress: {asyncOperation.progress * 100}%");
                yield return null;
            }
            Debug.Log($"Scene {scene} unloaded!");
        }

        IEnumerator LoadSceneAsync(string scene, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            yield return null;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene, mode);
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                Debug.Log($"Loading {scene} progress: {asyncOperation.progress * 100}%");
                if (asyncOperation.progress >= 0.9f)
                {
                    Debug.Log($"Scene {scene} loaded!");
                    asyncOperation.allowSceneActivation = true;
                    _currentScene = scene;
                }
                yield return null;
            }
        }

        IEnumerator LoadAdditiveSceneAsync(string scene, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            yield return null;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene, mode);
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                Debug.Log($"Loading {scene} progress: {asyncOperation.progress * 100}%");
                if (asyncOperation.progress >= 0.9f)
                {
                    Debug.Log($"Scene {scene} loaded!");
                    asyncOperation.allowSceneActivation = true;
                    _additiveScenes.Add(scene);
                }
                yield return null;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {

        }
    }
}