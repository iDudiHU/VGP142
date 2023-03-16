using Schoolwork.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Schoolwork.Systems
{
    public static class SceneLoadSystem
    {
        public static string SceneToLoad;
        public static string LoadingScreen = "LoadScene";
        public static event Action SceneLoaded;

        public static AsyncOperation LoadSceneAsync(string levelName, bool loadFromSave)
        {
            GameManager.LoadedFromSave = loadFromSave;
            var operation = SceneManager.LoadSceneAsync(levelName);
            operation.completed += OnSceneLoaded;
            return operation;
        }

        public static AsyncOperation LoadSceneAsync(string levelName)
        {
            return LoadSceneAsync(levelName, false);
        }

        public static void LoadScene(string levelName, bool loadFromSave)
        {
            LoadLoadingScreen(levelName, loadFromSave);
        }

        public static void LoadScene(string levelName)
        {
            LoadLoadingScreen(levelName, false);
        }

        private static void LoadLoadingScreen(string levelName, bool loadFromSave)
        {
            GameManager.LoadedFromSave = loadFromSave;
            SceneToLoad = levelName;
            SceneManager.LoadScene(LoadingScreen);
        }

        public static void LoadSceneButton(string levelName)
        {
            SceneToLoad = levelName;
            LoadScene(LoadingScreen, false);
        }

        private static IEnumerator WaitForSceneLoadComplete()
        {
            yield return new WaitUntil(() => SceneManager.GetActiveScene().name == SceneToLoad);
            SceneLoaded?.Invoke();
        }

        private static void OnSceneLoaded(AsyncOperation operation)
        {
            GameManager.Instance.StartCoroutine(WaitForSceneLoadComplete());
        }
    }
}
