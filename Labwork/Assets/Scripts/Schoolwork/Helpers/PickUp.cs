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

		public virtual void Awake()
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
            collectibleData.transformData.position = transform.position;
            collectibleData.transformData.rotation = transform.rotation;
            collectibleData.transformData.scale = transform.localScale;

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
            // Else load pickup data
            GameData.CollectibleData matchingCollectible = data.collectibleDataList.FirstOrDefault(c => c.Id == this.Id);
            //Set position
            transform.position = matchingCollectible.transformData.position;
            //Set rotation
            transform.localRotation = matchingCollectible.transformData.rotation;
            //Set scale
            transform.localScale = matchingCollectible.transformData.scale;
        }

		public void OnDestroy()
		{
            PickupDestroyed?.Invoke(this);
        }
	}
}