using UnityEngine;

public class CMCommandoActor : CMBehavior
{
	#region Public configuration

	public float Speed = 1f;
	public float SpriteSpeed = 5f;

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

	#region Logic properties

	float CurrentMovement
	{ get { return 1f; } }

	#endregion

	#region Engine methods

	void Start()
	{
		CameraManager.gameObject.GetOrAddComponent<CMFollower>().Followee = transform;
		CurrentSprite = 0;
	}

	void Update()
	{
		if (CurrentMovement == 0f)
			return;
		transform.position += new Vector3(CurrentMovement, 0, 0);
		transform.eulerAngles = new Vector3(
				transform.eulerAngles.x,
				CurrentMovement > 0 ? 0f : 180f,
				transform.eulerAngles.z
			);
		SpriteProgress += Mathf.Abs(CurrentMovement * SpriteSpeed);
	}

	#endregion
}
