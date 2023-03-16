using System.Collections.Generic;
using Schoolwork.Helpers;
using Schoolwork.Systems;
using Schoolwork.UI.Element;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Schoolwork.UI
{
	public class HealthBarManager : MonoBehaviour
	{
		[SerializeField] private EnemyHealthBar healthBarPrefab;

		public Dictionary<EnemyHealthSystem, EnemyHealthBar> healthBars = new Dictionary<EnemyHealthSystem, EnemyHealthBar>();

		void Awake()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
			EnemyHealthSystem.OnEnemyHealthSystemAdded += AddHealthBar;
			EnemyHealthSystem.OnEnemyHealthSystemRemoved += RemoveHealthBar;
			healthBars = new Dictionary<EnemyHealthSystem, EnemyHealthBar>();
		}
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			healthBars = new Dictionary<EnemyHealthSystem, EnemyHealthBar>();
		}
		private void AddHealthBar(EnemyHealthSystem health)
		{
			if (healthBars.ContainsKey(health))
			{
				RemoveHealthBar(health);
			}
			if (healthBars.ContainsKey(health) == false)
			{
				var healthBar = Instantiate(healthBarPrefab, transform);
				healthBars.Add(health, healthBar);
				healthBar.SetHealth(health);
			}
		}

		private void RemoveHealthBar(EnemyHealthSystem health)
		{
			if (healthBars.ContainsKey(health))
			{
				Destroy(healthBars[health].gameObject, 0.3f);
				healthBars.Remove(health);
			}
		}

		private void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
			EnemyHealthSystem.OnEnemyHealthSystemAdded -= AddHealthBar;
			EnemyHealthSystem.OnEnemyHealthSystemRemoved -= RemoveHealthBar;
		}

		private void OnApplicationQuit()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
			EnemyHealthSystem.OnEnemyHealthSystemAdded -= AddHealthBar;
			EnemyHealthSystem.OnEnemyHealthSystemRemoved -= RemoveHealthBar;
		}
	}
}
