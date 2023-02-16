using System;
using Schoolwork.UI;
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
        public float experience;
        [Tooltip("The experience needed for the next level")]
        public float experienceToNextLevel;
        [Tooltip("The experience scale factor")]
        private float experienceScaleFactor = 1.1f;

        public int attributePoints;

        [Header("Player Stats")]
        public int extraMaxLives;
        public int extraJump;
        public int extraSpeed;

        private ExpBar expBar;
        private TextMeshProUGUI levelText;

        private void Awake()
        {
            expBar = FindObjectOfType<ExpBar>();
            levelText = GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>();
            SetLevelNumber(level);
        }
        public void AddExperience(float enemyExperience)
        {
            experience += enemyExperience * Mathf.Pow(experienceScaleFactor, level/2.0f);


            if (experience >= experienceToNextLevel) {
                LevelUp();
            }
            if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
        }

        void LevelUp()
        {
            level++;
            attributePoints++;
            experience -= experienceToNextLevel;
            CalculateNextLevel();
            SetLevelNumber(level);
            
            if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
            if (experience >= experienceToNextLevel)
            {
                LevelUp();
            }
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
            attributePoints--;
            if(OnAttributeSpent != null) OnAttributeSpent(this, EventArgs.Empty);
            if(OnAllAttributesSpent != null && attributePoints == 0) OnAllAttributesSpent(this, EventArgs.Empty);
        }
        private	void SetLevelNumber (int levelNumber)
        {
            levelText.text = (levelNumber + 1).ToString();
        }
    }
}
