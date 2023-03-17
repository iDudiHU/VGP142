using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Schoolwork
{
	public class EnemySpawner : MonoBehaviour
	{
		public GameObject[] enemyPrefabs;

		public void SpawnEnemy()
		{
			Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-.5f, .5f), 0, Random.Range(-.5f, .5f));
			GameObject go = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPosition, Quaternion.identity);
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(transform.position + new Vector3(0f, 1.5f, 0f), new Vector3(1f, 3f, 1f));
		}
	}
}
