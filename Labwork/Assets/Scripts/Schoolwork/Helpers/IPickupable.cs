using UnityEngine;

namespace Schoolwork.Helpers
{
	public interface IPickupable
	{
		void DoOnPickup(Collider collider);
		void DoInRange();
	}
	
}

