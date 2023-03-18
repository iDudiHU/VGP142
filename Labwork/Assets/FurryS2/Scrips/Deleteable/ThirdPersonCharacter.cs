using System;
using Schoolwork.Helpers;
using Schoolwork.Systems;
using Schoolwork;
using Unity.VisualScripting;
using UnityEngine;
using static Schoolwork.Systems.WeaponSystem;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class ThirdPersonCharacter : MonoBehaviour, IDamageable
	{
		[SerializeField] float m_MovingTurnSpeed = 360;
		[SerializeField] float m_StationaryTurnSpeed = 180;
		[SerializeField] float m_JumpPower = 6f;
		[Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
		[SerializeField] float m_RunCycleLegOffset = 0.2f;
		[SerializeField] float m_MoveSpeedMultiplier = 1f;
		float m_OrigMoveSpeedMultiplier = 1f;
		[SerializeField] float m_SprintSpeedMultiplier = 1.5f;
		[SerializeField] float m_AnimSpeedMultiplier = 1f;
		[SerializeField] float m_AttackAnimSpeedMultiplier = 1f;
		[SerializeField] float m_GroundCheckDistance = 0.1f;
		[SerializeField]  GameObject m_ShootPosition;
		[SerializeField]  GameObject m_ProjectilePrefab;
		[SerializeField] private GameObject m_AOE_Go;
		[SerializeField] private GameObject m_Punch_Go;
		[SerializeField] private LayerMask m_crouchLayermask;
		public event EventHandler OnPlayerDeath;

		[SerializeField] GameObject m_Bow;
		[SerializeField] GameObject m_Staff;
		Rigidbody m_Rigidbody;
		Animator m_Animator;
		HealthSystem m_HealthSystem;
		bool m_IsGrounded;
		float m_OrigGroundCheckDistance;
		const float k_Half = 0.5f;
		float m_TurnAmount;
		float m_ForwardAmount;
		Vector3 m_GroundNormal;
		float m_CapsuleHeight;
		Vector3 m_CapsuleCenter;
		CapsuleCollider m_Capsule;
		bool m_Crouching;
		private bool m_IsHandEmpty = true;
		private bool isAlive = true;
		private bool isAttacking = false;

		public bool IsAlive => isAlive;

		private static readonly int NormalAttack = Animator.StringToHash("NormalAttack");
		private void OnEnable()
		{
			SetupGameManagerPlayer();
		}
		
		private void SetupGameManagerPlayer()
		{
			if (GameManager.Instance != null && GameManager.Instance.player == null)
			{
				GameManager.Instance.player = this.transform.gameObject;
			}
		}


		void Start()
		{
			m_Animator = GetComponent<Animator>();
			m_Rigidbody = GetComponent<Rigidbody>();
			m_Capsule = GetComponent<CapsuleCollider>();
			m_HealthSystem = GetComponent<HealthSystem>();
			m_CapsuleHeight = m_Capsule.height;
			m_CapsuleCenter = m_Capsule.center;
			m_OrigMoveSpeedMultiplier = m_MoveSpeedMultiplier;

			m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			m_OrigGroundCheckDistance = m_GroundCheckDistance;
			m_AOE_Go.SetActive(false);
			m_Punch_Go.SetActive(false);
		}
		
		

		private void Update()
		{
			if (isAlive)
			{
				if (Input.GetKey(KeyCode.LeftShift))
					m_MoveSpeedMultiplier = m_SprintSpeedMultiplier;
				if (Input.GetKeyUp(KeyCode.LeftShift))
					m_MoveSpeedMultiplier = m_OrigMoveSpeedMultiplier;
				if (Input.GetMouseButton(0) && !isAttacking && (GameManager.Instance.weaponSystem.currentWeapon == WeaponTypes.Bow
					|| GameManager.Instance.weaponSystem.currentWeapon == WeaponTypes.Combined))
				{
					m_Animator.SetTrigger(NormalAttack);
					isAttacking = true;
				}
				else if (Input.GetMouseButton(1) && !isAttacking && (GameManager.Instance.weaponSystem.currentWeapon == WeaponTypes.Staff
				  || GameManager.Instance.weaponSystem.currentWeapon == WeaponTypes.Combined))
				{
					m_Animator.SetTrigger("SpecialAttack");
					isAttacking = true;
				}
				else if (Input.GetKey(KeyCode.E) && !isAttacking)
				{
					m_Animator.SetTrigger("Punch");
					isAttacking = true;
				}
			}
		}

		public void Punch()
		{
			m_Punch_Go.SetActive(true);
		}

		public void EndPunch()
		{
			m_Punch_Go.SetActive(false);
			isAttacking = false;
		}

		public void SpecialAttack()
		{
			m_AOE_Go.SetActive(true);
		}

		public void EndSpecialAttack()
		{
			m_AOE_Go.SetActive(false);
			isAttacking = false;
		}

		private void OnTriggerEnter(Collider other)
		{
		}

		private void OnCollisionEnter(Collision collision)
		{
			IPickupable pickupable = collision.gameObject.GetComponent<IPickupable>();
			if (pickupable != null) {
				pickupable.DoOnPickup();
			}
		}

		public void ItemInRangeCollision(Collider other)
		{
			IPickupable pickupable = other.gameObject.GetComponent<IPickupable>();
			if(pickupable != null){
				other.gameObject.GetComponent<IPickupable>().DoInRange();
			}
		}

		public void PickUpItem(Collider other)
		{
			IPickupable pickupable = other.gameObject.GetComponent<IPickupable>();
			if (pickupable != null) {
				pickupable.DoOnPickup();
				GameManager.UpdateUIElements();
			}
		}

		public void StartShoot()
		{
			if (!m_IsHandEmpty) {
				Instantiate(m_ProjectilePrefab, m_ShootPosition.transform.position, m_ShootPosition.transform.rotation);
			}
		}
		public void EndShoot()
		{
			isAttacking = false;
		}


		public void Move(Vector3 move, bool crouch, bool jump)
		{

			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			CheckGroundStatus();
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;

			ApplyExtraTurnRotation();

			// control and velocity handling is different when grounded and airborne:
			if (m_IsGrounded)
			{
				HandleGroundedMovement(crouch, jump);
			}
			else
			{
				HandleAirborneMovement();
			}

			ScaleCapsuleForCrouching(crouch);
			PreventStandingInLowHeadroom();

			// send input and other state parameters to the animator
			UpdateAnimator(move);
		}


		void ScaleCapsuleForCrouching(bool crouch)
		{
			if (m_IsGrounded && crouch)
			{
				if (m_Crouching) return;
				m_Capsule.height = m_Capsule.height / 2f;
				m_Capsule.center = m_Capsule.center / 2f;
				m_Crouching = true;
			}
			else
			{
				Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * (m_Capsule.radius * k_Half), Vector3.up);
				float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
				if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, m_crouchLayermask, QueryTriggerInteraction.Ignore))
				{
					m_Crouching = true;
					return;
				}
				m_Capsule.height = m_CapsuleHeight;
				m_Capsule.center = m_CapsuleCenter;
				m_Crouching = false;
			}
		}

		void PreventStandingInLowHeadroom()
		{
			// prevent standing up in crouch-only zones
			if (!m_Crouching)
			{
				Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * (m_Capsule.radius * k_Half), Vector3.up);
				float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
				if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, m_crouchLayermask, QueryTriggerInteraction.Ignore))
				{
					m_Crouching = true;
				}
			}
		}


		void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
			m_Animator.SetBool("Crouch", m_Crouching);
			m_Animator.SetBool("OnGround", m_IsGrounded);
			if (!m_IsGrounded)
			{
				m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
			}

			// calculate which leg is behind, so as to leave that leg trailing in the jump animation
			// (This code is reliant on the specific run cycle offset in our animations,
			// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
			float runCycle =
				Mathf.Repeat(
					m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
			float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
			if (m_IsGrounded)
			{
				m_Animator.SetFloat("JumpLeg", jumpLeg);
			}

			// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
			// which affects the movement speed because of the root motion.
			if (m_IsGrounded && move.magnitude > 0)
			{
				m_Animator.speed = m_AnimSpeedMultiplier;
			}
			else
			{
				// don't use that while airborne
				m_Animator.speed = 1;
			}
		}


		void HandleAirborneMovement()
		{
			// apply extra gravity from multiplier:
			Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
			m_Rigidbody.AddForce(extraGravityForce);

			m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
		}


		void HandleGroundedMovement(bool crouch, bool jump)
		{
			// check whether conditions are right to allow a jump:
			if (jump && !crouch && (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded With Bow")))
			{
				// jump!
				m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
				m_IsGrounded = false;
				m_Animator.applyRootMotion = false;
				m_GroundCheckDistance = 0.1f;
			}
		}

		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		}


		public void OnAnimatorMove()
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (m_IsGrounded && Time.deltaTime > 0)
			{

				//move forward with motionZ//
				Vector3 moveForward = transform.forward*m_Animator.GetFloat("motionZ")*Time.deltaTime;
				Vector3 v = ((m_Animator.deltaPosition + moveForward) * m_MoveSpeedMultiplier) / Time.deltaTime;

				// we preserve the existing y part of the current velocity.
				v.y = m_Rigidbody.velocity.y;
				m_Rigidbody.velocity = v;
			}
		}

		public void PickupBow(bool useAnim = true)
		{
			UnequipAllWeapons();
			if (useAnim)
			{
				m_Animator.Play("Pickup");
			}
			m_Animator.SetBool("HasBow", true);
			m_Bow.SetActive(true);
			m_IsHandEmpty = false;
		}

		public void PickupUpStaff(bool useAnim = true)
		{
			UnequipAllWeapons();
			if (useAnim)
			{
				m_Animator.Play("Pickup");
			}
			m_Animator.SetBool("HasStaff", true);
			m_Staff.SetActive(true);
			m_IsHandEmpty = false;
		}

		public void UnequipAllWeapons()
		{
			m_Staff.SetActive(false);
			m_Animator.SetBool("HasStaff", false);
			m_Bow.SetActive(false);
			m_Animator.SetBool("HasBow", false);
			m_IsHandEmpty = true;
		}

		public void EquipAllWeapons(bool useAnim = true)
		{
			UnequipAllWeapons();
			if (useAnim)
			{
				m_Animator.Play("Pickup");
			}
			m_Animator.SetBool("HasBow", true);
			m_Animator.SetBool("HasStaff", true);
			m_Bow.SetActive(true);
			m_Staff.SetActive(true);
			m_IsHandEmpty = false;
		}

		public void Die()
		{
				m_Animator.SetTrigger("Death");
				//GetComponent<Rigidbody>().isKinematic = true;
				isAlive = false;
				if (OnPlayerDeath != null) OnPlayerDeath(this, EventArgs.Empty);
				//Destroy(transform.parent.gameObject, 3);
		}
		public void Respawn()
		{
			m_Animator.Rebind();
			//GetComponent<Rigidbody>().isKinematic = true;
			isAlive = true;
			if (OnPlayerDeath != null) OnPlayerDeath(this, EventArgs.Empty);
			//Destroy(transform.parent.gameObject, 3);
		}
		public void IncreaseAttackAnimationSpeed()
		{
			GameManager.Instance.levelSystem.SpendAttribute();
			m_AttackAnimSpeedMultiplier += .3f;
			m_Animator.SetFloat("AttackSpeed", m_AttackAnimSpeedMultiplier);
		}
		public void IncreaseWalkSpeed()
		{
			GameManager.Instance.levelSystem.SpendAttribute();
			m_MoveSpeedMultiplier += 0.1f;
			m_OrigMoveSpeedMultiplier = m_MoveSpeedMultiplier;
			m_SprintSpeedMultiplier += 0.3f;
		}
		public void IncreaseJumpHeight()
		{
			GameManager.Instance.levelSystem.SpendAttribute();
			m_JumpPower += 3f;
		}
		void CheckGroundStatus()
		{
			RaycastHit hitInfo;
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
			{
				m_GroundNormal = hitInfo.normal;
				m_IsGrounded = true;
				m_Animator.applyRootMotion = true;
			}
			else
			{
				m_IsGrounded = false;
				m_GroundNormal = Vector3.up;
				m_Animator.applyRootMotion = false;
			}
		}

		public void OnDamage(float damageAmount)
		{
			GameManager.Instance.healthSystem.TakeDamage(damageAmount);
		}
		public void Load(GameData data)
		{
			GameData.PlayerData playerData = data.player;
			//Set position
			transform.position = playerData.transformData.position;
			//Set rotation
			transform.localRotation = playerData.transformData.rotation;
			//Set scale
			transform.localScale = playerData.transformData.scale;
			//Set health
			GetComponent<HealthSystem>().Load(playerData.healthData);
			//Set level
			GetComponent<LevelSystem>().Load(playerData.levelData);
			//Set weapon
			GameManager.Instance.weaponSystem.Load(ref playerData);
			//KeyRing.Load(ref data);
			GameManager.UpdateUIElements();
			//Load leveled stats
			m_MoveSpeedMultiplier = data.player.leveledStats.walkSpeed;
			m_SprintSpeedMultiplier = data.player.leveledStats.printSpeed;
			m_AnimSpeedMultiplier = data.player.leveledStats.attackSpeed;
			m_Animator.SetFloat("AttackSpeed", m_AttackAnimSpeedMultiplier);
			m_JumpPower = data.player.leveledStats.jumpHeight;

		}
		public void Save(ref GameData data)
		{
			//TransformData
			data.player.transformData.position = transform.position;
			data.player.transformData.rotation = transform.rotation;
			data.player.transformData.scale = transform.localScale;
			//HealthData
			GetComponent<HealthSystem>().Save(ref data);
			//playerData.healthData._currentHealth = GetComponent<HealthSystem>().currentHealth;
			//playerData.healthData._maxHealth = GetComponent<HealthSystem>().maximumHealth;
			//LevelData
			GetComponent<LevelSystem>().Save(ref data);
			GameManager.Instance.weaponSystem.Save(ref data);
			//KeyRing.Save(ref data);
			//Save Leveled data;
			data.player.leveledStats.walkSpeed = m_MoveSpeedMultiplier;
			data.player.leveledStats.printSpeed = m_SprintSpeedMultiplier;
			data.player.leveledStats.attackSpeed = m_AnimSpeedMultiplier;
			data.player.leveledStats.jumpHeight = m_JumpPower;
		}
	}
}
