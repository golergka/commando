using UnityEngine;
using System.Collections;

public abstract class CMCharacterActor : CMBehavior
{
	#region Firing bullets

	public Vector3			BulletSpawn;
	public Vector3			BulletSpawnBack;

	Vector3 BulletSpawnPosition
	{
		get
		{
			return (LooksLeft ? BulletSpawnBack : BulletSpawn) + transform.position;
		}
	}

	protected abstract bool	LooksLeft	{ get; }

	public CMBulletActor	BulletPrefab;
	public float			FireRate = 1f;

	IEnumerator m_FireCoroutine;

	protected void StartFire()
	{
		StartCoroutine(m_FireCoroutine = Fire());
	}

	protected void StopFire()
	{
		if (m_FireCoroutine != null)
		{
			StopCoroutine(m_FireCoroutine);
			m_FireCoroutine = null;
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
				bullet.Speed = !LooksLeft ? bullet.Speed : -bullet.Speed;
			}
			yield return new WaitForSeconds(1/FireRate);
		}
	}

	#endregion

	#region Current sprite

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

	public Sprite[] Sprites;
	public Sprite[] SpritesBack;

	Sprite[] CurrentSprites
	{
		get
		{
			return LooksLeft ? SpritesBack : Sprites;
		}
	}


	private		int		m_CurrentSprite;
	protected	int		CurrentSprite
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

	protected void UpdateSprite()
	{
		if (CurrentSprites.Length == 0)
		{
			Debug.LogError("I don't have sprites!");
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

	#region Components

	public CMLocomotion	Mover	{ get; private set; }
	public CMHealth		Health	{ get; private set; }

	#endregion

	#region Events

	public event System.Action OnDeath;

	#endregion

	#region Engine methods

	protected override void Awake()
	{
		base.Awake();
		CurrentSprite = 0;
		Mover = gameObject.GetOrAddComponent<CMLocomotion>();
		Mover.OnMovement += m => SpriteProgress += Mathf.Abs(m.magnitude * SpriteSpeed);
		Mover.OnGroundedChanged += delegate(bool _Grounded)
		{
			Renderer.sortingLayerID = _Grounded ? 9 : 11;
		};
		Health = gameObject.GetOrAddComponent<CMHealth>();
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

	#region Gizmos

	void OnDrawGizmosSelected()
	{
		Gizmos.DrawIcon(BulletSpawnPosition, "gizmo_star", false);
	}

	#endregion

}
