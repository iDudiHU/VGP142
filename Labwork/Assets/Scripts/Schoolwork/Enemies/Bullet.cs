using Schoolwork.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that handles the bullet
namespace Schoolwork.Enemies
{
	[RequireComponent(typeof(Rigidbody))]
	public class Bullet : MonoBehaviour
	{
		[Tooltip("Rigidbody for the bullet")]
		[SerializeField] private Rigidbody _rigidbody;
		[Tooltip("Bullet damage")]
		[SerializeField] private float _damage;
		[Tooltip("Bullet speed")]
		[SerializeField] private float _speed;
		[Tooltip("Bullet lifetime to be destroyed ")]
		[SerializeField] private float _lifeTime;

		private BulletPool _pool;

		private void Start()
		{
			//basic error check
			if (_rigidbody == null)
			{
				Debug.LogError("Add bullet's rigidbody reference!", transform);
				Debug.Break();
			}
		}

		public void SetBulletPool(BulletPool pool)
		{
			_pool = pool;
		}

		public void Shoot(Transform startPos)
		{
			//use invoke to make sure the bullet resets on lifetime time if it not collided with anything
			Invoke(nameof(ResetBullet), _lifeTime);
			gameObject.SetActive(true);
			transform.position = startPos.position;
			_rigidbody.isKinematic = false;
			transform.forward = startPos.transform.forward.normalized;
			_rigidbody.AddForce(transform.forward * _speed, ForceMode.VelocityChange);
			Invoke(nameof(ResetBullet), _lifeTime);
		}

		private void OnTriggerEnter(Collider other)
		{
			//Accesing the IDamageableinterface on collsion if it is a damageable object otherwise reset bullet
			CancelInvoke(nameof(ResetBullet));

			IDamageable damageable = other.GetComponentInParent<IDamageable>();
			if (damageable != null)
			{
				damageable.OnDamage(_damage);
			}

			ResetBullet();
		}
		//put bullet back to the pool reseted out
		public void ResetBullet()
		{
			_rigidbody.velocity = _rigidbody.angularVelocity = Vector3.zero;
			_rigidbody.isKinematic = true;
			gameObject.SetActive(false);
			_pool.AddbulletToPool(this);
		}
	}
}
