using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class BowPickup : MonoBehaviour, IPickupable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

	public void Pickup()
	{
		Destroy(gameObject);
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
