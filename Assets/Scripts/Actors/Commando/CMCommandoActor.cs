using UnityEngine;
using System.Collections;

public class CMCommandoActor : CMBehavior
{
	#region Public configuration

	public int		MaxHealth	= 100;

	#endregion

	#region Gizmos

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawIcon(BulletSpawnPosition, "gizmo_star", false);
	}

	#endregion

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

	public CMBulletActor	BulletPrefab;
	public float			FireRate = 1f;

	IEnumerator m_FireCoroutine;

	void StartFire()
	{
		StartCoroutine(m_FireCoroutine = Fire());
	}

	void StopFire()
	{
		if (m_FireCoroutine != null)
		{
			StopCoroutine(m_FireCoroutine);
			m_FireCoroutine = null;
		}
	}

	Vector3 BulletSpawnPosition
	{
		get
		{
			return (LooksBack ? BulletSpawnBack : BulletSpawn) + transform.position;
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
			if (BulletPrefab == null)
			{
				Debug.LogError("Can't instantiate null prefab!");
				yield break;
			}
			var bullet = Instantiate(BulletPrefab, BulletSpawnPosition, Quaternion.identity) as CMBulletActor;
			if (bullet != null)
			{
				bullet.Speed = LooksBack ? -bullet.Speed : bullet.Speed;
			}
			yield return new WaitForSeconds(1/FireRate);
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

	#region Current sprite

	public Sprite[] Sprites;
	public Sprite[] SpritesBack;

	Sprite[] CurrentSprites
	{
		get
		{
			return LooksBack ? SpritesBack : Sprites;
		}
	}

	int		m_CurrentSprite;
	int		CurrentSprite
	{
		get { return m_CurrentSprite; }
		set
		{
			if (m_CurrentSprite == value)
			{ return; }
			m_CurrentSprite = value;
			UpdateSprite();
		}
	}

	void UpdateSprite()
	{
		if (CurrentSprites.Length == 0)
		{
			Debug.LogError("I don't have sprites for current state: " + (LooksBack ? "back" : "forward"));
			return;
		}
		m_CurrentSprite = m_CurrentSprite % CurrentSprites.Length;
		if (m_CurrentSprite < 0)
			m_CurrentSprite += CurrentSprites.Length;
		Renderer.sprite = CurrentSprites[CurrentSprite];
	}

	#endregion

	#region Sprite progress

	public float SpriteSpeed = 5f;

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

	#region Components

	public CMCommandoMover	Mover { get; private set; }
	public CMHealth			Health { get; private set; }

	SpriteRenderer			m_Renderer;
	SpriteRenderer			Renderer
	{
		get
		{
			if (m_Renderer == null)
			{
				m_Renderer = gameObject.GetOrAddComponent<SpriteRenderer>();
			}
			return m_Renderer;
		}
	}

	#endregion

	#region Blinking state

	public float	BlinkLength = 0.1f;
	public int		BlinkCount	= 4;

	float m_BlinkStartTime = float.NegativeInfinity;
	float BlinkStop { get { return m_BlinkStartTime + BlinkLength * BlinkCount; } }
	float Alpha
	{
		get
		{
			return Renderer.color.a;
		}

		set
		{
			var color = Renderer.color;
			color.a = value;
			Renderer.color = color;
		}
	}

	#endregion

	#region Events

	public event System.Action OnDeath;

	#endregion

	#region Engine methods

	protected override void Awake()
	{
		base.Awake();
		CurrentSprite = 0;
		Mover = gameObject.GetOrAddComponent<CMCommandoMover>();
		Mover.OnMovement += m => SpriteProgress += Mathf.Abs(m.magnitude * SpriteSpeed);
		Health = gameObject.GetOrAddComponent<CMHealth>();
		Health.InitWithMax(MaxHealth);
		Health.OnHealthChange += delegate(float _Delta)
		{
			if (_Delta < 0)
			{
				m_BlinkStartTime = Time.time;
			}
			if (Health.Health == 0)
			{
				if (OnDeath != null)
				{
					OnDeath();
				}
				Mover.DeathJump();
			}
		};
	}

	void Start()
	{
		CommandoManager.RegisterCommando(this);
		StartFire();
	}

	void Update()
	{
		if (Time.time < BlinkStop)
		{
			float phase = (Time.time - m_BlinkStartTime) / BlinkLength;
			Alpha = Mathf.Sin(Mathf.PI * (2 * phase - 0.5f)) * 0.5f + 0.5f;
		}
		else
		{
			Alpha = 1f;
		}
	}

	#endregion
}
