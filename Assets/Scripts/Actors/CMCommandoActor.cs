using UnityEngine;

public class CMCommandoActor : CMBehavior
{
	#region Public configuration

	public float Speed = 1f;
	public float SpriteSpeed = 5f;
	public float JumpForce = 5f;
	public float Gravity = 9.8f;

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

	#region Collisions

	CapsuleCollider	m_CapsuleCollider;

	#endregion

	#region Obstacles

	const int LAYER_PLATFORM			= 8;

	#endregion

	#region Logic properties

	float Direction
	{ get { return 1f; } }

	float m_VerticalImpulse = 0f;

	#endregion

	#region Engine methods

	void Start()
	{
		CameraManager.gameObject.GetOrAddComponent<CMFollower>().Followee = transform;
		CurrentSprite = 0;
		InputManager.OnTapDown += delegate
		{
			if (Mathf.Approximately(m_VerticalImpulse, 0f))
			{
				m_VerticalImpulse = JumpForce;
			}
		};
		m_CapsuleCollider = gameObject.GetOrAddComponent<CapsuleCollider>();
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
			RaycastHit hit;
			if (rigidbody.SweepTest(new Vector3(0, -movement.y, 0), out hit, movement.y))
			{
				movement.y = 0f;
				m_VerticalImpulse = 0f;
			}
		}
		// Assigning destination
		var destination = transform.position + movement;
		// Moving the actor horizontaly
		transform.position = destination;
		//rigidbody.AddForce(new Vector3(movement - rigidbody.velocity.x, 0, 0), ForceMode.VelocityChange);
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
