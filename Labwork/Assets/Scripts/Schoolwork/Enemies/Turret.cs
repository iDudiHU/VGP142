using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Schoolwork.Helpers;
using UnityStandardAssets.Characters.ThirdPerson;

//Class that handles the turret
namespace Schoolwork.Enemies
{
    public class Turret : MonoBehaviour, IDamageable
    {
        private Transform target;
        [Tooltip("Turret ange")]
        [SerializeField] private float _range = 15f;
        [Tooltip("Yaw rotation speed")]
        [SerializeField] private float _speedYaw = 60.0f;
        [Tooltip("Pitch rotation speed")]
        [SerializeField] private float _speedPitch = 5.0f;
        [SerializeField] private float _speedBarrel = 3.0f;
        [Tooltip("Angle to be able to shoot (Dot product)")]
        [SerializeField] private float _shootAngle = 30.0f;
        [Tooltip("Turret helper gameobject to point at the target")]
        [SerializeField] private Transform _turretHelper;
        [SerializeField] private Transform _leftBarrelHelper;
        [SerializeField] private Transform _rightBarrelHelper;
        [Tooltip("Weapon script in the prefab")]
        [SerializeField] private TurretWeaponController _turretWeapon;
        [Tooltip("Target aim gameobjects name")]
        [SerializeField] private string _turretTargetString;
        [SerializeField] private float _health;
        [SerializeField] private LayerMask LM;

        public Transform yawBase;
        public Transform pitchBase;
        public Transform rightBarrelBase;
        public Transform leftBarrelBase;

        [Tooltip("Turret Aim limits")]
        [SerializeField] private float _maxPitchAngle = 30;
        [SerializeField] private float _maxminAngle = -30;



        void Awake()
        {
        }

        void Start()
		{
            target = GameManager.Instance.player.transform;
        }

        void Update()
        {
            if (target == null) return;
            if (!TargetInRange())
			{
                yawBase.localRotation = Quaternion.RotateTowards(yawBase.localRotation, Quaternion.Euler(0f, 0f, 0f), _speedYaw * Time.deltaTime);
                pitchBase.localRotation = Quaternion.RotateTowards(pitchBase.localRotation, Quaternion.Euler(0, 0, 0), _speedPitch * Time.deltaTime);
                rightBarrelBase.localRotation = Quaternion.RotateTowards(rightBarrelBase.localRotation, Quaternion.Euler(0,0, 0), _speedBarrel * Time.deltaTime);
                leftBarrelBase.localRotation = Quaternion.RotateTowards(leftBarrelBase.localRotation, Quaternion.Euler(0, 0, 0), _speedBarrel * Time.deltaTime);
                return;
            }
            //Helps with the dual axis rotation of the turret
            _turretHelper.LookAt(target);
            _rightBarrelHelper.LookAt(target);
            _leftBarrelHelper.LookAt(target);

            Vector3 baseDir = target.position - transform.position;
            Vector3 pitchDir = target.position - pitchBase.position;
            Vector3 rightBarrelDir = target.position + Vector3.up - rightBarrelBase.position;
            Vector3 leftBarrelDir = target.position + Vector3.up - leftBarrelBase.position;

            Quaternion baseLookRotation = Quaternion.LookRotation(baseDir);
            Quaternion pitchLookRotation = Quaternion.LookRotation(pitchDir);
            Quaternion rightLookRotation = Quaternion.LookRotation(rightBarrelDir);
            Quaternion leftLookRotation = Quaternion.LookRotation(leftBarrelDir);


            Vector3 yawRotation = baseLookRotation.eulerAngles;
            Vector3 pitchRotation = pitchLookRotation.eulerAngles;
            Vector3 rightRotation = rightLookRotation.eulerAngles;
            Vector3 leftRotation = leftLookRotation.eulerAngles;

            yawBase.rotation = Quaternion.RotateTowards(yawBase.rotation, Quaternion.Euler(0f, yawRotation.y, 0f), _speedYaw * Time.deltaTime);
            pitchBase.localRotation = Quaternion.RotateTowards(pitchBase.localRotation, Quaternion.Euler(pitchRotation.x, 0, 0), _speedPitch * Time.deltaTime);

            rightBarrelBase.localRotation = Quaternion.RotateTowards(rightBarrelBase.localRotation, Quaternion.Euler(rightRotation.x - pitchRotation.x, rightRotation.y - yawRotation.y, 0), _speedBarrel * Time.deltaTime);
            leftBarrelBase.localRotation = Quaternion.RotateTowards(leftBarrelBase.localRotation, Quaternion.Euler(leftRotation.x - pitchRotation.x, leftRotation.y - yawRotation.y, 0), _speedBarrel * Time.deltaTime);
            if (TargetVisible())
            {
                _turretWeapon.Fire();
            }
        }

        void UpdateTarget()
        {
            GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
            float shortestDistance = Mathf.Infinity;
            GameObject nearestUnit = null;

            foreach (GameObject unit in units)
            {
                float distanceToUnit = Vector3.Distance(transform.position, unit.transform.position);
                if (distanceToUnit < shortestDistance)
                {
                    shortestDistance = distanceToUnit;
                    nearestUnit = unit;
                }
            }

            if (nearestUnit != null && shortestDistance <= _range)
            {
                target = nearestUnit.gameObject.transform.Find(_turretTargetString).transform;
            }
            else
            {
                target = null;
            }
        }

        public bool TargetInRange()
		{
            Vector3 dirToTarget = Vector3.Normalize(target.position - pitchBase.position);
            RaycastHit hitInfo;
            if (Physics.Raycast(pitchBase.position, dirToTarget, out hitInfo, _range, LM))
            {
                if (hitInfo.transform == target)
                {
                    // The target is blocked by something else
                    Debug.DrawRay(pitchBase.position, dirToTarget * hitInfo.distance, Color.green);
                    return true;
                }
            }
            Debug.DrawRay(pitchBase.position, dirToTarget * _range, Color.red);
            return false;
        }
        public bool TargetVisible()
        {
            Vector3 dirToTarget = Vector3.Normalize(target.position - pitchBase.position);
            float dot = Vector3.Dot(pitchBase.forward, dirToTarget);

            if (dot < Mathf.Cos(_shootAngle * Mathf.Deg2Rad))
            {
                // Target is outside of the maximum angle of visibility
                return false;
            }
            return true;           
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _range);
        }

		public void OnDamage(float damageAmount)
		{
			_health -= damageAmount;
            if (_health < 0)
			{
                Destroy(this);
			}
		}
	}
}

