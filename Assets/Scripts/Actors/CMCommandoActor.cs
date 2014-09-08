using UnityEngine;
using System.Collections;

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

	public bool LooksBack
	{ get { return m_Position == 2; } }

	#endregion

	#region Firing bullets

	public Vector3			BulletSpawn;
	public Vector3			BulletSpawnBack;

	protected override Vector3 BulletSpawnPosition
	{
		get
		{
			return (LooksBack ? BulletSpawnBack : BulletSpawn) + transform.position;
		}
	}
	
	protected override bool BulletDirectedRight
	{
		get
		{
			return !LooksBack;
		}
	}

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

	#endregion

	#region Sprites

	public Sprite[] Sprites;
	public Sprite[] SpritesBack;

	protected override Sprite[] CurrentSprites
	{
		get
		{
			return LooksBack ? SpritesBack : Sprites;
		}
	}

	#endregion

	#region Engine methods

	void Start()
	{
		CommandoManager.RegisterCommando(this);
		StartFire();
	}

	#endregion
}
