using UnityEngine;

public class CMHouseActor : CMBehavior
{
	void OnTriggerEnter(Collider _Other)
	{
		var target = _Other.GetComponent<CMHelicopterTarget>();
		if (target != null)
		{
			target.ProtectedCounter++;
		}
	}

	void OnTriggerExit(Collider _Other)
	{
		var target = _Other.GetComponent<CMHelicopterTarget>();
		if (target != null)
		{
			target.ProtectedCounter--;
		}
	}
}
