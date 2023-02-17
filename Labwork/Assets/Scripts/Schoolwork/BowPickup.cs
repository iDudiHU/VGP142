using Schoolwork.Helpers;
using UnityEngine;

namespace Schoolwork
{
	public class BowPickup : PickUp
	{
		public  override void DoOnPickup()
		{
			base.DoOnPickup();
			Destroy(gameObject, 0.5f);
		}
	}
}

