using System;
using System.Collections;
using Schoolwork.Helpers;
using Schoolwork.Systems;
using Unity.VisualScripting;
using UnityEngine;

namespace Schoolwork
{
    public class ExpPickUp : PickUp
    {
        public float experienceValue;
        [SerializeField]
        private float lerpDuration = 1.5f;
        private GameObject player;
        private bool isPickedUp;
        public AnimationCurve animCurve;
        public LayerMask layerMask;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        public void Init(float expValue)
        {
            experienceValue = expValue;
        }
        public  override void DoOnPickup()
        {
                GameManager.Instance.levelSystem.AddExperience(experienceValue);
                base.DoOnPickup();
                Destroy(gameObject);
        }

        public override void DoInRange()
        {
            StartCoroutine(LerpFunction(lerpDuration));
        }
        IEnumerator LerpFunction( float duration)
        {
            float time = 0;
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = player.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
            while (time < duration)
            {
                float t = time / lerpDuration;
                t = animCurve.Evaluate(t);
                targetPosition = player.transform.position + new Vector3(0.0f, 1.0f, 0.0f); // update target position
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                time += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPosition;
            gameObject.layer = 9;
        }
    }
}
