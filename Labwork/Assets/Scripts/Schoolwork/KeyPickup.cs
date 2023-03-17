using Schoolwork.Helpers;
using Schoolwork.Systems;
using UnityEngine;

/// <summary>
/// Pickup-derived class which handles a key collectable
/// </summary>
namespace Schoolwork
{
    public class KeyPickup : PickUp
    {
        [Header("Key Settings")]

        [Tooltip("The ID of the key used to determine which doors it unlocks (unlocks doors with matching IDs)\n" +
                 "A key ID of 0 allows the player to open unlocked doors, and is therefore pointless.")]
        public int keyID = 0;

        /// <summary>
        /// Description:
        /// When picked up, adds to the player's score
        /// Inputs: 
        /// Collider collision
        /// Return: 
        /// void (no return)
        /// </summary>
        /// <param name="collision">The collider that picked up this key</param>
        public override void DoOnPickup()
        {
            KeyRing.AddKey(keyID);
            base.DoOnPickup();
            Destroy(gameObject);
        }
    }   
}

