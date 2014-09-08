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

	protected override bool LooksLeft
	{ get { return m_Position == 2; } }

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
			return LooksLeft ? SpritesBack : Sprites;
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
