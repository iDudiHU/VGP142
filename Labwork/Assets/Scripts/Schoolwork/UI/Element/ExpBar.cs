using System;
using System.Collections;
using System.Globalization;
using Schoolwork.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Schoolwork.UI.Element
{
    public class ExpBar : UIelement
    {
        private Slider expSlider;
        private LevelSystem _LevelSystem;
        public TextMeshProUGUI currentExpTMP;
        public TextMeshProUGUI maxExpTMP;
        public TextMeshProUGUI currentLevel;
        public float updateSpeedSeconds = 0.5f;
        private void Awake()
        {
            _LevelSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelSystem>();
            expSlider = GetComponent<Slider>();
        }
        private void Start()
        {
            if (_LevelSystem == null && (GameManager.Instance != null && GameManager.Instance.player != null))
            {
                _LevelSystem = GameManager.Instance.player.GetComponent<LevelSystem>();
            }
            UpdateUI();
        }
        
        public override void UpdateUI()
        {
            if (_LevelSystem != null && currentExpTMP != null && gameObject.activeSelf)
            {
                SetExperienceBarSize(_LevelSystem.GetExperienceNormalized());
                SetExperienceNumber(_LevelSystem.Experience, _LevelSystem.ExperienceToNextLevel);
                currentLevel.text = (_LevelSystem._currentLevel + 1).ToString();
            }
        }
        private void SetExperienceBarSize(float experienceNormalized)
        {
            StartCoroutine(ChangeToPct(experienceNormalized));
        }

        private void SetExperienceNumber(float currentExp, float maxExp)
        {
            StartCoroutine(AnimateNumber(currentExp, maxExp));
        }
        private IEnumerator AnimateNumber(float target, float maxExp)
        {
            float startExp;
            float currentExp;
            float.TryParse(currentExpTMP.text, out startExp);
            float elapsed = .0f;
            while (elapsed < updateSpeedSeconds) {
                elapsed += Time.deltaTime;
                currentExp = Mathf.Lerp(startExp, target, elapsed / updateSpeedSeconds);
                currentExpTMP.text = Mathf.RoundToInt(currentExp).ToString();
                maxExpTMP.text = "/" + Mathf.RoundToInt(maxExp);
            
                yield return null;
            }

            currentExp = target;
            currentExpTMP.text = Mathf.RoundToInt(currentExp).ToString();
        }
        private IEnumerator ChangeToPct(float targetExp)
        {
            float preChangePct = expSlider.value;
            float targetPct = targetExp;
            float elapsed = .0f;
            while (elapsed < updateSpeedSeconds) {
                elapsed += Time.deltaTime;
                expSlider.value = Mathf.Lerp(preChangePct, targetPct, elapsed / updateSpeedSeconds);
                yield return null;
            }

            expSlider.value = targetExp;
        }
		private void OnDisable()
		{
            StopAllCoroutines();
        }

	}
}