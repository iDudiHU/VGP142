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
        public int _currentLevel = 0;
        [Tooltip("The current experience of the player")]
        private float _currentExperience;
        public float Experience => _currentExperience;


        [Tooltip("The experience needed for the next level")]
        private float _experienceNeededForNextLevel = 100.0f;
        public float ExperienceToNextLevel => _experienceNeededForNextLevel;
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
            SetLevelNumber(_currentLevel);
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
            _currentExperience += enemyExperience * Mathf.Pow(experienceScaleFactor, _currentLevel/2.0f);
            GameManager.UpdateUIElements();
            if (_currentExperience >= _experienceNeededForNextLevel) {
                LevelUp();
            }
        }

        void LevelUp()
        {
            while (_currentExperience >= _experienceNeededForNextLevel)
            {
                _currentLevel++;
                attributePoints++;
                attributePoints++;
                _currentExperience -= _experienceNeededForNextLevel;
                CalculateNextLevel();
                SetLevelNumber(_currentLevel);
                GameManager.UpdateUIElements();
                GameManager.Instance.healthSystem.AddMaxHealth(_currentLevel);
                if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
            }

            GameManager.Instance.uiManager.ToggleLevelUp();
        }

        void CalculateNextLevel()
        {

            _experienceNeededForNextLevel *= Mathf.Pow(experienceScaleFactor, (_currentLevel + 1));
        }

        public int GetLevelNumber()
        {
            return _currentLevel;
        }
        public int GetAttributePoints()
        {
            return attributePoints;
        }

        public float GetExperienceNormalized()
        {
            return _currentExperience / _experienceNeededForNextLevel;
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

		public void Load(LevelData levelData)
		{
            _currentLevel = levelData._level;
            _currentExperience = levelData._currentExperience;
            _experienceNeededForNextLevel = levelData._experienceNeededForNextLevel;
        }

        public LevelData Save()
		{
            LevelData levelData = new LevelData(0, 0, 0);
            levelData._level = _currentLevel;
            levelData._currentExperience = _currentExperience;
            levelData._experienceNeededForNextLevel = _experienceNeededForNextLevel;
            return levelData;
        }
	}
}
