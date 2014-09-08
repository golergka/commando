using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CMLocomotion : CMBehavior
{
	#region Parameters

	public float JumpForce		= 5f;
	public float Gravity		= 9.8f;
	public float Speed			= 0f;
	public float GroundSnap		= 0.1f;
	public float GroundAdjust	= 1f;

	#endregion

	#region Movement state

	struct MovementState
	{
		public float	GroundHeight;
		public float	VerticalImpulse;
		public bool		DoubleJumped;
	}

	MovementState State = new MovementState();

	public void SwitchWith(CMLocomotion _OtherMover)
	{
		var temp = _OtherMover.State;
		_OtherMover.State = this.State;
		this.State = temp;
	}

	#endregion

	#region Service methods

	void HitGround()
	{
		State.VerticalImpulse = 0f;
		State.DoubleJumped = false;
	}

	#endregion

	#region Movement interface

	public float Direction { get; set; }

	public void Jump()
	{
		bool grounded = Mathf.Approximately(State.VerticalImpulse, 0f);
		if (grounded || !State.DoubleJumped)
		{
			State.VerticalImpulse = JumpForce;
			if (!grounded)
			{
				State.DoubleJumped = true;
			}
		}
	}

	public void DeathJump()
	{
		State.VerticalImpulse = JumpForce;
		State.DoubleJumped = true;
		State.GroundHeight = float.NegativeInfinity;
	}

	#endregion

	#region Grounded

	bool m_GroundedPrevious;
	bool Grounded
	{
		get
		{
			return State.VerticalImpulse == 0f;
		}
	}

	#endregion

	#region Events

	public event System.Action<Vector3> OnMovement;
	public event System.Action<bool> OnGroundedChanged;

	#endregion

	#region Engine methods

	protected override void Awake()
	{
		base.Awake();
		State.GroundHeight = transform.position.y;
	}

	void Update()
	{
		State.VerticalImpulse -= Gravity * Time.deltaTime;
		var movement = new Vector3(
				Direction * Speed * Time.deltaTime,
				State.VerticalImpulse * Time.deltaTime,
				0
			);
		// Clamping vertical movement with vertical cast
		if (movement.y < 0)
		{
			// Hitting obstacles and platforms
			RaycastHit hit;
			if (rigidbody.SweepTest(new Vector3(0, -movement.y, 0), out hit, movement.y))
			{
				movement.y = 0f;
				HitGround();
			}
			// Hitting the ground
			if (transform.position.y <= State.GroundHeight)
			{
				if ((State.GroundHeight - transform.position.y) < GroundSnap)
				{
					transform.position = new Vector3(
							transform.position.x,
							State.GroundHeight,
							transform.position.z
						);
				}
				movement.y = (State.GroundHeight - transform.position.y) * Time.deltaTime * GroundAdjust;
				HitGround();
			}
		}
		if (m_GroundedPrevious != Grounded && OnGroundedChanged != null)
		{
			OnGroundedChanged(Grounded);
			m_GroundedPrevious = Grounded;
		}
		// Moving the actor
		transform.position = transform.position + movement;
		// Changing the sprite
		if (OnMovement != null)
		{
			OnMovement(movement);
		}
	}

	#endregion

}
