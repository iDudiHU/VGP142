using Schoolwork.Helpers;
using Schoolwork.Systems;
using UnityEngine;

/// <summary>
/// This class inherits from the Pickup class and will heal the player when picked up
/// </summary>
namespace Schoolwork
{
    public class HealthPickup : PickUp
    {
        [Header("Healing Settings")]
        [Tooltip("The healing to apply")]
        public int healingAmount = 1;

        /// <summary>
        /// Description:
        /// Function called when this pickup is picked up
        /// Heals the health attatched to the collider that picks this up
        /// Input: 
        /// Collider collision
        /// Return: 
        /// void (no return)
        /// </summary>
        /// <param name="collision">The collider that is picking up this pickup</param>
        public override void DoOnPickup(Collider collision)
        {
            if (collision.tag == "Player" && collision.gameObject.GetComponent<HealthSystem>() != null)
            {
                HealthSystem playerHealth = collision.gameObject.GetComponent<HealthSystem>();
                playerHealth.ReceiveHealing(healingAmount);
                GameManager.UpdateUIElements();
            }
            base.DoOnPickup(collision);
        }
    }
}

