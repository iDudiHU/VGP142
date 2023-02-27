using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Schoolwork.UI.Element
{
    public class DamageNumberIndicator : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI numberTMP;
        [SerializeField]
        private Color m_CriticalColor;
        [SerializeField]
        private Color m_NormallColor;
        [SerializeField]
        private AnimationCurve sizeCurve;

        private float destroyTime = 1.3f;
        private float currentLifeTime = 0f;

        public void Initialize(float damageValue)
        {
            Destroy(this.gameObject, destroyTime);
            transform.LookAt(2 * transform.position - GameManager.Instance.mainCamera.transform.position);
            numberTMP.text = damageValue.ToString();
            numberTMP.color = damageValue >= 450 ? m_CriticalColor : m_NormallColor;
        }

        private void Update()
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            currentLifeTime += Time.deltaTime;

            if (currentLifeTime < destroyTime)
            {
                transform.position += new Vector3(0f, 0.3f, 0f) * Time.deltaTime;
                numberTMP.fontSize = sizeCurve.Evaluate(currentLifeTime / destroyTime);
            }
            transform.LookAt(2 * transform.position - GameManager.Instance.mainCamera.transform.position);
        }
    }
}


