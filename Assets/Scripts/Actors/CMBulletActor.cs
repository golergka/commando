using UnityEngine;

public class CMBulletActor : CMBehavior
{
	public int		Damage;
	public float	Speed;
	public float	Distance;

	void OnTriggerEnter(Collider _Other)
	{
		var health = _Other.GetComponent<CMHealth>();
		if (health == null)
		{ return; }
		else 
		{
			health.Health -= Damage;
			Destroy(gameObject);
		}
	}

	Vector3 m_SpawnPosition;

	protected override void Awake()
	{
		m_SpawnPosition = transform.position;
	}

	void Update()
	{
		transform.position = new Vector3(
				transform.position.x + Speed * Time.deltaTime,
				transform.position.y,
				transform.position.z
			);
		if ((transform.position - m_SpawnPosition).magnitude >= Distance)
		{
			Destroy(gameObject);
		}
	}
}
