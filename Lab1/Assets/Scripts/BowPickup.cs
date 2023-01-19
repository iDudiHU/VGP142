using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class BowPickup : MonoBehaviour, IPickup
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")) {
			OnPickup();
			other.gameObject.GetComponent<ThirdPersonCharacter>().Pickup(gameObject);
			Destroy(gameObject);
		}
	}

	public void OnPickup()
	{

	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
