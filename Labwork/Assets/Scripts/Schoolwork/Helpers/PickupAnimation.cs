using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Schoolwork.Helpers
{
    public class PickupAnimation : MonoBehaviour
    {
        [Tooltip("The height difference between the resting position of the object and it's maximum or minimum height.")]
        public float oscillationHeight = 0.5f;
        [Tooltip("The speed at which the object oscilates up and down.")]
        public float oscillationSpeed = 2.0f;
        [Tooltip("The speed at which the object rotates per second (in degrees)")]
        public float rotationSpeed = 90.0f;
        // The starting position of the object.
        private Vector3 startPosition;
        private Vector3 originalPosition;
        [SerializeField] private bool isMovable;

        /// <summary>
        /// Description:
        /// When this script starts up, save the starting position of the object
        /// Inputs: N/A
        /// Outputs: N/A
        /// </summary>
        private void Start()
        {
            startPosition = originalPosition = transform.localPosition;
        }

        /// <summary>
        /// Description:
        /// Every update, rotate and move the object according to the values set for this script
        /// Inputs: N/A
        /// Outputs: N/A
        /// </summary>
        private void LateUpdate()
        {
            if (isMovable)
            {
                startPosition = new Vector3(transform.localPosition.x, originalPosition.y, transform.localPosition.z);
            }
            // Calculate the vertical offset using the oscillation pattern
            float yOffset = oscillationHeight * Mathf.Cos(Time.timeSinceLevelLoad * oscillationSpeed);
            // Update the position using the vertical offset
            transform.localPosition = startPosition + Vector3.up * yOffset;
            // Update the rotation based on the rotation speed
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.up * Time.deltaTime * rotationSpeed);
        }
    }
}
