using System;
using Schoolwork.UI;
using Schoolwork.UI.Element;
using TMPro;
using UnityEngine;

namespace Schoolwork.Systems
{
    public class LevelSystem : MonoBehaviour
    {
        public event EventHandler OnExperienceChanged;
        public event EventHandler OnLevelChanged;
        public event EventHandler OnAttributeSpent;
        public event EventHandler OnAllAttributesSpent;

        [Header("Systems")]
        public int level = 0;
        [Tooltip("The current experience of the player")]
        private float experience;
        public float Experience => experience;


        [Tooltip("The experience needed for the next level")]
        private float experienceToNextLevel = 100.0f;
        public float ExperienceToNextLevel => experienceToNextLevel;
        [Tooltip("The experience scale factor")]
        private float experienceScaleFactor = 1.1f;

        public int attributePoints;
        public int AttributePoints => attributePoints;

        [Header("Player Stats")]

        private ExpBar expBar;
        private TextMeshProUGUI levelText;

        private void Awake()
        {
            expBar = FindObjectOfType<ExpBar>();
            levelText = GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>();
            SetLevelNumber(level);
        }
        private void OnEnable()
        {
            SetupGameManagerLevelSystem();
        }
        private void SetupGameManagerLevelSystem()
        {
            if (GameManager.Instance != null && GameManager.Instance.levelSystem == null)
            {
                GameManager.Instance.levelSystem = this;
            }
        }
        public void AddExperience(float enemyExperience)
        {
            experience += enemyExperience * Mathf.Pow(experienceScaleFactor, level/2.0f);
            GameManager.UpdateUIElements();
            if (experience >= experienceToNextLevel) {
                LevelUp();
            }
        }

        void LevelUp()
        {
            while (experience >= experienceToNextLevel)
            {
                level++;
                attributePoints++;
                attributePoints++;
                experience -= experienceToNextLevel;
                CalculateNextLevel();
                SetLevelNumber(level);
                GameManager.UpdateUIElements();
                GameManager.Instance.healthSystem.AddMaxHealth(level);
                if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
            }

            GameManager.Instance.uiManager.ToggleLevelUp();
        }

        void CalculateNextLevel()
        {

            experienceToNextLevel *= Mathf.Pow(experienceScaleFactor, (level + 1));
        }

        public int GetLevelNumber()
        {
            return level;
        }
        public int GetAttributePoints()
        {
            return attributePoints;
        }

        public float GetExperienceNormalized()
        {
            return experience / experienceToNextLevel;
        }

        public void SpendAttribute()
        {
            if (attributePoints > 0)
			{
                attributePoints--;
                if (OnAttributeSpent != null) OnAttributeSpent(this, EventArgs.Empty);
                if (OnAllAttributesSpent != null && attributePoints == 0) OnAllAttributesSpent(this, EventArgs.Empty);
                if (attributePoints == 0)
                    GameManager.Instance.uiManager.ToggleLevelUp();
            }
        }
        private	void SetLevelNumber (int levelNumber)
        {
            levelText.text = (levelNumber + 1).ToString();
        }
    }
}
