using UnityEngine.AI;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour
{
	public GameObject target; // The player's transform
	public GameObject deathEffect;
	private NavMeshAgent navMeshAgent;
	Animator animator;

	void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		target = GameObject.FindWithTag("Player");
		animator = GetComponentInChildren<Animator>();
		try {
			animator.SetFloat("locomotion", 1);
		} catch (System.Exception) {

			throw;
		}
		
	}

	void Update()
	{
		navMeshAgent.SetDestination(target.transform.position);

	}
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")) {
			//other.gameObject.GetComponent<ThirdPersonCharacter>().Pickup(gameObject);
			GameObject go = Instantiate(deathEffect, gameObject.transform);
			go.transform.SetParent(null);
			Destroy(gameObject);
			throw new System.Exception("Collision occurred with player!");
		}
	}
}
