using UnityEngine;

public class CMBulletActor : CMBehavior
{
	public int		Damage;
	public float	Speed;

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

	void Update()
	{
		transform.position = new Vector3(
				transform.position.x + Speed * Time.deltaTime,
				transform.position.y,
				transform.position.z
			);
	}
}
