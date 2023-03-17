using Schoolwork;
using Schoolwork.Helpers;
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
		SetupGameManagerReference();
	}
	private void SetupGameManagerReference()
	{
		GameManager.Instance.enemySystem = this;
	}

	public void SpawnEnemy(GameData.EnemyData enemyData)
	{
		GameObject enemyObject = Instantiate(GetEnemyPrefab(enemyData.enemyType), enemyData.transformData.position, Quaternion.Euler(enemyData.transformData.rotation.x, enemyData.transformData.rotation.y, enemyData.transformData.rotation.z));
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
