using System.Collections;
using Schoolwork.Systems;
using Schoolwork;
using TMPro;
using UnityEngine;
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
        [Tooltip("The TMP to display the number")]
        public TextMeshProUGUI healthNumberDisplay = null;
        [Tooltip("The maximum health to display")]
        public int maximumHealthToDisplay = 100;

        public float updateSpeedSeconds = 0.5f;
    

        private void Start()
        {
            if (displayedHealth == null && (GameManager.Instance != null && GameManager.Instance.player != null))
            {
                displayedHealth = GameManager.Instance.player.GetComponentInChildren<HealthSystem>();
            }
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
            if (healthDisplaySlider != null) {
                if (maximumHealthToDisplay <= targetHealthNumber)
                    return;
                if (healthNumberDisplay == null)
                    return;
                StartCoroutine(ChangeToPct(targetHealthNumber));
                StartCoroutine(AnimateNumber(targetHealthNumber));
            }
        }
        private IEnumerator AnimateNumber(float target)
        {
            float startHealth;
            float currentHealth;
            float.TryParse(healthNumberDisplay.text, out startHealth);
            float elapsed = .0f;
            while (elapsed < updateSpeedSeconds) {
                elapsed += Time.deltaTime;
                currentHealth = Mathf.Lerp(startHealth, target, elapsed / updateSpeedSeconds);
                healthNumberDisplay.text = Mathf.RoundToInt(currentHealth) + "/100";
            
                yield return null;
            }

            currentHealth = target;
            healthNumberDisplay.text = Mathf.RoundToInt(currentHealth) + "/100";
        }
        private IEnumerator ChangeToPct(float targetHealth)
        {
            float preChangePct = healthDisplaySlider.value;
            float targetPct = targetHealth / maximumHealthToDisplay;
            float elapsed = .0f;
            while (elapsed < updateSpeedSeconds) {
                elapsed += Time.deltaTime;
                healthDisplaySlider.value = Mathf.Lerp(preChangePct, targetPct, elapsed / updateSpeedSeconds);
                yield return null;
            }

            healthDisplaySlider.value = targetPct;
        }
    }
}
