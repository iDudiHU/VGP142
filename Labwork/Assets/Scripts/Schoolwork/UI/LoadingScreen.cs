using Schoolwork.Systems;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Schoolwork.UI
{
	public class LoadingScreen : MonoBehaviour
	{
		public delegate void SceneLoadedEventHandler();
		public static event SceneLoadedEventHandler SceneLoaded;

		[SerializeField] private Slider progressBarSlider;
		[SerializeField] private TextMeshProUGUI loadText;
		[SerializeField] private float waitTime = 2f;
		
		private float progress;

		private void Start()
		{
			Time.timeScale = 1.0f;
			StartCoroutine(LoadSceneAsync());
		}

		private IEnumerator LoadSceneAsync()
		{
			AsyncOperation asyncLoad = SceneLoadSystem.LoadSceneAsync(SceneLoadSystem.SceneToLoad, GameManager.LoadedFromSave);
			//This is to make the load take longer 
			asyncLoad.allowSceneActivation = false;

			// Initialize timer variables
			float timer = 0f;
			bool timerStarted = false;

			while (!asyncLoad.isDone)
			{
				// Continue the progress from where it stopped during the wait
				progress = Mathf.Lerp(progress, asyncLoad.progress / 0.9f, Time.deltaTime * 1f);
				progressBarSlider.value = progress;
				loadText.text = $"Loading {Mathf.RoundToInt(progress * 100)} %";

				// Start the timer when progress is 90% or greater
				if (progress >= 0.9f && !timerStarted)
				{
					timerStarted = true;
					timer = 0f;
				}

				// Increment the timer
				if (timerStarted)
				{
					timer += Time.deltaTime;
				}

				//Allow scene activation after waiting for the specified time
				if (timer >= waitTime)
					{
						asyncLoad.allowSceneActivation = true;
						Time.timeScale = 1.0f;
				}
				yield return null;
			}
		}
	}
}
