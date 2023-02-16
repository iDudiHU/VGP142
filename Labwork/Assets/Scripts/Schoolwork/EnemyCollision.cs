using System;
using System.Collections;
using System.Collections.Generic;
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
          enemy.Die();
      }
  
      private void OnTriggerEnter(Collider other)
      {
          if (other.CompareTag("Player")) {
              other.gameObject.GetComponent<HealthSystem>().TakeDamage(enemy.Damage);
          }
      }
  }  
}

