using System.Collections;
using System.Collections.Generic;
using Schoolwork.Helpers;
using Schoolwork.Systems;
using Schoolwork.UI;
using UnityEngine;

namespace Schoolwork
{
	public class GameManager : Singleton<GameManager>
	{
		[Header("References:")]
		[Tooltip("The UIManager component which manages the current scene's UI")]
		public UIManager uiManager = null;
		[Tooltip("The HealthSystem component which manages the current players health")]
		public HealthSystem healthSystem = null;
		[Tooltip("The HealthSystem component which manages the current players health")]
		public WeaponSystem weaponSystem = null;
		[Tooltip("The HealthSystem component which manages the current players health")]
		public LevelSystem levelSystem = null;
		
		[Tooltip("The player gameobject")]
		public GameObject player = null;
    
		// Start is called before the first frame update
		void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			KeyRing.ClearKeyRing();
		}
		

		// Update is called once per frame
		void Update()
		{
        
		}
		/// <summary>
		/// Description:
		/// Sends out a message to UI elements to update
		/// Input:
		/// none
		/// Return: 
		/// void (no return)
		/// </summary>
		public static void UpdateUIElements()
		{
			if (Instance != null && Instance.uiManager != null)
			{
				Instance.uiManager.UpdateUI();
			}
		}
		
		[Header("Game Over Settings:")]
		[Tooltip("The index in the UI manager of the game over page")]
		public int gameOverPageIndex = 0;
		[Tooltip("The game over effect to create when the game is lost")]
		public GameObject gameOverEffect;

		// Whether or not the game is over
		[HideInInspector]
		public bool gameIsOver = false;
		/// <summary>
		/// Description:
		/// Displays game over screen
		/// Input:
		/// none
		/// Return:
		/// void (no return)
		/// </summary>
		public static void GameOver()
		{
			Instance.gameIsOver = true;
			if (Instance.gameOverEffect != null)
			{
				Instantiate(Instance.gameOverEffect, Instance.transform.position, Instance.transform.rotation, null);
			}
			if (Instance.uiManager != null)
			{
				// pause the game without brining up the pause screen
				Time.timeScale = 0.1f;
				CursorManager.instance.ChangeCursorMode(CursorManager.CursorState.Menu);
				Instance.uiManager.allowPause = false;
				Instance.uiManager.GoToPageByName("LosePage");
				//Add player death .................
			}
		}
	}	
}
