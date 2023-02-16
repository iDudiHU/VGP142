using System;
using Schoolwork.Systems;
using TMPro;
using UnityEngine;

namespace Schoolwork.UI
{
    public class LevelUpWindow : UIelement
    {
        [SerializeField]
        private TextMeshProUGUI levelText;
        private TextMeshProUGUI attributePoints;
        private LevelSystem _LevelSystem;

        private void Awake()
        {
            _LevelSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelSystem>();
            SetLevelNumber(_LevelSystem.GetLevelNumber());
            //attributePoints = transform.Find("AttributePoints").Find("AttributePointsText").GetComponent<TextMeshProUGUI>();
        }

        private	void SetLevelNumber (int levelNumber)
        {
            levelText.text = (levelNumber + 1).ToString();
        }

        private void OnEnable()
        {
            SetLevelNumber(_LevelSystem.GetLevelNumber());
            //SetAtrributePoints(_LevelSystem.GetAttributePoints());
            //_LevelSystem.OnAttributeSpent += _LevelSystem_OnAttributeSpent;
        }

        private void _LevelSystem_OnAttributeSpent(object sender, System.EventArgs e)
        {
            SetAtrributePoints(_LevelSystem.GetAttributePoints());
        }

        private void SetAtrributePoints (int attributePointsNumber)
        {
            attributePoints.text = "Attributes:\n" + (attributePointsNumber);
        }

    }
}