using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class handles the health state of a game object.
/// 
/// Implementation Notes: 2D Rigidbodies must be set to never sleep for this to interact with trigger stay damage
/// </summary>\
namespace Schoolwork.Systems
{
	public class HealthSystem : MonoBehaviour
	{
		[Header("Team Settings")]
		[Tooltip("The team associated with this damage")]
		public int teamId = 0;

		[Header("Health Settings")]
		[Tooltip("The default health value")]
		public float defaultHealth = 100.0f;
		[Tooltip("The maximum health value")]
		public float maximumHealth = 100.0f;
		[Tooltip("The current in game health value")]
		public float currentHealth = 100.0f;
		[Tooltip("Invulnerability duration, in seconds, after taking damage")]
		public float invincibilityTime = 0.5f;
		[Tooltip("Whether or not this health is always invincible")]
		public bool isAlwaysInvincible = false;

		[Header("Lives settings")]
		[Tooltip("Whether or not to use lives")]
		public bool useLives = false;
		[Tooltip("Current number of lives this health has")]
		public int currentLives = 3;
		[Tooltip("The maximum number of lives this health has")]
		public int maximumLives = 5;
		[Tooltip("The amount of time to wait before respawning")]
		public float respawnWaitTime = 3f;

		/// <summary>
		/// Description:
		/// Standard Unity function called once before the first Update call
		/// Input:
		/// none
		/// Return:
		/// void (no return)
		/// </summary>
		void Start()
		{
			SetRespawnPoint(transform.position);
		}

		private void OnEnable()
		{
			if (gameObject.CompareTag("Player"))
			{
				SetupGameManagerReference();
			}
		}
		private void SetupGameManagerReference()
		{
			GameManager.Instance.healthSystem = this;
		}

		/// <summary>
		/// Description:
		/// Standard Unity function called once very frame
		/// Input:
		/// none
		/// Return:
		/// void (no return)
		/// </summary>
		void Update()
		{
			InvincibilityCheck();
			RespawnCheck();
		}

		private float respawnTime;

		/// <summary>
		/// Description:
		/// Checks to see if the health gameobject should be respawned yet and only respawns it if the alloted time has passed
		/// Input:
		/// none
		/// Return:
		/// void (no return)
		/// </summary>
		private void RespawnCheck()
		{
			if (respawnWaitTime != 0 && currentHealth <= 0 && currentLives > 0)
			{
				if (Time.time >= respawnTime)
				{
					Respawn();
				}
			}
		}

		// The specific game time when the health can be damged again
		private float timeToBecomeDamagableAgain = 0;
		// Whether or not the health is invincible
		private bool isInvincableFromDamage = false;

		/// <summary>
		/// Description:
		/// Checks against the current time and the time when the health can be damaged again.
		/// Removes invicibility if the time frame has passed
		/// Input:
		/// none
		/// Return:
		/// void (no return)
		/// </summary>
		private void InvincibilityCheck()
		{
			if (timeToBecomeDamagableAgain <= Time.time)
			{
				isInvincableFromDamage = false;
			}
		}

		// The position that the health's gameobject will respawn at
		private Vector3 respawnPosition;
		/// <summary>
		/// Description:
		/// Changes the respawn position to a new position
		/// Input:
		/// Vector3 newRespawnPosition
		/// Returns:
		/// void (no return)
		/// </summary>
		/// <param name="newRespawnPosition">The new position to respawn at</param>
		public void SetRespawnPoint(Vector3 newRespawnPosition)
		{
			//respawnPosition = newRespawnPosition;
		}

		/// <summary>
		/// Description:
		/// Repositions the health's game object to the respawn position and resets the current health to the default value
		/// Input:
		/// none
		/// Return:
		/// void (no return)
		/// </summary>
		void Respawn()
		{

		}

		/// <summary>
		/// Description:
		/// Applies damage to the health unless the health is invincible.
		/// Input:
		/// int damageAmount
		/// Return:
		/// void (no return)
		/// </summary>
		/// <param name="damageAmount">The amount of damage to take</param>
		public void TakeDamage(float damageAmount)
		{
			if (isInvincableFromDamage || currentHealth <= 0 || isAlwaysInvincible)
			{
				return;
			}
			else
			{
				if (hitEffect != null)
				{
					Instantiate(hitEffect, transform.position, transform.rotation, null);
				}
				eventsOnHit?.Invoke();
				timeToBecomeDamagableAgain = Time.time + invincibilityTime;
				isInvincableFromDamage = true;
				currentHealth = Mathf.Clamp(currentHealth - damageAmount, 0, maximumHealth);
				GameManager.UpdateUIElements();
				CheckDeath();
			}
		}

		/// <summary>
		/// Description:
		/// Applies healing to the health, capped out at the maximum health.
		/// Input:
		/// int healingAmount
		/// Return:
		/// void (no return)
		/// </summary>
		/// <param name="healingAmount">How much healing to apply</param>
		public void ReceiveHealing(float healingAmount)
		{
			currentHealth += healingAmount;
			if (currentHealth > maximumHealth)
			{
				currentHealth = maximumHealth;
			}
			GameManager.UpdateUIElements();
			CheckDeath();
		}

		/// <summary>
		/// Description:
		/// Gives the health script more lives if the health is using lives
		/// Input:
		/// int bonusLives
		/// Return:
		/// void (no return)
		/// </summary>
		/// <param name="bonusLives">The number of lives to add</param>
		public void AddLives(int bonusLives)
		{
			if (useLives)
			{
				currentLives += bonusLives;
				if (currentLives > maximumLives)
				{
					currentLives = maximumLives;
				}
			}
		}

		public void AddMaxLives()
		{
			if (maximumLives < 20)
			{
				maximumLives++;
				currentLives++;
			}
			GameManager.UpdateUIElements();
		}

		public void AddMaxHealth(int level)
		{
			maximumHealth += 20 * Mathf.Pow(1.3f, level);
			currentHealth = maximumHealth;
		}


		[Header("Effects & Polish")]
		[Tooltip("The effect to create when this health dies")]
		public GameObject deathEffect;
		[Tooltip("The effect to create when this health is damaged (but does not die)")]
		public GameObject hitEffect;
		[Tooltip("A list of events that occur when the health becomes 0 or lower")]
		public UnityEvent eventsOnDeath;
		[Tooltip("A list of events that occur when the health becomes 0 or lower")]
		public UnityEvent eventsOnHit;
		[Tooltip("A list of events that occur on respawn")]
		public UnityEvent eventsOnRespawn;

		/// <summary>
		/// Description:
		/// Checks if the health is dead or not. If it is, true is returned, false otherwise.
		/// Calls Die() if the health is dead.
		/// Input:
		/// none
		/// Return:
		/// bool
		/// </summary>
		/// <returns>bool: A boolean value representing if the health has died or not (true for dead)</returns>
		bool CheckDeath()
		{
			if (currentHealth <= 0)
			{
				Die();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Description:
		/// Handles the death of the health. If a death effect is set, it is created. If lives are being used, the health is respawned.
		/// If lives are not being used or the lives are 0 then the health's game object is destroyed.
		/// Input:
		/// none
		/// Return:
		/// void (no return)
		/// </summary>
		void Die()
		{
			if (deathEffect != null)
			{
				if (deathEffect != null)
				{
					Instantiate(deathEffect, transform.position, transform.rotation, null);
				}
			}

			// Do on death events
			if (eventsOnDeath != null)
			{
				eventsOnDeath.Invoke();
			}

			if (useLives)
			{
				currentLives -= 1;
				if (currentLives > 0)
				{
					if (respawnWaitTime == 0)
					{
						Respawn();
					}
					else
					{
						respawnTime = Time.time + respawnWaitTime;
					}
				}
				else
				{
					if (respawnWaitTime != 0)
					{
						respawnTime = Time.time + respawnWaitTime;
					}
					GameOver();
				}

			}
			else
			{
				GameOver();
			}
		}

		/// <summary>
		/// Description:
		/// Tries to notify the game manager that the game is over
		/// Input: 
		/// none
		/// Return: 
		/// void (no return)
		/// </summary>
		public void GameOver()
		{
			if (GameManager.Instance != null && gameObject.tag == "Player")
			{
				GameManager.GameOver();
			}
		}
		public void Load(GameData.HealthData healthData)
		{
			currentHealth = healthData._currentHealth;
			maximumHealth = healthData._maxHealth;
		}

		public void Save(ref GameData data)
		{
			data.player.healthData._currentHealth = currentHealth;
			data.player.healthData._maxHealth =	maximumHealth;
		}
	}
}

