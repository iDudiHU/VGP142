using UnityEngine;

namespace Schoolwork.Helpers
{
    public class PickUp : MonoBehaviour, IPickupable
    {   [Header("Settings")]
        [Tooltip("The effect to create when this pickup is collected")]
        public GameObject pickUpEffect;
        public virtual void DoOnPickup()
        {
            if (pickUpEffect != null)
            {
                Instantiate(pickUpEffect, transform.position, Quaternion.identity, null);
            }
        }

        public virtual void DoInRange()
        {
            
        }
    }
}