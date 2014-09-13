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

	#region Current helicopter state

	HelicopterIntentState m_State;
	HelicopterIntentState IntentState
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
		IntentState = new CampMoveState(this);
	}

	void Update()
	{
		if (IntentState != null)
		{
			IntentState.OnUpdate();
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
		if (IntentState != null)
		{
			IntentState.OnDrawGizmosSelected();
		}
	}

	#endregion

}
