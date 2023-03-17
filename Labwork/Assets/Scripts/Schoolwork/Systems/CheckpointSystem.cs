using Schoolwork.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Schoolwork.Systems
{
	public class CheckpointleSystem : MonoBehaviour
	{
		public static Checkpoint currentCheckpoint = null;
		public void OnEnable()
		{
			SetupGameManagerReference();
		}
		private void SetupGameManagerReference()
		{
			GameManager.Instance.checkpointSystem = this;
		}
	}
}


