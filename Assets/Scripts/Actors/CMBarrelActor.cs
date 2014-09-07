using UnityEngine;

public class CMBarrelActor : CMBehavior
{
	protected override void Awake()
	{
		var health = gameObject.GetOrAddComponent<CMHealth>();
		health.OnHealthChange += delegate(float _Delta)
		{
			if (health.Health == 0)
			{
				gameObject.GetOrAddComponent<CMDetonator>().Detonate();
			}
		};
	}
}
