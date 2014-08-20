using UnityEngine;

public class CMCommandoActor : CMBehavior
{
	public float Speed = 1f;

	void Start()
	{
		CameraController.gameObject.GetOrAddComponent<CMFollower>().Followee = transform;
	}

	void Update()
	{
		float movement = Input.GetAxis("Horizontal");
		transform.position += new Vector3(movement, 0, 0) * Speed * Time.deltaTime;
	}
}
