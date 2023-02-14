using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Schoolwork
{
	public class EnemySpawner : MonoBehaviour
	{
		public GameObject[] enemyPrefabs;
		public float spawnInterval = 5f;
		private int m_EnemySpawned = 0;
		public int MaxEnemyToSpawn = 5;

		private float lastSpawnTime;

		void Update()
		{
			if (Time.time - lastSpawnTime > spawnInterval) {
				lastSpawnTime = Time.time;
				SpawnEnemy();
				if (m_EnemySpawned > MaxEnemyToSpawn)
					Destroy(gameObject);
			}
		}

		void SpawnEnemy()
		{
			Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-.5f, .5f), 0, Random.Range(-.5f, .5f));
			Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPosition, Quaternion.identity);
			m_EnemySpawned++;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(transform.position + new Vector3(0f, 1.5f, 0f), new Vector3(1f, 3f, 1f));
		}
	}
}
