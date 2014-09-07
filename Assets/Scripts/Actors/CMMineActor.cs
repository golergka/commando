using UnityEngine;
using System.Collections;

public class CMMineActor : CMBehavior
{
	public float			Delay = 2f;

	bool m_Triggered = false;

	void OnTriggerEnter(Collider _Other)
	{
		if (m_Triggered)
		{ return; }
		if (_Other.GetComponent<CMCommandoActor>() == null)
		{ return; }
		m_Triggered = true;
		StartCoroutine(Arm());
	}

	IEnumerator Arm()
	{
		yield return new WaitForSeconds(Delay);
		gameObject.GetOrAddComponent<CMDetonator>().Detonate();
	}
}
