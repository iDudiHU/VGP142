using System.Collections.Generic;
using Schoolwork.Systems;
using Schoolwork.UI.Element;
using UnityEngine;

namespace Schoolwork.UI
{
   public class HealthBarManager : MonoBehaviour
   {
      [SerializeField] private EnemyHealthBar healthBarPrefab;

      private Dictionary<EnemyHealthSystem, EnemyHealthBar> healthBars = new Dictionary<EnemyHealthSystem, EnemyHealthBar>();

      private void Awake()
      {
         EnemyHealthSystem.OnEnemyHealthSystemAdded += AddHealthBar;
         EnemyHealthSystem.OnEnemyHealthSystemRemoved += RemoveHealthBar;
      }

      private void AddHealthBar(EnemyHealthSystem health)
      {
         if (healthBars.ContainsKey(health) == false) {
            var healthBar = Instantiate(healthBarPrefab, transform);
            healthBars.Add(health, healthBar);
            healthBar.SetHealth(health);
         }
      }

      private void RemoveHealthBar(EnemyHealthSystem health)
      {
         if (healthBars.ContainsKey(health)) {
            Destroy(healthBars[health].gameObject);
            healthBars.Remove(health);
         }
      }
   }
}
