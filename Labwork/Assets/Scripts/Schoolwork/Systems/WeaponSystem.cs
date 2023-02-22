using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Schoolwork.Systems
{
    public class WeaponSystem : MonoBehaviour
    {
        public Sprite unarmedSprite;
        public Sprite bowSprite;
        public Sprite staffSprite;
        public Sprite combinedSprite;
        public Sprite displaySprite;
        public enum WeaponTypes
        {
            Unarmed, Bow, Staff, Combined
        }
        public WeaponTypes currentWeapon = WeaponTypes.Unarmed;
        public string CurrentWeaponName
		{
			get
			{
                return Enum.GetName(typeof(WeaponTypes), currentWeapon);
			}
		}
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

        public void ChangeWeapon(WeaponTypes weaponTypeToChangeTo)
        {
            switch (weaponTypeToChangeTo)
            {
                case WeaponTypes.Unarmed:
                    currentWeapon = WeaponTypes.Unarmed;
                    displaySprite = unarmedSprite;
                    GameManager.Instance.player.GetComponent<ThirdPersonCharacter>().UnequipAllWeapons();
                    break;
                case WeaponTypes.Bow:
                    currentWeapon = WeaponTypes.Bow;
                    displaySprite = bowSprite;
                    GameManager.Instance.player.GetComponent<ThirdPersonCharacter>().PickupBow();
                    break;
                case WeaponTypes.Staff:
                    currentWeapon = WeaponTypes.Staff;
                    displaySprite = staffSprite;
                    GameManager.Instance.player.GetComponent<ThirdPersonCharacter>().PickupUpStaff();
                    break;
                case WeaponTypes.Combined:
                    currentWeapon = WeaponTypes.Combined;
                    displaySprite = combinedSprite;
                    GameManager.Instance.player.GetComponent<ThirdPersonCharacter>().EquipAllWeapons();
                    break;
            }
            GameManager.UpdateUIElements();
        }

        private void Start()
        {
            SetupGameManagerWeaponSystem();
        }
    }
}
