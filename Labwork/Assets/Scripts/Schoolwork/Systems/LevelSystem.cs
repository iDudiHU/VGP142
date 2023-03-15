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
        [SerializeField]
        private float _currentExperience;
        public float Experience => _currentExperience;


        [Tooltip("The experience needed for the next level")]
        [SerializeField]
        private float _experienceNeededForNextLevel = 100.0f;
        public float ExperienceToNextLevel => _experienceNeededForNextLevel;
        [Tooltip("The experience scale factor")]
        private float experienceScaleFactor = 1.1f;

        public int attributePoints;
        public int AttributePoints => attributePoints;

        private void Awake()
        {
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
                GameManager.UpdateUIElements();
                GameManager.Instance.healthSystem.AddMaxHealth(_currentLevel);
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

		public void Load(GameData.LevelData levelData)
		{
            _currentLevel = levelData._level;
            _currentExperience = levelData._currentExperience;
            _experienceNeededForNextLevel = levelData._experienceNeededForNextLevel;
            attributePoints = levelData._attributePoints;
        }

        public void Save(ref GameData data)
		{
            data.player.levelData._level = _currentLevel;
            data.player.levelData._currentExperience = _currentExperience;
            data.player.levelData._experienceNeededForNextLevel = _experienceNeededForNextLevel;
            data.player.levelData._attributePoints = attributePoints;
        }
	}
}
