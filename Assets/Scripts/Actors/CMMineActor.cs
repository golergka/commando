using UnityEngine;
using System.Collections;

public class CMMineActor : CMBehavior
{
	public int				Damage = 5;
	public float			Delay = 2f;
	public AnimationCurve	DistanceFalloff;

	bool m_Triggered = false;

	void OnTriggerEnter(Collider _Other)
	{
		if (m_Triggered)
		{ return; }
		if (_Other.GetComponent<CMCommandoActor>() == null)
		{ return; }
		m_Triggered = true;
		StartCoroutine(Detonate());
	}

	IEnumerator Detonate()
	{
		yield return new WaitForSeconds(Delay);
		foreach(var cm in CommandoManager.Commandos)
		{
			float distance = (transform.position - cm.transform.position).magnitude ;
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
