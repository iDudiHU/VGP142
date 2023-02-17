using Schoolwork.Helpers;
using Schoolwork.Systems;
using UnityEngine;

namespace Schoolwork
{
	public class StaffPickup : PickUp
	{
		public  override void DoOnPickup()
		{
			GameManager.Instance.weaponSystem.ChangeWeapon(WeaponSystem.WeaponTypes.Staff);
			base.DoOnPickup();
			Destroy(gameObject, 0.5f);
		}
	}
}

