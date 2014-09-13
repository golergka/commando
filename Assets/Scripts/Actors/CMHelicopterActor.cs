using UnityEngine;
using System.Linq;
using System.Collections;
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
		public virtual void OnDrawGizmosSelected() {}
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

		public override void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(HelicopterActor.transform.position, HelicopterActor.Target);
		}
	}

	class CampWaitState : HelicopterState
	{
		public CampWaitState(CMHelicopterActor _HelicopterActor)
			: base(_HelicopterActor)
		{ }

		public override void OnUpdate()
		{
			HelicopterActor.OnEnemySpotted(delegate
			{
				HelicopterActor.State = new CampFireState(HelicopterActor);
			});
		}

		public override void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			var boxCollider = HelicopterActor.GetComponent<BoxCollider>();
			Gizmos.DrawWireCube(
					HelicopterActor.transform.TransformPoint(boxCollider.center),
					HelicopterActor.transform.TransformVector(boxCollider.size)
				);
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
			HelicopterActor.StartFire();
			HelicopterActor.OnEnemyLeft(delegate
			{
				HelicopterActor.State = new CampMoveState(HelicopterActor);
			});
		}

		public override void OnFinish()
		{
			HelicopterActor.StopFire();
		}

		public override void OnUpdate()
		{
			if (m_Started + HelicopterActor.FireTime < Time.time)
			{
				HelicopterActor.State = new CampMoveState(HelicopterActor);
			}
		}

		public override void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			var boxCollider = HelicopterActor.GetComponent<BoxCollider>();
			Gizmos.DrawWireCube(
					HelicopterActor.transform.TransformPoint(boxCollider.center),
					HelicopterActor.transform.TransformVector(boxCollider.size)
				);
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
			foreach(var t in m_Targets.Select(t => t.gameObject.GetComponent<CMHealth>()))
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
			var delta = Target - transform.position;
			var distance = Mathf.Min(delta.magnitude, Speed * Time.deltaTime);
			var movement = delta.normalized * distance;
			transform.position = transform.position + movement;
		}
	}

	#endregion

	#region Gizmos

	void OnDrawGizmosSelected()
	{
		if (State != null)
		{
			State.OnDrawGizmosSelected();
		}
	}

	#endregion

}
