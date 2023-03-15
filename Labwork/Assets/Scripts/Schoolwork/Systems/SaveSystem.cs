using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Schoolwork.Systems
{
	public class SaveSystem: MonoBehaviour
	{
		public GameData gameData;
		string savePath;

		public void Awake()
		{
			SetupGameManagerSaveSystem();
			savePath = Application.persistentDataPath + "/Saves/savegame.json";
		}
		private void SetupGameManagerSaveSystem()
		{
			if (GameManager.Instance != null && GameManager.Instance.saveSystem == null)
			{
				GameManager.Instance.saveSystem = this;
			}
		}
		public void SaveGame()
		{
			string SAVE_FOLDER = Application.persistentDataPath + "/Saves/";
			if (!Directory.Exists(SAVE_FOLDER))
			{
				Directory.CreateDirectory(SAVE_FOLDER);
			}
			string json = JsonUtility.ToJson(gameData, true);
			File.WriteAllText(savePath, json);
			Debug.Log(savePath);
			Debug.Log(json);
		}

		public GameData LoadGameData()
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
		public void SaveGameData(GameObject player, List<Enemy> enemies)
		{
			gameData = new GameData();
			player.GetComponent<ThirdPersonCharacter>().Save(ref gameData);
			List<GameData.EnemyData> enemyDataList = new List<GameData.EnemyData>();
			foreach (Enemy enemy in enemies)
			{
				enemy.Save(ref gameData);
			}
			SaveGame();
		}

		public void LoadGame()
		{
			GameData gameData = LoadGameData();
			if (gameData == null)
			{
				Debug.Log("No saved game data found.");
				return;
			}
			GameManager.Instance.player.GetComponent<ThirdPersonCharacter>().Load(gameData);
			List<GameData.EnemyData> enemyDataList = gameData.enemyDataList;
			foreach (GameData.EnemyData enemyData in enemyDataList)
			{
				int enemyIndex = GameManager.Instance.enemySystem.EnemiesGuids.IndexOf(enemyData.Id);
				if (enemyIndex != -1)
				{
					// Load the enemy's data
					GameManager.Instance.enemySystem.Enemies[enemyIndex].Load(enemyData);
				}
				else
				{
					// Create a new enemy based on the saved data
					GameManager.Instance.enemySystem.SpawnEnemy(enemyData);
				}
			}
			GameManager.UpdateUIElements();
		}
	}
}