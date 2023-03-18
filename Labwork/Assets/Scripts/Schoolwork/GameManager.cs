using System;
using System.Collections;
using System.Collections.Generic;
using Schoolwork.Helpers;
using Schoolwork.Music;
using Schoolwork.Systems;
using Schoolwork.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Schoolwork
{
	[DefaultExecutionOrder(-1)]
	public class GameManager : Singelton<GameManager>
	{
		[Header("GameState:")]
		public static GameState gameState;
		[Header("References:")]
		[Tooltip("The UIManager component which manages the current scene's UI")]
		public UIManager uiManager = null;
		[Tooltip("The HealthSystem component which manages the current players health")]
		public HealthSystem healthSystem = null;
		[Tooltip("The HealthSystem component which manages the current players health")]
		public WeaponSystem weaponSystem = null;
		[Tooltip("The HealthSystem component which manages the current players health")]
		public LevelSystem levelSystem = null;
		[Tooltip("The HealthSystem component which manages the current players health")]
		public EnemySystem enemySystem = null;
		public CollectibleSystem collectibleSystem = null;
		public CheckpointleSystem checkpointSystem = null;

		private float lastCombatTime = 0f;
		private float combatTimeout = 10f;

		public static bool LoadedFromSave = false;

		public Camera mainCamera = null;
		
		[Tooltip("The player gameobject")]
		public GameObject player = null;
    
		// Start is called before the first frame update
		protected override void Awake()
		{
			base.Awake();
			MusicManager.Instance?.Play();
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			KeyRing.ClearKeyRing();
			SceneLoadSystem.SceneLoaded += OnSceneLoaded;
			EnemyHealthSystem.OnHealthLost += OnHealthLost;
			HealthSystem.OnHealthLost += OnHealthLost;
			mainCamera = Camera.main;
			gameState = GameState.Menu;
		}

		private void OnSceneLoaded()
		{
			mainCamera = Camera.main;
			if (LoadedFromSave)
			{
				SaveSystem.OnSceneLoaded();
			}
			MusicManager.Instance.SwitchToIdle();
			UpdateUIElements();
		}
		private void OnHealthLost()
		{
			if(gameState != GameState.Combat)
				SwitchState(GameState.Combat);
		}



		// Update is called once per frame
		void Update()
		{
			if (gameState == GameState.Combat && Time.time - lastCombatTime > combatTimeout)
			{
				SwitchState(GameState.Idle);
			}
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
				Instance.player.GetComponent<ThirdPersonCharacter>().Die();
			}
		}
		public void SwitchState(GameState newState)
		{
			switch (newState)
			{
				case GameState.Menu:
					// Do any necessary setup for the menu state
					break;
				case GameState.Idle:
					// Do any necessary setup for the idle state
					break;
				case GameState.Combat:
					// Do any necessary setup for the combat state
					break;
				default:
					Debug.LogError("Invalid game state");
					return;
			}

			// Perform any necessary cleanup for the current state
			switch (gameState)
			{
				case GameState.Menu:
					// Do any necessary cleanup for the menu state
					break;
				case GameState.Idle:
					// Do any necessary cleanup for the idle state
					break;
				case GameState.Combat:
					// Do any necessary cleanup for the combat state
					break;
				default:
					Debug.LogError("Invalid game state");
					return;
			}

			// Set the new game state
			gameState = newState;

			// Perform any necessary actions for the new state
			switch (gameState)
			{
				case GameState.Menu:
					// Do any necessary actions for the menu state
					break;
				case GameState.Idle:
					MusicManager.Instance.SwitchToIdle();
					lastCombatTime = Time.time;
					break;
				case GameState.Combat:
					MusicManager.Instance.SwitchToCombat();
					lastCombatTime = Time.time;
					break;
				default:
					Debug.LogError("Invalid game state");
					return;
			}
		}

		private void OnApplicationQuit()
		{
			SceneLoadSystem.SceneLoaded -= OnSceneLoaded;
		}
	}
	public enum GameState
	{
		Menu,
		Idle,
		Combat
	}
}
