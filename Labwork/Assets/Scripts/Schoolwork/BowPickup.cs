using Schoolwork.Helpers;
using UnityEngine;

namespace Schoolwork
{
	public class BowPickup : PickUp
	{
		public  override void DoOnPickup(Collider collision)
		{
			if (collision.CompareTag("Player")) {
				base.DoOnPickup(collision);
				Destroy(gameObject, 0.5f);
			}
		}
	}
}

