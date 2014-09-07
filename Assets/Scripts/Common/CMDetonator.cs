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
}
