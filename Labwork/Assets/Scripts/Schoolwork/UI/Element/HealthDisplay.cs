using System.Collections;
using Schoolwork.Systems;
using Schoolwork;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Schoolwork.UI.Element
{
    /// <summary>
    /// Class intended to work with grid layout groups to create an image based health bar
    /// </summary>
    public class HealthDisplay : UIelement
    {
        [Header("Settings")]
        [Tooltip("The health component to display health values for")]
        public HealthSystem displayedHealth = null;
        [Tooltip("The image which represents one unit of health")]
        public Slider healthDisplaySlider = null;
        [Tooltip("The TMP to display the current health")]
        public TextMeshProUGUI healthNumberTMP = null;
        [Tooltip("The TMP to display the max health")]
        public TextMeshProUGUI healthMaxTMP = null;
        public float updateSpeedSeconds = 0.5f;
    

        private void Start()
        {
            displayedHealth = GameManager.Instance.healthSystem;
            UpdateUI();
        }
		/// <summary>
		/// Description:
		/// Upadates this UI element
		/// Input: 
		/// none
		/// Return: 
		/// void (no return)
		/// </summary>
		public override void UpdateUI()
        {
            //if (GameManager.instance != null && GameManager.instance.player != null)
            //{
            //    Health playerHealth = GameManager.instance.player.GetComponent<Health>();
            //    if (playerHealth != null)
            //    {
            //        SetChildImageNumber(playerHealth.currentHealth);
            //    }
            //}
            if (displayedHealth != null)
            {
                SetChildHealthDisplay(displayedHealth.currentHealth);
            }
        }

        /// <summary>
        /// Description:
        /// Deletes and spawns images until this gameobject has as many children as the player has health
        /// Input: 
        /// int
        /// Return: 
        /// void (no return)
        /// </summary>
        /// <param name="targetHealthNumber">The number of images that this object should have as children</param>
        private void SetChildHealthDisplay(float targetHealthNumber)
        {
            if (healthDisplaySlider != null && healthNumberTMP != null && gameObject.activeSelf) {
                SetHealthBarSize(targetHealthNumber);
                SetHealthNumber(targetHealthNumber);
            }
        }
        private void SetHealthBarSize(float targetHealthNumber)
        {
            StartCoroutine(ChangeToPct(targetHealthNumber));
        }

        private void SetHealthNumber(float targetHealthNumber)
        {
            StartCoroutine(AnimateNumber(targetHealthNumber));
        }
        private IEnumerator AnimateNumber(float target)
        {
            float startHealth;
            float currentHealth;
            float.TryParse(healthNumberTMP.text, out startHealth);
            float elapsed = .0f;
            while (elapsed < updateSpeedSeconds) {
                elapsed += Time.deltaTime;
                currentHealth = Mathf.Lerp(startHealth, target, elapsed / updateSpeedSeconds);
                healthNumberTMP.text = Mathf.RoundToInt(currentHealth).ToString();
                healthMaxTMP.text = "/" + Mathf.RoundToInt(displayedHealth.maximumHealth);
            
                yield return null;
            }

            currentHealth = target;
            healthNumberTMP.text = Mathf.RoundToInt(currentHealth).ToString();
        }
        private IEnumerator ChangeToPct(float targetHealth)
        {
            float preChangePct = healthDisplaySlider.value;
            float targetPct = targetHealth / displayedHealth.maximumHealth;
            float elapsed = .0f;
            while (elapsed < updateSpeedSeconds) {
                elapsed += Time.deltaTime;
                healthDisplaySlider.value = Mathf.Lerp(preChangePct, targetPct, elapsed / updateSpeedSeconds);
                yield return null;
            }

            healthDisplaySlider.value = targetPct;
        }

		private void OnDisable()
		{
            StopAllCoroutines();
		}
	}
}
