using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject[] enemyPrefabs;
	public float spawnInterval = 5f;

	private float lastSpawnTime;

	void Update()
	{
		if (Time.time - lastSpawnTime > spawnInterval) {
			lastSpawnTime = Time.time;
			SpawnEnemy();
		}
	}

	void SpawnEnemy()
	{
		Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-.5f, .5f), 0, Random.Range(-.5f, .5f));
		Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPosition, Quaternion.identity);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position + new Vector3(0f, 1.5f, 0f), new Vector3(1f, 3f, 1f));
	}
}
