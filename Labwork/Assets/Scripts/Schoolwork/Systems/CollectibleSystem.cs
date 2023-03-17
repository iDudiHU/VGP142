using Schoolwork.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Schoolwork.Systems
{
	[DefaultExecutionOrder(-1)]
	public class CollectibleSystem : MonoBehaviour
	{
		public List<PickUp> collectibles = new List<PickUp>();
		public List<string> collectibleGuids = new List<string>();
		private void Awake()
		{
			PickUp.PickupCreated += PickupCreated;
			PickUp.PickupDestroyed += PickupDeath;
		}
		private void PickupCreated(PickUp pickup)
		{
			collectibles.Add(pickup);
			collectibleGuids.Add(pickup.Id);
		}
		private void PickupDeath(PickUp pickup)
		{
			collectibles.Remove(pickup);
			collectibleGuids.Remove(pickup.Id);
		}
		public void OnEnable()
		{
			SetupGameManagerReference();
		}
		private void SetupGameManagerReference()
		{
			GameManager.Instance.collectibleSystem = this;
		}
		private void OnDisable()
		{
			PickUp.PickupCreated -= PickupCreated;
			PickUp.PickupDestroyed -= PickupDeath;
		}
	}
}


