using System.Collections;
using System.Collections.Generic;
using Schoolwork.Helpers;
using Schoolwork.Systems;
using UnityEngine;

/// <summary>
/// Class which manages a checkpoint
/// </summary>
namespace Schoolwork
{
    public class Checkpoint : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The location this checkpoint will respawn the player at")]
    public Transform respawnLocation;
    [Tooltip("The animator for this checkpoint")]
    public Animator checkpointAnimator = null;
    [Tooltip("The name of the parameter in the animator which determines if this checkpoint displays as active")]
    public string animatorActiveParameter = "isActive";
    [Tooltip("The effect to create when activating the checkpoint")]
    public GameObject checkpointActivationEffect;

    /// <summary>
    /// Description:
    /// Standard unity function called when a trigger is entered by another collider
    /// Input:
    /// Collider collision
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="collision">The collider that has entered the trigger</param>
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player" && collision.gameObject.GetComponent<HealthSystem>() != null)
        {
            HealthSystem playerHealth = collision.gameObject.GetComponent<HealthSystem>();
            respawnLocation = playerHealth.transform;
            playerHealth.SetRespawnPoint(respawnLocation.position, respawnLocation.rotation);
            SaveSystem.SaveGame();

            // Reset the last checkpoint if it exists
            if (CheckpointleSystem.currentCheckpoint != null)
            {
                if (CheckpointleSystem.currentCheckpoint.checkpointAnimator != null)
                {
                        CheckpointleSystem.currentCheckpoint.checkpointAnimator.SetBool(animatorActiveParameter, false);
                }
            }

            if (CheckpointleSystem.currentCheckpoint != this && checkpointActivationEffect != null)
            {
                Instantiate(checkpointActivationEffect, transform.position, Quaternion.identity, null);
            }

            // Set current checkpoint to this and set up its animation
            CheckpointleSystem.currentCheckpoint = this;
            if (checkpointAnimator != null)
            {
                checkpointAnimator.SetBool(animatorActiveParameter, true);
            }
        }
    }
}
}

