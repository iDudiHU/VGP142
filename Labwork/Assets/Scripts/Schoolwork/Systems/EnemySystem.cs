using Schoolwork;
using Schoolwork.Systems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Schoolwork.Enemy;

public class EnemySystem : MonoBehaviour
{
	public List<GameObject> EnemyPrefabs = new List<GameObject>();
	public List<Enemy> Enemies = new List<Enemy>();
	public List<string> EnemiesGuids = new List<string>();
	private void Awake()
    {
        Enemy.EnemyCreated += EnemyCreated;
		Enemy.EnemyDestroyed += EnemyDeath;
    }
	private void EnemyCreated(Enemy enemy)
	{
		Enemies.Add(enemy);
		EnemiesGuids.Add(enemy.Id);
	}
	private void EnemyDeath(Enemy enemy)
	{
		Enemies.Remove(enemy);
		EnemiesGuids.Remove(enemy.Id);
	}
	public void OnEnable()
	{
		SetupGameManagerEnemySystem();
	}
	private void SetupGameManagerEnemySystem()
	{
		if (GameManager.Instance != null && GameManager.Instance.enemySystem == null)
		{
			GameManager.Instance.enemySystem = this;
		}
	}

	public void SpawnEnemy(GameData.EnemyData enemyData)
	{
		GameObject enemyObject = Instantiate(GetEnemyPrefab(enemyData.enemyType), new Vector3(enemyData.transformData.posX, enemyData.transformData.posY, enemyData.transformData.posZ), Quaternion.Euler(enemyData.transformData.rotX, enemyData.transformData.rotY, enemyData.transformData.posZ));
		enemyObject.GetComponentInChildren<Enemy>().Load(enemyData);
	}
	public GameObject GetEnemyPrefab(EnemyTypes enemyType)
	{
		switch (enemyType)
		{
			case EnemyTypes.Anubis:
				return EnemyPrefabs[0];
			case EnemyTypes.Demon:
				return EnemyPrefabs[1];
			case EnemyTypes.Fishman:
				return EnemyPrefabs[2];
			default:
				Debug.LogError("Invalid enemy type: " + enemyType);
				return null;
		}
	}

	private void OnDisable()
	{
		Enemy.EnemyCreated -= EnemyCreated;
		Enemy.EnemyDestroyed -= EnemyDeath;
	}
}
