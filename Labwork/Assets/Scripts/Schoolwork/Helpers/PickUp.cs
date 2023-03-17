using Schoolwork.Systems;
using System;
using System.Linq;
using UnityEngine;

namespace Schoolwork.Helpers
{
    public class PickUp : MonoBehaviour, IPickupable
    {
        public static event Action<PickUp> PickupCreated;
        public static event Action<PickUp> PickupDestroyed;

        [SerializeField] private string id = string.Empty;

        public string Id => id;

        [ContextMenu("Generate id")]
        public void GenerateId()
        {
            if (String.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();
            }
        }
        [Header("Settings")]
        [Tooltip("The effect to create when this pickup is collected")]
        public GameObject pickUpEffect;

		public void Awake()
		{
            PickupCreated?.Invoke(this);
        }
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

        public virtual void Save(ref GameData data)
		{
            GameData.CollectibleData collectibleData = new GameData.CollectibleData();
            //Collectible Data
            collectibleData.Id = Id;
            //TransformData
            collectibleData.transformData.posX = transform.position.x;
            collectibleData.transformData.posY = transform.position.y;
            collectibleData.transformData.posZ = transform.position.z;

            collectibleData.transformData.rotX = transform.rotation.x;
            collectibleData.transformData.rotY = transform.rotation.y;
            collectibleData.transformData.rotZ = transform.rotation.z;

            collectibleData.transformData.scaleX = transform.localScale.x;
            collectibleData.transformData.scaleY = transform.localScale.y;
            collectibleData.transformData.scaleZ = transform.localScale.z;

            data.collectibleDataList.Add(collectibleData);
        }

        public virtual void Load(GameData data)
		{
            //instead this if shoudl check if any of the data.collectibleDataList elemt's Id is this gameobject's Id
            if (!data.collectibleDataList.Any(collectibleData => collectibleData.Id == this.Id))
            {
                Destroy(gameObject);
                return;
            }
            // Else load enemy data
            GameData.CollectibleData matchingCollectible = data.collectibleDataList.FirstOrDefault(c => c.Id == this.Id);
            //Set position
            transform.position = new Vector3(matchingCollectible.transformData.posX, matchingCollectible.transformData.posY, matchingCollectible.transformData.posZ);
            //Set rotation
            transform.localRotation = Quaternion.Euler(matchingCollectible.transformData.rotX, matchingCollectible.transformData.rotY, matchingCollectible.transformData.rotZ);
            //Set scale
            transform.localScale = new Vector3(matchingCollectible.transformData.scaleX, matchingCollectible.transformData.scaleY, matchingCollectible.transformData.scaleZ);
        }

		public void OnDestroy()
		{
            PickupDestroyed?.Invoke(this);
        }
	}
}