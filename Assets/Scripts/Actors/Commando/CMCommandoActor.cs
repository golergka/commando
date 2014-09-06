using UnityEngine;

public class CMCommandoActor : CMBehavior
{
	#region Public configuration

	public float SpriteSpeed = 5f;

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

	#region Current sprite

	public Sprite[] Sprites;

	int		m_CurrentSprite;
	int		CurrentSprite
	{
		get { return m_CurrentSprite; }
		set
		{
			m_CurrentSprite = value % Sprites.Length;
			if (m_CurrentSprite < 0)
				m_CurrentSprite += Sprites.Length;
			SpriteRenderer renderer = GetComponent<SpriteRenderer>();
			renderer.sprite = Sprites[m_CurrentSprite];
		}
	}

	#endregion

	#region Sprite progress

	float	m_SpriteProgress;
	float	SpriteProgress
	{
		get { return m_SpriteProgress; }
		set
		{
			m_SpriteProgress = value;
			while(m_SpriteProgress > 1f)
			{
				m_SpriteProgress--;
				CurrentSprite++;
			}
			while(m_SpriteProgress < 0f)
			{
				m_SpriteProgress++;
				CurrentSprite--;
			}
		}
	}

	#endregion

	#region Commando components

	public CMCommandoMover Mover { get; private set; }

	#endregion

	#region Engine methods

	protected override void Awake()
	{
		base.Awake();
		CurrentSprite = 0;
		Mover = gameObject.GetOrAddComponent<CMCommandoMover>();
		Mover.OnMovement += m => SpriteProgress += Mathf.Abs(m.magnitude * SpriteSpeed);
	}

	void Start()
	{
		CommandoManager.RegisterCommando(this);
	}

	#endregion
}
