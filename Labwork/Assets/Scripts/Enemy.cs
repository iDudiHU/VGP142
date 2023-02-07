using System;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour
{
	public Transform target; // The player's transform
	public GameObject deathEffect;
	public GameObject player;
	private NavMeshAgent navMeshAgent;
	Animator m_Animator;
	private string m_currentAnimationClipName;
	private float deathTime;
	string m_ClipName;
	AnimatorClipInfo[] m_CurrentClipInfo;
	float m_CurrentClipLength;
	private float m_attackDelay;
	public LayerMask playerLayerMask;
	public enum EnemyState
	{
		Chase, Patrol
	}
	public float chaseDefaultLength = 2f;
	private float timeSinceLastChase;
	private float timeSinceLastAttack;
	private bool attackTriggerActivated = false;
	public EnemyState currentState = EnemyState.Patrol;
	public float vision = 8f;

	public GameObject[] path;
	public int pathIndex;
	public float distThreshhold;
	public float coneAngle = 60f;
	void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag("Player");
		m_Animator = GetComponentInChildren<Animator>();
		UpdateAnimClipTimes();
		try {
			m_Animator.SetFloat("locomotion", 1);
		} catch (System.Exception) {

			throw;
		}
		if (path.Length <= 0)
		{
			path = GameObject.FindGameObjectsWithTag("Patrol");
		}

		if (distThreshhold <= 0)
		{
			distThreshhold = 0.25f;
		}

	}

	public void UpdateAnimClipTimes()
	{
		AnimationClip[] clips = m_Animator.runtimeAnimatorController.animationClips;
		foreach (AnimationClip clip in clips) {
			switch (clip.name) {
				case "Dead":
					deathTime = clip.length;
					break;
			}
		}
	}

	void Update()
	{
		if (currentState == EnemyState.Patrol)
		{
			if (navMeshAgent.remainingDistance < distThreshhold && !navMeshAgent.pathPending)
			{
				pathIndex++;
				pathIndex %= path.Length;

				target = path[pathIndex].transform;
			}
			if (navMeshAgent.velocity.magnitude > 0) {
				currentState = IsPlayerVisible() ? EnemyState.Chase : EnemyState.Patrol;
			}
		}

		if (currentState == EnemyState.Chase) {
			target = player.transform;
			if (Vector3.Distance(player.transform.position,transform.position) <= 2 && Time.time - timeSinceLastAttack > m_attackDelay) {
				switch (UnityEngine.Random.Range(1, 4)) {
						case 1:
							m_Animator.SetTrigger("attack1");
							break;
						case 2:
							m_Animator.SetTrigger("attack2");
							break;
						case 3:
							m_Animator.SetTrigger("attack3");
							break;
						case 4:
							m_Animator.SetTrigger("attack4");
							break;
				}
				m_CurrentClipInfo = m_Animator.GetCurrentAnimatorClipInfo(0);
				m_CurrentClipLength = m_CurrentClipInfo[0].clip.length;
				m_attackDelay = m_CurrentClipLength;
				timeSinceLastAttack = Time.time + m_attackDelay; //Set timer to current game time plus our delay;
			}
			if (Time.time - timeSinceLastChase > chaseDefaultLength) {
				timeSinceLastChase = IsPlayerVisible() ? 0 : Time.time;
			}
			else {
				currentState = EnemyState.Patrol;
				target = path[pathIndex].transform;
			}
		}
		if (target)
			navMeshAgent.SetDestination(target.position);
	}

	private bool IsPlayerVisible()
	{
		Vector3 forward = transform.forward;
		Vector3 playerPos = player.transform.position;
		Vector3 currentPos = transform.position;
		Vector3 toPlayer = (playerPos - currentPos);
		float angle = Vector3.Angle(forward, toPlayer.normalized);
		if (!(angle < coneAngle / 2)) return false;
		RaycastHit hit;
		if (Physics.Raycast(currentPos, toPlayer.normalized * vision, out hit, vision,playerLayerMask)) {
			return hit.collider.gameObject.CompareTag("Player");
		}
		return false;
	}
	public void Die()
	{
		GameObject go = Instantiate(deathEffect, gameObject.transform);
		go.transform.SetParent(null);
		m_Animator.Play("Death");
		m_CurrentClipInfo = this.m_Animator.GetCurrentAnimatorClipInfo(0);
		m_CurrentClipLength = m_CurrentClipInfo[0].clip.length;
		Destroy(gameObject, m_CurrentClipLength);
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")) {
			other.gameObject.GetComponent<ThirdPersonCharacter>().Die();
		}
		if (other.CompareTag("AOE") || other.CompareTag("Punch")) {
			Die();
		}
	}
}
