using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.SceneManagement;

namespace Schoolwork.Systems
{
	public static class SaveSystem
	{
		static GameData gameData;
		static string savePath;

		static SaveSystem()
		{
			savePath = Application.persistentDataPath + "/Saves/savegame.json";
		}

		private static void SaveGameFile()
		{
			string SAVE_FOLDER = Application.persistentDataPath + "/Saves/";
			if (!Directory.Exists(SAVE_FOLDER))
			{
				Directory.CreateDirectory(SAVE_FOLDER);
			}
			string json = JsonUtility.ToJson(gameData, true);
			File.WriteAllText(savePath, json);
			Debug.Log(json);
		}

		private static GameData LoadGameData()
		{
			if (File.Exists(savePath))
			{
				string json = File.ReadAllText(savePath);
				return JsonUtility.FromJson<GameData>(json);
			}
			else
			{
				Debug.LogWarning("Save file not found at " + savePath);
				return null;
			}
		}

		public static void SaveGame()
		{
			string currentSceneName = SceneManager.GetActiveScene().name;
			GameObject player = GameManager.Instance.player.gameObject;
			List<Enemy> enemies = GameManager.Instance.enemySystem.Enemies;
			List<Helpers.PickUp> pickups = GameManager.Instance.collectibleSystem.collectibles;
			gameData = new GameData();
			gameData.sceneToLoad = currentSceneName;
			player.GetComponent<ThirdPersonCharacter>().Save(ref gameData);
			List<GameData.EnemyData> enemyDataList = new List<GameData.EnemyData>();
			foreach (Enemy enemy in enemies)
			{
				enemy.Save(ref gameData);
			}
			List<GameData.CollectibleData> collectibleDataList = new List<GameData.CollectibleData>();
			foreach (Helpers.PickUp pickup in pickups)
			{
				pickup.Save(ref gameData);
			}
			SaveGameFile();
		}
		public static void LoadGame()
		{
			GameData gameData = LoadGameData();
			SceneLoadSystem.LoadScene(gameData.sceneToLoad, true);
		}

		public static void OnSceneLoaded()
		{
			string currentSceneName = SceneManager.GetActiveScene().name;
			GameData gameData = LoadGameData();
			if (gameData.sceneToLoad != currentSceneName) return;
			if (gameData == null)
			{
				Debug.Log("No saved game data found.");
				return;
			}
			if (GameManager.Instance.player != null)
			{
				GameManager.Instance.player.GetComponent<ThirdPersonCharacter>().Load(gameData);
			}
			List<GameData.EnemyData> enemyDataList = gameData.enemyDataList;
			if (GameManager.Instance.enemySystem != null && GameManager.Instance.enemySystem.EnemiesGuids.Count > 0)
			{
				foreach (GameData.EnemyData enemyData in enemyDataList)
				{
					int enemyIndex = GameManager.Instance.enemySystem.EnemiesGuids.IndexOf(enemyData.Id);
					if (enemyIndex != -1)
					{
						GameManager.Instance.enemySystem.Enemies[enemyIndex].Load(enemyData);
						//GameManager.Instance.enemySystem.SpawnEnemy(enemyData);
					}
					else
					{
						// Create a new enemy based on the saved data
						GameManager.Instance.enemySystem.SpawnEnemy(enemyData);
					}
				}
				GameManager.UpdateUIElements();
			}
			else
			{
				Debug.Log("No player/enemies to load");
			}
			List<GameData.CollectibleData> collectibleDataList = gameData.collectibleDataList;
			List<Helpers.PickUp> pickups = GameManager.Instance.collectibleSystem.collectibles;
			if (GameManager.Instance.collectibleSystem != null && GameManager.Instance.collectibleSystem.collectibleGuids.Count > 0)
			{
				foreach (Helpers.PickUp pickup in pickups)
				{
					//Call this on that Pickup Object that has the same Id as collectible.id
						pickup.Load(gameData);
				}
				GameManager.UpdateUIElements();
			}
			else
			{
				Debug.Log("No collectibles load");
			}
		}
	}
}
