using Schoolwork.Helpers;
using Schoolwork.Systems;
using UnityEngine;

namespace Schoolwork
{
	public class CombinedPickup : PickUp
	{
		public  override void DoOnPickup()
		{
			GameManager.Instance.weaponSystem.ChangeWeapon(WeaponSystem.WeaponTypes.Combined);
			base.DoOnPickup();
			Destroy(gameObject, 0.5f);
		}
	}
}

