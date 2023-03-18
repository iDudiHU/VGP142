using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Schoolwork.Systems.WeaponSystem;
using Schoolwork.UI;

namespace Schoolwork.Systems
{
    public class TooltipManager : UIelement
    {
        [SerializeField]
        private List<TextMeshProUGUI> m_CurrentWeaponInfoTMP;
        [SerializeField]
        private TextMeshProUGUI m_CurrentWeaponNameTMP;
        // Start is called before the first frame update
        private void Start()
        {
            SetActiveWeapon(GameManager.Instance.weaponSystem.currentWeapon);
        }

        public override void UpdateUI()
        {
            SetActiveWeapon(GameManager.Instance.weaponSystem.currentWeapon);
        }

        public void SetActiveWeapon(WeaponTypes weaponType)
        {
            m_CurrentWeaponNameTMP.text = GameManager.Instance.weaponSystem.CurrentWeaponName;

            for (int i = 0; i < m_CurrentWeaponInfoTMP.Count; i++)
            {
                if (i == (int)weaponType)
                {
                    m_CurrentWeaponInfoTMP[i].gameObject.SetActive(true);
                }
                else
                {
                    m_CurrentWeaponInfoTMP[i].gameObject.SetActive(false);
                }
            }
        }
    }
}


