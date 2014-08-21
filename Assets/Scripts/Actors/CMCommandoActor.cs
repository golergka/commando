using UnityEngine;

public class CMCommandoActor : CMBehavior
{
	public float Speed = 1f;
	public float SpriteSpeed = 5f;

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

	void Start()
	{
		CameraController.gameObject.GetOrAddComponent<CMFollower>().Followee = transform;
		CurrentSprite = 0;
	}

	void Update()
	{
		float movement = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
		transform.position += new Vector3(movement, 0, 0);
		SpriteProgress += Mathf.Abs(movement * SpriteSpeed);
	}
}
