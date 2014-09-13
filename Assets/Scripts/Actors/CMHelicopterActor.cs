using UnityEngine;
using System.Collections.Generic;

public class CMHelicopterActor : CMBehavior
{
	#region State classes

	abstract class HelicopterState
	{
		protected readonly CMHelicopterActor HelicopterActor;

		public HelicopterState(CMHelicopterActor _HelicopterActor)
		{
			this.HelicopterActor = _HelicopterActor;
		}

		public virtual void OnStart() {}
		public virtual void OnFinish() {}
		public virtual void OnUpdate() {}
	}

	class FlybyState : HelicopterState
	{
		public FlybyState(CMHelicopterActor _HelicopterActor)
			: base(_HelicopterActor)
		{ }

		public override void OnStart()
		{
		}

		public override void OnFinish()
		{
		}

		public override void OnUpdate()
		{
		}
	}

	class CampMoveState : HelicopterState
	{
		public CampMoveState(CMHelicopterActor _HelicopterActor)
			: base(_HelicopterActor)
		{ }

		public override void OnUpdate()
		{
			HelicopterActor.Target = HelicopterActor.HelicopterManager.NextCampPoint();
			if (HelicopterActor.IsTargetReached())
			{
				HelicopterActor.State = new CampWaitState(HelicopterActor);
			}
		}
	}

	class CampWaitState : HelicopterState
	{
		public CampWaitState(CMHelicopterActor _HelicopterActor)
			: base(_HelicopterActor)
		{ }

		public override void OnUpdate()
		{
			foreach(var c in HelicopterActor.CommandoManager.Commandos)
			{
				if ((c.transform.position - HelicopterActor.transform.position).magnitude < HelicopterActor.FireDistance)
				{
					HelicopterActor.State = new CampFireState(HelicopterActor);
					break;
				}
			}
		}
	}

	class CampFireState : HelicopterState
	{
		float m_Started;

		public CampFireState(CMHelicopterActor _HelicopterActor)
			: base (_HelicopterActor)
		{ }

		public override void OnStart()
		{
			m_Started = Time.time;
		}

		public override void OnUpdate()
		{
			if (m_Started + HelicopterActor.FireTime < Time.time)
			{
				HelicopterActor.State = new CampMoveState(HelicopterActor);
			}
		}
	}

	#endregion

	#region Current helicopter state

	HelicopterState m_State;
	HelicopterState State
	{
		get { return m_State; }
		set
		{
			if (m_State != null)
			{
				m_State.OnFinish();
			}
			m_State = value;
			if (value != null)
			{
				value.OnStart();
			}
		}
	}

	#endregion

	#region Movement

	public float Speed = 1f;
	public float SnapDistance = 0.1f;

	Vector3 Target { get; set; }
	bool IsTargetReached()
	{
		return (Target - transform.position).magnitude < SnapDistance;
	}

	#endregion

	#region Firing

	public float FireDistance	= 2f;
	public float FireTime		= 3f;

	#endregion

	#region Engine methods

	void Start()
	{
		State = new CampMoveState(this);
	}

	void Update()
	{
		if (State != null)
		{
			State.OnUpdate();
		}
		if (IsTargetReached())
		{
			transform.position = Target;
		}
		else
		{
			var movement = (Target - transform.position).normalized * Speed * Time.deltaTime;
			transform.position = transform.position + movement;
		}
	}

	#endregion

	#region Gizmos

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, Target);
	}

	#endregion

}
