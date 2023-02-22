using System;
using System.Collections;
using System.Collections.Generic;
using Schoolwork.Helpers;
using Schoolwork.Systems;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Schoolwork
{
    public class EnemyCollision : MonoBehaviour
    {
        public Enemy enemy;

        private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag("Player")) {
                enemy.OnDamage(other.GetComponent<ParticleCollisionInstance>().MDamageAmmount);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null && other.CompareTag("Player")) {
                damageable.OnDamage(enemy.Damage);
            }
            if (other.CompareTag("AOE"))
            {
                enemy.OnDamage(UnityEngine.Random.Range(400, 800));
            }
            if (other.CompareTag("Punch"))
            {
                enemy.OnDamage(UnityEngine.Random.Range(100, 200));
            }

        }
    }
}

