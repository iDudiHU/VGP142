using UnityEngine.AI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public GameObject target; // The player's transform
	private NavMeshAgent navMeshAgent;

	void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		target = GameObject.FindWithTag("Player");
	}

	void Update()
	{
		navMeshAgent.SetDestination(target.transform.position);
	}
}
