using UnityEngine;

public class CMCommandoActor : CMBehavior
{
	#region Public configuration

	public float Speed = 1f;
	public float SpriteSpeed = 5f;
	public float JumpForce = 5f;
	public float Gravity = 9.8f;
	public float GroundSnap = 0.1f;
	public float GroundAdjust = 1f;

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

	#region Movement state

	public float GroundHeight { get; set; }

	float Direction
	{ get { return 1f; } }

	float	m_VerticalImpulse = 0f;
	bool	m_DoubleJumped = false;

	void HitGround()
	{
		m_VerticalImpulse = 0f;
		m_DoubleJumped = false;
	}

	#endregion

	#region Engine methods

	void Start()
	{
		CameraManager.gameObject.GetOrAddComponent<CMFollower>().Followee = transform;
		CurrentSprite = 0;
		InputManager.OnTapDown += delegate
		{
			bool grounded = Mathf.Approximately(m_VerticalImpulse, 0f);
			if (grounded || !m_DoubleJumped)
			{
				m_VerticalImpulse = JumpForce;
				if (!grounded)
				{
					m_DoubleJumped = true;
				}
			}
		};
		GroundHeight = transform.position.y;
		CommandoManager.RegisterCommando(this);
	}

	void Update()
	{
		m_VerticalImpulse -= Gravity * Time.deltaTime;
		var movement = new Vector3(
				Direction * Speed * Time.deltaTime,
				m_VerticalImpulse * Time.deltaTime,
				0
			);
		// Clamping horizontal movement with horizontal cast
		{
			RaycastHit hit;
			if (rigidbody.SweepTest(new Vector3(movement.x, 0, 0), out hit, movement.x))
			{
				// TODO
			}
		}
		// Clamping vertical movement with vertical cast
		if (movement.y < 0)
		{
			// Hitting obstacles and platforms
			RaycastHit hit;
			if (rigidbody.SweepTest(new Vector3(0, -movement.y, 0), out hit, movement.y))
			{
				movement.y = 0f;
				HitGround();
			}
			// Hitting the ground
			if (transform.position.y <= GroundHeight)
			{
				if ((GroundHeight - transform.position.y) < GroundSnap)
				{
					transform.position = new Vector3(
							transform.position.x,
							GroundHeight,
							transform.position.z
						);
				}
				movement.y = (GroundHeight - transform.position.y) * Time.deltaTime * GroundAdjust;
				HitGround();
			}
		}
		// Moving the actor
		transform.position = transform.position + movement;
		// Rotating the transform
		transform.eulerAngles = new Vector3(
				transform.eulerAngles.x,
				movement.x >= 0 ? 0f : 180f,
				transform.eulerAngles.z
			);
		// Changing the sprite
		SpriteProgress += Mathf.Abs(movement.magnitude * SpriteSpeed);
	}

	#endregion
}
