using System.Collections;
using Schoolwork.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace Schoolwork.UI.Element
{
    public class EnemyHealthBar : MonoBehaviour
    {
        private Slider healthSlider;
        [SerializeField] private float updateSpeedSeconds = 0.5f;
        [SerializeField] private float positionOffset;
        [SerializeField] private Image maskImage;

        private EnemyHealthSystem health;

        public void OnEnable()
        {
            healthSlider = GetComponent<Slider>();
        }

        public void SetHealth(EnemyHealthSystem health)
        {
            this.health = health;
            health.OnEnemyHealthPctChanged += HandleHealthChanged;
        }

        private void HandleHealthChanged(float pct)
        {
            StartCoroutine(ChangeToPct(pct));
        }

        private IEnumerator ChangeToPct(float pct)
        {
            float preChangePct = healthSlider.value;
            float elapsed = .0f;
            while (elapsed < updateSpeedSeconds) {
                elapsed += Time.deltaTime;
                healthSlider.value = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
                yield return null;
            }

            healthSlider.value = pct;
        }

        private void LateUpdate()
        {
            transform.position = GameManager.Instance.mainCamera.WorldToScreenPoint(health.transform.position + Vector3.up * positionOffset);
        }

        private void OnDestroy()
        {
            health.OnEnemyHealthPctChanged -= HandleHealthChanged;
        }
    }
}
