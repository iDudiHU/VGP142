using Schoolwork.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Schoolwork.Systems 
{
    public static class SceneLoadSystem
    {
		public static string SceneToLoad;
		public static string LoadingScreen = "LoadScene";
		public static AsyncOperation LoadSceneAsync(string levelName, bool loadFromSave)
		{
			GameManager.LoadedFromSave = loadFromSave;
			return SceneManager.LoadSceneAsync(levelName);
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
	}
}

