using UnityEngine;
using System.Collections;

public class CMDetonator : CMBehavior
{
	public int				Damage = 5;
	public AnimationCurve	DistanceFalloff;

	public void Detonate()
	{
		foreach(var cm in CommandoManager.Commandos)
		{
			float distance = (transform.position - cm.transform.position).magnitude;
			float damage = DistanceFalloff.Evaluate(distance) * Damage;
			var health = cm.Health;
			if (health != null)
			{
				health.Health -= Mathf.FloorToInt(damage);
			}
		}
		Destroy(this.gameObject);
	}

	void OnDrawGizmosSelected()
	{
		int gizmos = 4;
		float maxDistance = DistanceFalloff.keys[DistanceFalloff.length-1].time;
		for(int i = 0; i <= gizmos; i++)
		{
			float distance = maxDistance * (1 - (((float) i) / (float)(gizmos + 1)));
			float intensity = DistanceFalloff.Evaluate(distance);
			Gizmos.color = Color.Lerp(Color.white, Color.red, intensity);
			Gizmos.DrawWireSphere(transform.position, distance);
		}
	}
}
