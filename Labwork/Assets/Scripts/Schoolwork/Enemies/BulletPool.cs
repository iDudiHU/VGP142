using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Classs that handles the bullets in its pool
namespace Schoolwork.Enemies
{
	public class BulletPool : MonoBehaviour
	{
		[Tooltip("Bullet prefab to be spawned in this bullets pool")]
		[SerializeField] private GameObject _bulletPrefab;
		[Tooltip("Create a base pool size")]
		[SerializeField] private int _startCount = 20;
		[Tooltip("Maximum bullet of bullets")]
		[SerializeField] private int _maxCount = 50;
		//Queue used for a first in last out system to make sure to never use the same bullet pool object
		private Queue<Bullet> _pool = new Queue<Bullet>();
		private int _createdBulletCount;

		private void Start()
		{   //create bullets for the bullet pool
			for (int i = 0; i < _startCount; i++) CreateBullet();
		}

		private void CreateBullet()
		{
			if (_createdBulletCount == _maxCount) return;
			_createdBulletCount++;
			GameObject newBullet = Instantiate(_bulletPrefab, transform);
			Bullet bulletSript = newBullet.GetComponent<Bullet>();
			bulletSript.SetBulletPool(this);
			bulletSript.ResetBullet();
		}


		public void AddbulletToPool(Bullet bullet)
		{
			_pool.Enqueue(bullet);
		}

		//Out puts a bullet 
		public Bullet GetBulletFromPool()
		{
			if (_pool.Count == 0) CreateBullet();

			if (_pool.Count == 0) return null;

			return _pool.Dequeue();
		}
	}
}
