using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CMHealth))]
[RequireComponent(typeof(CMLocomotion))]
public class CMCommandoActor : CMCharacterActor
{
	#region Position

	int m_Position;

	public int Position
	{
		get { return m_Position; }
		set
		{
			if (value == m_Position)
			{ return; }
			m_Position = value;
			UpdateSprite();
		}
	}

	protected override bool LooksBack
	{ get { return m_Position == 2; } }
	protected override bool DirectedRight
	{ get { return true; } }

	#endregion

	#region Public interface

	public void SwitchWith(CMCommandoActor _Actor)
	{
		// Switching positions
		{
			var temp = _Actor.transform.position;
			_Actor.transform.position = this.transform.position;
			this.transform.position = temp;
		}
		// Switching movement state
		this.Mover.SwitchWith(_Actor.Mover);
	}

	bool m_DefensePosition = false;
	public bool DefensePosition
	{
		get { return m_DefensePosition; }
		set
		{
			if (m_DefensePosition == value) return;
			Mover.Speed = value ? 0f : CommandoManager.Speed;
			FireRate = value ? FireRate * 2 : FireRate / 2;
		}
	}

	#endregion

	#region Engine methods

	void Start()
	{
		CommandoManager.RegisterCommando(this);
		StartFire();
		Mover.JumpForce		= CommandoManager.JumpForce;
		Mover.Gravity		= CommandoManager.Gravity;
		Mover.Speed			= CommandoManager.Speed;
		Mover.GroundSnap	= CommandoManager.GroundSnap;
		Mover.GroundAdjust	= CommandoManager.GroundAdjust;
	}

	#endregion
}
