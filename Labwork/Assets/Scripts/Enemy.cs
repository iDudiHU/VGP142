using UnityEngine.AI;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour
{
	public GameObject target; // The player's transform
	public GameObject deathEffect;
	private NavMeshAgent navMeshAgent;
	Animator m_Animator;
	private float deathTime;
	string m_ClipName;
	AnimatorClipInfo[] m_CurrentClipInfo;
	float m_CurrentClipLength;

	void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		target = GameObject.FindWithTag("Player");
		m_Animator = GetComponentInChildren<Animator>();
		UpdateAnimClipTimes();
		try {
			m_Animator.SetFloat("locomotion", 1);
		} catch (System.Exception) {

			throw;
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
		navMeshAgent.SetDestination(target.transform.position);

	}

	public void Die()
	{
		GameObject go = Instantiate(deathEffect, gameObject.transform);
		go.transform.SetParent(null);
		m_Animator.Play("Death");
		m_CurrentClipInfo = this.m_Animator.GetCurrentAnimatorClipInfo(0);
		m_CurrentClipLength = m_CurrentClipInfo[0].clip.length;
		Destroy(gameObject, m_CurrentClipLength);
		throw new System.Exception("Collision occurred with player!");
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")) {
			//other.gameObject.GetComponent<ThirdPersonCharacter>().Pickup(gameObject);
			Die();
		}
	}
}
