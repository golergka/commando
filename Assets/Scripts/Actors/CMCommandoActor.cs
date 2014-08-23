using UnityEngine;

public class CMCommandoActor : CMBehavior
{
	#region Public configuration

	public float Speed = 1f;
	public float SpriteSpeed = 5f;
	public float JumpForce = 5f;

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

	void OnCollisionEnter2D(Collision2D _Collision)
	{
		if (_Collision.gameObject.tag == TAG_OBSTACLE)
			m_ObstacleCollisions++;
	}

	void OnCollisionExit2D(Collision2D _Collision)
	{
		if (_Collision.gameObject.tag == TAG_OBSTACLE)
			m_ObstacleCollisions--;
	}

	#endregion

	#region Obstacles

	const string TAG_OBSTACLE	= "Obstacle";
	const string TAG_FLOOR		= "Floor";
	int m_ObstacleCollisions = 0;

	#endregion

	#region Logic properties

	float Direction
	{ get { return m_ObstacleCollisions > 0 ? 0f : 1f; } }

	#endregion

	#region Engine methods

	void Start()
	{
		CameraManager.gameObject.GetOrAddComponent<CMFollower>().Followee = transform;
		CurrentSprite = 0;
		InputManager.OnTapDown += delegate
		{
			Debug.Log("f");
			rigidbody.AddForce(new Vector3(0,JumpForce,0),ForceMode.Impulse);
		};
	}

	void Update()
	{
		float movement = Direction * Speed;
		if (movement == 0f)
			return;
		rigidbody.AddForce(new Vector3(movement - rigidbody.velocity.x, 0, 0), ForceMode.VelocityChange);
		transform.eulerAngles = new Vector3(
				transform.eulerAngles.x,
				movement > 0 ? 0f : 180f,
				transform.eulerAngles.z
			);
		SpriteProgress += Mathf.Abs(movement * Time.deltaTime * SpriteSpeed);
	}

	#endregion
}
