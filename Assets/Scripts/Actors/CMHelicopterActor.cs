using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CMHelicopterActor : CMBehavior
{
	#region Abstract helicopter state

	abstract class HelicopterState
	{
		protected readonly CMHelicopterActor Heli;

		public HelicopterState(CMHelicopterActor _Heli)
		{
			this.Heli = _Heli;
		}

		public virtual void OnStart() {}
		public virtual void OnFinish() {}
		public virtual void OnUpdate() {}
		public virtual void OnDrawGizmosSelected() {}
	}

	#endregion

	#region IntentState classes

	abstract class HelicopterIntentState : HelicopterState
	{
		public HelicopterIntentState(CMHelicopterActor _Heli)
			: base (_Heli)
		{ }
	}

	class CampMoveState : HelicopterIntentState
	{
		public CampMoveState(CMHelicopterActor _Heli)
			: base(_Heli)
		{ }

		public override void OnUpdate()
		{
			var nextPoint = Heli.HelicopterManager.NextCampPoint();
			if (nextPoint == null)
			{
				Heli.IntentState = new SilentFollowState(Heli);
			}
			else
			{
				Heli.Target = nextPoint.transform.position;
				if (Heli.IsTargetReached())
				{
					Heli.IntentState = new CampWaitState(Heli);
					Heli.HelicopterManager.CampPoints.Remove(nextPoint);
				}
			}
		}

		public override void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(Heli.transform.position, Heli.Target);
		}
	}

	class CampWaitState : HelicopterIntentState
	{
		public CampWaitState(CMHelicopterActor _Heli)
			: base(_Heli)
		{ }

		public override void OnUpdate()
		{
			Heli.OnEnemySpotted(delegate
			{
				Heli.IntentState = new CampFireState(Heli);
			});
		}

		public override void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			var boxCollider = Heli.GetComponent<BoxCollider>();
			Gizmos.DrawWireCube(
					Heli.transform.TransformPoint(boxCollider.center),
					Heli.transform.TransformVector(boxCollider.size)
				);
		}
	}

	class CampFireState : HelicopterIntentState
	{
		float	m_Started;
		bool	m_Finished = false;

		public CampFireState(CMHelicopterActor _Heli)
			: base (_Heli)
		{ }

		public override void OnStart()
		{
			m_Started = Time.time;
			Heli.StartFire();
			Heli.OnEnemyLeft(delegate
			{
				if (!m_Finished)
				{
					Heli.IntentState = new CampMoveState(Heli);
				}
			});
		}

		public override void OnFinish()
		{
			Heli.StopFire();
			m_Finished = true;
		}

		public override void OnUpdate()
		{
			if (m_Started + Heli.FireTime < Time.time)
			{
				Heli.IntentState = new CampMoveState(Heli);
			}
		}

		public override void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			var boxCollider = Heli.GetComponent<BoxCollider>();
			Gizmos.DrawWireCube(
					Heli.transform.TransformPoint(boxCollider.center),
					Heli.transform.TransformVector(boxCollider.size)
				);
		}
	}

	class SilentFollowState : HelicopterIntentState
	{
		public SilentFollowState(CMHelicopterActor _Heli)
			: base(_Heli)
		{ }
	}

	#endregion

	#region Helicopter movement state

	abstract class HelicopterMovementState : HelicopterState
	{
		public HelicopterMovementState(CMHelicopterActor _Heli)
			: base(_Heli)
		{ }

		public abstract bool IsTargetReached();
	}

	class VerticalMovementState : HelicopterMovementState
	{
		float m_TargetHeight;

		public VerticalMovementState(CMHelicopterActor _Heli, float _TargetHeight)
			: base (_Heli)
		{
			m_TargetHeight = _TargetHeight;
		}

		public override bool IsTargetReached() { return false; }

		bool IsTargetHeightReached()
		{
			return Mathf.Abs(Heli.transform.position.y - m_TargetHeight) < Heli.SnapDistance;
		}

		public override void OnUpdate()
		{
			if (IsTargetHeightReached())
			{
				Heli.transform.position.Set(
						Heli.transform.position.x,
						m_TargetHeight,
						Heli.transform.position.z
					);
				Heli.MovementState = (Heli.transform.position -	Heli.Target).x == 0f ?
					(HelicopterMovementState) new IdleMovementState(Heli) :
					(HelicopterMovementState) new HorizontalMovementState(Heli, Heli.Target.x);
			}
			else
			{
				var delta = m_TargetHeight - Heli.transform.position.y;
				delta = Mathf.Sign(delta) * Mathf.Min(Mathf.Abs(delta), Heli.Speed * Time.deltaTime);
				var movement = new Vector3(0, delta, 0);
				Heli.transform.position = Heli.transform.position + movement;
			}
		}
	}

	class HorizontalMovementState : HelicopterMovementState
	{
		float m_TargetLattitude;

		public HorizontalMovementState(CMHelicopterActor _Heli, float _TargetLattitude)
			: base (_Heli)
		{
			m_TargetLattitude = _TargetLattitude;
		}

		bool IsTargetLattitudeReached()
		{
			return Mathf.Abs(Heli.transform.position.x - m_TargetLattitude) < Heli.SnapDistance;
		}

		public override bool IsTargetReached() { return false; }

		public override void OnUpdate()
		{
			if (IsTargetLattitudeReached())
			{
				Heli.transform.position.Set(
						m_TargetLattitude,
						Heli.transform.position.y,
						Heli.transform.position.x
					);
				Heli.MovementState = (Heli.transform.position -	Heli.Target).y == 0f ?
					(HelicopterMovementState) new IdleMovementState(Heli) :
					(HelicopterMovementState) new VerticalMovementState(Heli, Heli.Target.y);
			}
			else
			{
				var delta = m_TargetLattitude - Heli.transform.position.x;
				delta = Mathf.Sign(delta) * Mathf.Min(Mathf.Abs(delta), Heli.Speed * Time.deltaTime);
				var movement = new Vector3(delta, 0, 0);
				Heli.transform.position = Heli.transform.position + movement;
			}
		}
	}

	class IdleMovementState : HelicopterMovementState
	{
		public IdleMovementState(CMHelicopterActor _Heli)
			: base (_Heli)
		{ }

		public override bool IsTargetReached() { return true; }
	}

	#endregion

	#region Current helicopter intent state

	HelicopterIntentState m_IntentState;
	HelicopterIntentState IntentState
	{
		get { return m_IntentState; }
		set
		{
			if (m_IntentState != null)
			{
				m_IntentState.OnFinish();
			}
			m_IntentState = value;
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

	float	m_UndergroundLevel;
	Vector3 m_Target;
	Vector3 Target
	{
		get { return m_Target; }
		set
		{
			if (m_Target == value)
			{ return; }
			m_Target = value;
			MovementState = new VerticalMovementState(this, m_UndergroundLevel);
		}
	}
	bool IsTargetReached()
	{
		return MovementState.IsTargetReached();
	}

	HelicopterMovementState m_MovementState;
	HelicopterMovementState MovementState
	{
		get { return m_MovementState; }
		set
		{
			if (m_MovementState != null)
			{
				m_MovementState.OnFinish();
			}
			m_MovementState = value;
			if (value != null)
			{
				value.OnStart();
			}
		}
	}

	#endregion

	#region Firing

	public float	FireDistance	= 2f;
	public float	FireTime		= 3f;
	public float	FireRate		= 5f;
	public int		Damage			= 4;

	List<CMHelicopterTarget>	m_Targets = new List<CMHelicopterTarget>();
	event System.Action			m_OnEnemySpotted;
	event System.Action			m_OnEnemyLeft;
	IEnumerator					m_FireCoroutine;

	void OnEnemySpotted(System.Action _Action)
	{
		if (!m_Targets.Any())
		{
			m_OnEnemySpotted += _Action;
		}
		else
		{
			_Action();
		}
	}

	void OnEnemyLeft(System.Action _Action)
	{
		if (m_Targets.Any())
		{
			m_OnEnemyLeft += _Action;
		}
		else
		{
			_Action();
		}
	}

	void StartFire()
	{
		if (m_FireCoroutine == null)
		{
			StartCoroutine(m_FireCoroutine = Fire());
		}
	}

	void StopFire()
	{
		if (m_FireCoroutine != null)
		{
			StopCoroutine(m_FireCoroutine);
			m_FireCoroutine = null;
		}
	}

	void OnTriggerEnter(Collider _Other)
	{
		var target = _Other.gameObject.GetComponent<CMHelicopterTarget>();
		if (target)
		{
			m_Targets.Add(target);
			if (m_OnEnemySpotted != null)
			{
				m_OnEnemySpotted();
				m_OnEnemySpotted = null;
			}
		}
	}
	
	void OnTriggerExit(Collider _Other)
	{
		var target = _Other.gameObject.GetComponent<CMHelicopterTarget>();
		if (target)
		{
			m_Targets.Remove(target);
			if (!m_Targets.Any() && m_OnEnemyLeft != null)
			{
				m_OnEnemyLeft();
				m_OnEnemyLeft = null;
			}
		}
	}

	IEnumerator Fire()
	{
		while(true)
		{
			if (FireRate == 0f)
			{
				Debug.LogError("Fire rate can't be 0!");
				yield break;
			}
			foreach(var t in m_Targets.Where(t => !t.Protected)
					.Select(t => t.gameObject.GetComponent<CMHealth>()))
			{
				t.Health -= Damage;
			}
			yield return new WaitForSeconds(1/FireRate);
		}
	}

	#endregion

	#region Engine methods

	void Start()
	{
		m_UndergroundLevel = transform.position.y;
		MovementState = new IdleMovementState(this);
		IntentState = new CampMoveState(this);
	}

	void Update()
	{
		if (IntentState != null)
		{
			IntentState.OnUpdate();
		}
		if (MovementState != null)
		{
			MovementState.OnUpdate();
		}
	}

	#endregion

	#region Gizmos

	void OnDrawGizmosSelected()
	{
		if (IntentState != null)
		{
			IntentState.OnDrawGizmosSelected();
		}
	}

	#endregion

}
