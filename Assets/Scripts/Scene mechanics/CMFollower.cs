using UnityEngine;
using System.Collections;

public class CMFollower : CMBehavior
{
	public Transform Followee { get; set; }

	public float FollowSpeed = 1f;
	public float FollowSnap = 0.02f;

	void Update()
	{
		if (Followee == null)
			return;

		var target = new Vector3(
				Followee.position.x,
				Followee.position.y,
				transform.position.z
			);
		var delta = target - transform.position;
		if (delta.magnitude < FollowSnap)
		{
			transform.position = target;
		}
		else
		{
			float multiplier = Mathf.Min(1f, FollowSpeed * Time.deltaTime);
			transform.position += delta * multiplier;
		}
	}
}
