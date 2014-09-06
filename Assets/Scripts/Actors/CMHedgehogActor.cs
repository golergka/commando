using UnityEngine;

public class CMHedgehogActor : CMBehavior
{
	public int Damage = 5;

	bool m_Triggered = false;

	void OnTriggerEnter(Collider _Other)
	{
		if (m_Triggered)
		{ return; }
		if (_Other.GetComponent<CMCommandoActor>() == null)
		{ return; }
		var health = _Other.GetComponent<CMHealth>();
		if (health == null)
		{ return; }
		health.Health -= Damage;
		CommandoManager.Jump();
		m_Triggered = true;
	}
}
