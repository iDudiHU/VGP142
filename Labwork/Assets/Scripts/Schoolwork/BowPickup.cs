using Schoolwork.Helpers;
using Schoolwork.Systems;
using UnityEngine;

namespace Schoolwork
{
	public class BowPickup : PickUp
	{
		public  override void DoOnPickup()
		{
			GameManager.Instance.weaponSystem.ChangeWeapon(WeaponSystem.WeaponTypes.Bow);
			base.DoOnPickup();
			Destroy(gameObject, 0.5f);
		}
	}
}

