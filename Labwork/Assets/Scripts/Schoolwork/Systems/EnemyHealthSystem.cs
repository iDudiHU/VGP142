using Schoolwork.UI.Element;
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
    public class EnemyHealthSystem : MonoBehaviour
    {
        public static event Action<EnemyHealthSystem> OnEnemyHealthSystemAdded = delegate { }; 
        public static event Action<EnemyHealthSystem> OnEnemyHealthSystemRemoved = delegate { };
        [Header("Team Settings")] [Tooltip("The team associated with this damage")]
        public int teamId = 1;

        [Header("Health Settings")] [Tooltip("The default health value")]
        public float defaultHealth = 100.0f;

        [Tooltip("The maximum health value")] public float maximumHealth = 100.0f;

        [Tooltip("The current in game health value")]
        [SerializeField]
        private float m_CurrentHealth = 100.0f;

        public float CurrentHealth
		{
			get
			{
                return m_CurrentHealth;
			}
			set
			{
                m_CurrentHealth = value;
			}
		}
        public event Action<float> OnEnemyHealthPctChanged = delegate { };

            [Tooltip("Invulnerability duration, in seconds, after taking damage")]
        public float invincibilityTime = 0.5f;

        [Tooltip("Whether or not this health is always invincible")]
        public bool isAlwaysInvincible = false;
        

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
            OnEnemyHealthSystemAdded(this);
        }

        private void OnEnable()
        {
            CurrentHealth = maximumHealth;
            OnEnemyHealthSystemAdded(this);
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
            if (timeToBecomeDamagableAgain <= Time.time) {
                isInvincableFromDamage = false;
            }
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
            if (isInvincableFromDamage || m_CurrentHealth <= 0 || isAlwaysInvincible) {
                return;
            }
            else {
                if (hitEffect != null) {
                    var go = Instantiate(hitEffect, transform.position + Vector3.up * 1.5f, transform.rotation, null);
                    go.GetComponent<DamageNumberIndicator>().Initialize(damageAmount);
                    go.transform.SetParent(null);
                }

                eventsOnHit?.Invoke();
                timeToBecomeDamagableAgain = Time.time + invincibilityTime;
                isInvincableFromDamage = true;
                m_CurrentHealth = Mathf.Clamp(m_CurrentHealth - damageAmount, 0, maximumHealth);
                float currentHealthPct = m_CurrentHealth / maximumHealth;
                OnEnemyHealthPctChanged(currentHealthPct);
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
            m_CurrentHealth += healingAmount;
            if (m_CurrentHealth > maximumHealth) {
                m_CurrentHealth = maximumHealth;
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

        [Header("Effects & Polish")] [Tooltip("The effect to create when this health dies")]
        public GameObject deathEffect;

        [Tooltip("The effect to create when this health is damaged (but does not die)")]
        public GameObject hitEffect;

        [Tooltip("A list of events that occur when the health becomes 0 or lower")]
        public UnityEvent eventsOnDeath;

        [Tooltip("A list of events that occur when the health becomes 0 or lower")]
        public UnityEvent eventsOnHit;

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
            if (m_CurrentHealth <= 0) {
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
            if (deathEffect != null) {
                if (deathEffect != null) {
                    Instantiate(deathEffect, transform.position, transform.rotation, null);
                }
            }

            // Do on death events
            if (eventsOnDeath != null) {
                eventsOnDeath.Invoke();
            }
            
            OnEnemyHealthSystemRemoved(this);

        }
    }
}

