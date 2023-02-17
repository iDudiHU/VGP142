using System;
using UnityEngine;

namespace Schoolwork.Systems
{
    public class WeaponSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            SetupGameManagerWeaponSystem();
        }
        private void SetupGameManagerWeaponSystem()
        {
            if (GameManager.Instance != null && GameManager.Instance.weaponSystem == null)
            {
                GameManager.Instance.weaponSystem = this;
            }
        }

        private void Start()
        {
            SetupGameManagerWeaponSystem();
        }
    }
}
