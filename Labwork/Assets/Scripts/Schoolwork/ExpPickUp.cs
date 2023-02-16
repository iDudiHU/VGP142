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

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        public  override void DoOnPickup(Collider collision)
        {
            if (collision.CompareTag("Player")) {
                collision.GetComponent<LevelSystem>().AddExperience(experienceValue);
                base.DoOnPickup(collision);
                Destroy(gameObject);
            }
        }

        public override void DoInRange()
        {
            StartCoroutine(LerpFunction( lerpDuration));
        }
        IEnumerator LerpFunction( float duration)
        {
            float time = 0;
            Vector3 startPosition = transform.position;
            while (time < duration)
            {
                transform.position = Vector3.Lerp(startPosition, player.transform.position + new Vector3(.0f,1.0f,.0f), time / lerpDuration);
                time += Time.deltaTime;
                yield return null;
            }
            transform.position = player.transform.position + new Vector3(.0f,1.0f,.0f); 
        }
    }
}
