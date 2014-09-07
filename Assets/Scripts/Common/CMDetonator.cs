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
			var health = cm.Health;
			if (health != null)
			{
				float distance = (transform.position - cm.transform.position).magnitude;
				health.Health -= DamageAtDistance(distance);
			}
		}
		Destroy(this.gameObject);
	}

	int DamageAtDistance(float _Distance)
	{
		float damage = DistanceFalloff.Evaluate(_Distance) * Damage;
		return Mathf.FloorToInt(damage);
	}

	void OnDrawGizmosSelected()
	{
		int gizmos = 4;
		float maxDistance = DistanceFalloff.keys[DistanceFalloff.length-1].time;
		for(int i = 0; i <= gizmos; i++)
		{
			float distance = maxDistance * (1 - (((float) i) / (float)(gizmos + 1)));
			float intensity = ((float) DamageAtDistance(distance))/Damage;
			Gizmos.color = intensity > 0 ? Color.Lerp(Color.white, Color.red, intensity) : Color.green;
			Gizmos.DrawWireSphere(transform.position, distance);
		}
	}
}
