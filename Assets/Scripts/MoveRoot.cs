using UnityEngine;
using System.Collections;

public class MoveRoot : MonoBehaviour
{
	float MOVE_SNAP = 128f;

	// Update is called once per frame
	void Update ()
	{
		if (Mathf.Abs(transform.position.x) > MOVE_SNAP ||
			Mathf.Abs(transform.position.y) > MOVE_SNAP ||
			Mathf.Abs(transform.position.z) > MOVE_SNAP)
		{
			for(int i = 0; i < transform.childCount; i++)
			{
				var tr = transform.GetChild(i);
				tr.position += transform.position;
			}
			transform.position = Vector3.zero;
		}
	}
}
