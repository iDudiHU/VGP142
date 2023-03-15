using UnityEngine;
using System.Collections.Generic;
using System.IO;
using static Schoolwork.Enemy;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Schoolwork.Systems
{
	public static class SaveSystem
	{
		static readonly string SAVE_FOLDER = Application.persistentDataPath + "/Saves/";
		static readonly string savePath = SAVE_FOLDER + "/savegame.json";
		public static void SaveGame(PlayerData playerData, List<EnemyData> enemyDataList)
		{
			if (!Directory.Exists(SAVE_FOLDER))
			{
				Directory.CreateDirectory(SAVE_FOLDER);
			}
			GameData gameData = new GameData(playerData, enemyDataList);
			string json = JsonUtility.ToJson(gameData, true);
			File.WriteAllText(savePath, json);
			Debug.Log(savePath);
			Debug.Log(json);
		}

		public static GameData LoadGameData()
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
		public static void SaveGameData(GameObject player, List<Enemy> enemies)
		{
			PlayerData playerData = player.GetComponent<ThirdPersonCharacter>().Save();

			List<EnemyData> enemyDataList = new List<EnemyData>();
			foreach (Enemy enemy in enemies)
			{
				enemyDataList.Add(enemy.Save());
			}
			SaveGame(playerData, enemyDataList);
		}

		public static void LoadGame()
		{
			GameData gameData = LoadGameData();
			if (gameData == null)
			{
				Debug.Log("No saved game data found.");
				return;
			}
			GameManager.Instance.player.GetComponent<ThirdPersonCharacter>().Load(gameData.playerData);
			List<EnemyData> enemyDataList = gameData.enemyDataList;
			foreach (EnemyData enemyData in enemyDataList)
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
		}
	}
}



