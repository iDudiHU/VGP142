using Schoolwork.Helpers;
using UnityEngine;

namespace Schoolwork
{
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
}

