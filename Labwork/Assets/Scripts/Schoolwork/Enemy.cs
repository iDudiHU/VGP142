using System;
using System.Collections.Generic;
using Schoolwork.Helpers;
using Schoolwork.Systems;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityStandardAssets.Characters.ThirdPerson;
using Random = UnityEngine.Random;
using System.Collections;

namespace Schoolwork
{
	public class Enemy : MonoBehaviour, IDamageable
	{
		public static event Action<Enemy> EnemyCreated;
		public static event Action<Enemy> EnemyDestroyed;

		[SerializeField] private string id = string.Empty;

		public string Id => id;

		[ContextMenu("Generate id")]
		public void GenerateId()
		{
			if (String.IsNullOrEmpty(id))
			{
				id = Guid.NewGuid().ToString();
			}
		}

		public enum EnemyTypes
		{
			Anubis, Demon, Fishman
		}
		public EnemyTypes enemyType = EnemyTypes.Anubis;
		public Transform target; // The player's transform
		public GameObject deathEffect;
		public GameObject player;
		public ThirdPersonCharacter TPC;
		public GameObject experienceDrop;
		[SerializeField]
		private float experienceDropValue;
		private NavMeshAgent navMeshAgent;
		Animator m_Animator;
		private string m_currentAnimationClipName;
		private float deathTime;
		string m_ClipName;
		AnimatorClipInfo[] m_CurrentClipInfo;
		float m_CurrentClipLength;
		private float m_attackDelay;
		[SerializeField]
		private float m_damage;
		[SerializeField]
		private Collider m_AttackCollider;

		private EnemyHealthSystem enemyHealthSystem;

		public float Damage => m_damage;

		public LayerMask playerLayerMask;
		public enum EnemyState
		{
			Chase, Patrol, Attacking, Die
		}
		public float chaseDefaultLength = 2f;
		private float timeSinceLastChase;
		private float timeSinceLastAttack;
		public EnemyState currentState = EnemyState.Patrol;
		public float vision = 8f;
		[SerializeField]
		private GameObject WaypointHolder;

		public List<GameObject> path = new List<GameObject>();
		public int pathIndex;
		public float distThreshhold;
		public float coneAngle = 120f;
		private bool isPlayerDead;
		void Start()
		{
			navMeshAgent = GetComponent<NavMeshAgent>();
			player = GameObject.FindGameObjectWithTag("Player");
			TPC = GameManager.Instance.player.GetComponent<ThirdPersonCharacter>();
			TPC.OnPlayerDeath += TPCOnPlayerDeath;
			m_Animator = GetComponentInChildren<Animator>();
			enemyHealthSystem = GetComponent<EnemyHealthSystem>();
			UpdateAnimClipTimes();
			try
			{
				m_Animator.SetFloat("locomotion", 1);
			}
			catch (System.Exception)
			{

				throw;
			}

			if (path.Count <= 0)
			{
				Transform parentTransform = WaypointHolder.transform;

				// Iterate over all of the child transforms
				for (int i = 0; i < parentTransform.childCount; i++)
				{
					Transform childTransform = parentTransform.GetChild(i);

					// Check if the child has the "patrol" tag
					if (childTransform.CompareTag("Patrol"))
					{
						GameObject patrolObject = childTransform.gameObject;
						path.Add(patrolObject);
					}
				}
			}

			if (distThreshhold <= 0)
			{
				distThreshhold = 0.25f;
			}
			EnemyCreated?.Invoke(this);

		}

		private void TPCOnPlayerDeath(object sender, EventArgs e)
		{
			navMeshAgent.ResetPath();
			target = null;
			currentState = EnemyState.Patrol;
			target = path[0].transform;

		}

		public void UpdateAnimClipTimes()
		{
			AnimationClip[] clips = m_Animator.runtimeAnimatorController.animationClips;
			foreach (AnimationClip clip in clips)
			{
				switch (clip.name)
				{
					case "Dead":
						deathTime = clip.length;
						break;
				}
			}
		}

		public void AttackStart()
		{

		}
		public void AttackEnd()
		{

		}

		void Update()
		{
			if (currentState == EnemyState.Die)
			{
				navMeshAgent.ResetPath();
				target = null;
			}
			if (currentState == EnemyState.Patrol)
			{
				if (navMeshAgent.remainingDistance < distThreshhold && !navMeshAgent.pathPending)
				{
					pathIndex++;
					pathIndex %= path.Count;

					target = path[pathIndex].transform;
				}
				if (navMeshAgent.velocity.magnitude > 0 && !isPlayerDead)
				{
					currentState = IsPlayerVisible() ? EnemyState.Chase : EnemyState.Patrol;
				}
			}

			if (!isPlayerDead)
			{
				if (currentState == EnemyState.Chase)
				{
					target = player.transform;
					if (Vector3.Distance(player.transform.position, transform.position) <= 2 && (Time.time - timeSinceLastAttack > m_attackDelay))
					{
						StartCoroutine(AttackCoroutine("attack" + Random.Range(1, 4).ToString()));
						m_CurrentClipInfo = m_Animator.GetCurrentAnimatorClipInfo(0);
						m_CurrentClipLength = m_CurrentClipInfo[0].clip.length;
						m_attackDelay = m_CurrentClipLength;
						timeSinceLastAttack = Time.time + m_attackDelay; //Set timer to current game time plus our delay;
					}

					if (IsPlayerVisible())
					{
						timeSinceLastChase = Time.time;
					}

					if (Time.time - timeSinceLastChase > chaseDefaultLength)
					{
						currentState = EnemyState.Patrol;
						target = path[pathIndex].transform;
					}
				}
				else
				{
					currentState = EnemyState.Patrol;
					target = path[pathIndex].transform;
				}
			}
			else
			{
				currentState = EnemyState.Patrol;
				target = path[pathIndex].transform;
			}

			if (target)
				navMeshAgent.SetDestination(target.position);
		}

		private bool IsPlayerVisible()
		{
			if (!isPlayerDead)
			{
				Vector3 forward = transform.forward;
				Vector3 playerPos = player.transform.position;
				Vector3 currentPos = transform.position;
				Vector3 toPlayer = (playerPos - currentPos);
				float angle = Vector3.Angle(forward, toPlayer.normalized);
				if (!(angle < coneAngle / 2)) return false;
				RaycastHit hit;
				if (Physics.Raycast(currentPos, toPlayer.normalized * vision, out hit, vision, playerLayerMask))
				{
					return hit.collider.gameObject.CompareTag("Player");
				}
			}
			return false;
		}
		public void Die()
		{
			GameObject go = Instantiate(deathEffect, gameObject.transform);
			if (Random.Range(0.0f, 1.0f) > 0.2f)
			{
				GameObject Exp = Instantiate(experienceDrop, gameObject.transform.position + new Vector3(.0f, 1.0f, .0f), gameObject.transform.rotation);
				Exp.GetComponent<ExpPickUp>().Init(experienceDropValue);
				Exp.transform.SetParent(null);
			}
			currentState = EnemyState.Die;

			go.transform.SetParent(null);
			GetComponent<CapsuleCollider>().enabled = false;
			m_Animator.Play("Death");
			m_CurrentClipInfo = this.m_Animator.GetCurrentAnimatorClipInfo(0);
			m_CurrentClipLength = m_CurrentClipInfo[0].clip.length;
			EnemyDestroyed?.Invoke(this);
			Destroy(gameObject, m_CurrentClipLength);
		}
		private IEnumerator AttackCoroutine(string attackString)
		{
			m_Animator.SetTrigger(attackString);
			m_CurrentClipInfo = m_Animator.GetCurrentAnimatorClipInfo(0);
			m_CurrentClipLength = m_CurrentClipInfo[0].clip.length;
			m_attackDelay = m_CurrentClipLength;
			// Set the attack indicator to active
			m_AttackCollider.gameObject.SetActive(true);

			// Wait for the attack to finish
			yield return new WaitForSeconds(m_CurrentClipLength);

			// Set the attack indicator back to inactive
			m_AttackCollider.gameObject.SetActive(false);
		}

		public void OnDestroy()
		{
			if (TPC)
			{
				TPC.OnPlayerDeath -= TPCOnPlayerDeath;
			}
		}

		public void OnDamage(float damageAmount)
		{
			enemyHealthSystem.TakeDamage(damageAmount);
		}
		public void Load(GameData.EnemyData data)
		{
			// Else load enemy data
			currentState = data.currentState;

			//Set position
			transform.position = data.transformData.position;
			//Set rotation
			transform.localRotation = data.transformData.rotation;
			//Set scale
			transform.localScale = data.transformData.scale;
			//Set health
			GetComponent<EnemyHealthSystem>().Load(ref data);
		}
		public void Save(ref GameData data)
		{
			GameData.EnemyData enemyData = new GameData.EnemyData();
			//Enemy Data
			enemyData.Id = Id;
			enemyData.enemyType = enemyType;
			//TransformData
			enemyData.transformData.position = transform.position;
			enemyData.transformData.rotation = transform.rotation;
			enemyData.transformData.scale = transform.localScale;
			//HealthData
			GetComponent<EnemyHealthSystem>().Save(ref enemyData.healthData);
			//Add to the list
			data.enemyDataList.Add(enemyData);
		}
	}
}

