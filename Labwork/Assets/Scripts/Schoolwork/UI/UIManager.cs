using System;
using System.Collections.Generic;
using System.Linq;
using Schoolwork.Helpers;
using Schoolwork.Systems;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Schoolwork.UI
{
	/// <summary>
	/// A class which manages pages of UI elements
	/// and the game's UI
	/// </summary>
	public class UIManager : MonoBehaviour
	{

		[Header("Page Management")]
		[Tooltip("The pages (Panels) managed by the UI Manager")]
		public List<UIPage> pages;
		[Tooltip("The index of the active page in the UI")]
		public int currentPage = 0;
		[Tooltip("The page (by index) switched to when the UI Manager starts up")]
		public int defaultPage = 0;

		[Header("Pause Settings")]
		[Tooltip("The index of the pause page in the pages list")]
		public int pausePageIndex = 1;
		[Tooltip("Whether or not to allow pausing")]
		public bool allowPause = false;
		[Header("Polish Effects")]
		[Tooltip("The effect to create when navigating between UI")]
		public GameObject navigationEffect;
		[Tooltip("The effect to create when clicking on or pressing a UI element")]
		public GameObject clickEffect;
		[Tooltip("The effect to create when the player is backing out of a Menu page")]
		public GameObject backEffect;
		public GameObject worldSpaceUICanvas;

		// Whether the application is paused
		private bool isPaused = false;

		// A list of all UI element classes
		private List<UIelement> UIelements;

		// The event system handling UI navigation
		[HideInInspector]
		public EventSystem eventSystem;
		// The Input Manager to listen for pausing

		private bool isLevelingUp;
		private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
		{
			ToggleLevelUp();
		}

		void Awake()
		{
			SetupGameManagerReference();
		}

		/// <summary>
		/// Description:
		/// Creates a back effect if one is set
		/// Input:
		/// none
		/// Return:
		/// void (no return)
		/// </summary>
		public void CreateBackEffect()
		{
			if (backEffect)
			{
				Instantiate(backEffect, transform.position, Quaternion.identity, null);
			}
		}

		/// <summary>
		/// Description:
		/// Creates a click effect if one is set
		/// Input:
		/// none
		/// Return:
		/// void (no return)
		/// </summary>
		public void CreateClickEffect()
		{
			if (clickEffect)
			{
				Instantiate(clickEffect, transform.position, Quaternion.identity, null);
			}
		}

		/// <summary>
		/// Description:
		/// Creates a navigation effect if one is set
		/// Input:
		/// none
		/// Return:
		/// void (no return)
		/// </summary>
		public void CreateNavigationEffect()
		{
			if (navigationEffect)
			{
				Instantiate(navigationEffect, transform.position, Quaternion.identity, null);
			}
		}


		/// <summary>
		/// Description:
		/// Sets this component as the UI manager for the GameManager
		/// Input: 
		/// none
		/// Return: 
		/// void (no return)
		/// </summary>
		private void SetupGameManagerReference()
		{
			GameManager.Instance.uiManager = this;
			Debug.Log($"I triggered in scene {SceneManager.GetActiveScene().name}");
		}

		/// <summary>
		/// Description:
		/// Finds and stores all UIElements in the UIElements list
		/// Input:
		/// none
		/// Return:
		/// void (no return)
		/// </summary>
		private void SetUpUIElements()
		{
			UIelements = FindObjectsOfType<UIelement>().ToList();
		}

		/// <summary>
		/// Description:
		/// Gets the event system from the scene if one exists
		/// If one does not exist a warning will be displayed
		/// Input:
		/// none
		/// Return:
		/// void (no return)
		/// </summary>
		private void SetUpEventSystem()
		{
			eventSystem = FindObjectOfType<EventSystem>();
			if (eventSystem == null)
			{
				Debug.LogWarning("There is no event system in the scene but you are trying to use the UIManager. /n" +
								 "All UI in Unity requires an Event System to run. /n" +
								 "You can add one by right clicking in hierarchy then selecting UI->EventSystem.");
			}
		}
		/// <summary>
		/// Description:
		/// If the game is paused, unpauses the game.
		/// If the game is not paused, pauses the game.
		/// Input:
		/// none
		/// Retun:
		/// void (no return)
		/// </summary>
		public void TogglePause()
		{
			UIPage page = pages.Find(item => item.name == "MainMenu");
			if ((SceneManager.GetActiveScene().buildIndex != 0 && (currentPage != pages.IndexOf(page)) || allowPause))
			{
				if (isPaused)
				{
					CursorManager.instance.ChangeCursorMode(CursorManager.CursorState.FPS);
					GoToPage(defaultPage);
					Time.timeScale = 1;
					isPaused = false;
				}
				else
				{
					CursorManager.instance.ChangeCursorMode(CursorManager.CursorState.Menu);
					GoToPageByName("PausePage", false);
					Time.timeScale = 0;
					isPaused = true;
				}
			}
		}

		/// <summary>
		/// Description:
		/// Goes through all UI elements and calls their UpdateUI function
		/// Input:
		/// none
		/// Return:
		/// void (no return)
		/// </summary>
		public void UpdateUI()
		{
			foreach (UIelement uiElement in UIelements)
			{
				uiElement.UpdateUI();
			}
		}

		/// <summary>
		/// Description:
		/// Default function from Unity that runs when the script is first started
		/// Input: 
		/// none
		/// Return:
		/// void (no return)
		/// </summary>
		private void Start()
		{
			SetUpEventSystem();
			SetUpUIElements();
			InitilizeFirstPage();
			UpdateUI();
		}

		/// <summary>
		/// Description:
		/// Sets up the first page
		/// Input:
		/// none
		/// Return:
		/// void (no return)
		/// </summary>
		private void InitilizeFirstPage()
		{
			GoToPage(defaultPage);
		}

		/// <summary>
		/// Description:
		/// Default function from Unity that runs every frame
		/// Input: 
		/// none
		/// Return: 
		/// void (no return)
		/// </summary>
		private void Update()
		{
			CheckPauseInput();
		}

		/// <summary>
		/// Description:
		/// If the input manager is set up, reads the pause input
		/// Input:
		/// none
		/// Return:
		/// void (no return)
		/// </summary>
		private void CheckPauseInput()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				TogglePause();
			}
		}
		/// <summary>
		/// Description:
		/// Goes to a page by that page's index
		/// Input: 
		/// int pageIndex
		/// Return: 
		/// void (no return)
		/// </summary>
		/// <param name="pageIndex">The index in the page list to go to</param>
		public void GoToPage(int pageIndex)
		{
			if (pageIndex < pages.Count && pages[pageIndex] != null)
			{
				SetActiveAllPages(false);
				pages[pageIndex].gameObject.SetActive(true);
				pages[pageIndex].SetSelectedUIToDefault();
			}
		}

		public void GoToDefaultPage()
		{
			for (int i = 0; i < pages.Count; i++)
			{
				UIPage pg = pages[i];
				if (pg != null && i != 0)
				{
					pg.gameObject.SetActive(false);
				}
			}
			pages[0].SetSelectedUIToDefault();
		}

		/// <summary>
		/// Description:
		/// Goes to a page by that page's name
		/// Input: 
		/// int pageName
		/// Return: 
		/// void (no return)
		/// </summary>
		/// <param name="pageName">The name of the page in the game you want to go to, if their are duplicates this picks the first found</param>
		public void GoToPageByName(string pageName)
		{
			GoToPageByName(pageName, true);
		}

		public void GoToPageByName(string pageName, bool deactivateHud)
		{
			UIPage page = pages.Find(item => item.name == pageName);
			int pageIndex = pages.IndexOf(page);
			if (pageIndex < pages.Count && pages[pageIndex] != null)
			{
				if (pages != null)
				{
					for (int i = 0; i < pages.Count; i++)
					{
						UIPage pg = pages[i];
						if (pg != null && (deactivateHud && i != 0))
						{
							pg.gameObject.SetActive(false);
						}
					}
				}
				pages[pageIndex].gameObject.SetActive(true);
				pages[pageIndex].SetSelectedUIToDefault();
			}
		}

		public void ToggleLevelUp()
		{
			if (isLevelingUp)
			{
				CursorManager.instance.ChangeCursorMode(CursorManager.CursorState.FPS);
				GoToDefaultPage();
				GameManager.UpdateUIElements();
				Time.timeScale = 1;
				allowPause = true;
				isLevelingUp = false;
			}
			else
			{
				CursorManager.instance.ChangeCursorMode(CursorManager.CursorState.Menu);
				GoToPageByName("LevelUpPage", false);
				Time.timeScale = .0f;
				allowPause = false;
				isLevelingUp = true;
			}
		}
		public void ToggleDeathScreen()
		{
			CursorManager.instance.ChangeCursorMode(CursorManager.CursorState.Menu);
			allowPause = false;
			GoToPageByName("LosePage");
			Time.timeScale = 0.1f;
		}

		public void ToggleWinScreen()
		{
			CursorManager.instance.ChangeCursorMode(CursorManager.CursorState.Menu);
			allowPause = false;
			GoToPageByName("WinPage");
			Time.timeScale = 0.0f;
		}

		/// <summary>
		/// Description:
		/// Turns all stored pages on or off depending on parameters
		/// Input: 
		/// bool activated
		/// Return: 
		/// void (no return)
		/// </summary>
		/// <param name="activated">The true or false value to set all page game objects activeness to</param>
		public void SetActiveAllPages(bool activated)
		{
			if (pages != null)
			{
				foreach (UIPage page in pages)
				{
					if (page != null)
						page.gameObject.SetActive(activated);
				}
			}
		}

		public void Save()
		{
			SaveSystem.SaveGame();
		}

		public void Load()
		{
			SaveSystem.LoadGame();
		}
	}
}
