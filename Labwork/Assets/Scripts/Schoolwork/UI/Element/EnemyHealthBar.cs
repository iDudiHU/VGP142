using System;
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
        [SerializeField] private LayerMask layerMask;
        private Renderer renderer;

        private EnemyHealthSystem health;

        public void OnEnable()
        {
            healthSlider = GetComponent<Slider>();
            maskImage.color = new Color(maskImage.color.r, maskImage.color.g, maskImage.color.b, 0f);
        }

        public void SetHealth(EnemyHealthSystem health)
        {
            this.health = health;
            this.health.OnEnemyHealthPctChanged += HandleHealthChanged;
            renderer = health.GetComponentInChildren<Renderer>();
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

            //Vector3 targetViewportPos = GameManager.Instance.mainCamera.WorldToViewportPoint(health.transform.position);
            //bool targetIsVisible = (targetViewportPos.x > 0 && targetViewportPos.x < 1
            //                    && targetViewportPos.y > 0 && targetViewportPos.y < 1
            //                    && targetViewportPos.z > 0 && !Physics.Linecast(GameManager.Instance.mainCamera.transform.position, health.transform.position, layerMask)
            //                    && health.CurrentHealth < health.maximumHealth);
            if (health && GameManager.Instance.mainCamera != null)
			{
                bool targetIsVisible = !Physics.Linecast(GameManager.Instance.mainCamera.transform.position, health.transform.position, layerMask);

                Color maskColor = maskImage.color;
                maskColor.a = targetIsVisible ? 1f : 0f;
                maskImage.color = maskColor;

                if (targetIsVisible && renderer.isVisible)
                {
                    transform.position = (health.transform.position + Vector3.up * positionOffset);
                    transform.LookAt(2 * transform.position - GameManager.Instance.mainCamera.transform.position);
                }
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            health.OnEnemyHealthPctChanged -= HandleHealthChanged;
        }

		private void OnDisable()
		{
            StopAllCoroutines();
            health.OnEnemyHealthPctChanged -= HandleHealthChanged;
        }
	}
}
