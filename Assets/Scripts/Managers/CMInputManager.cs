using UnityEngine;
using System.Collections;

public class CMInputManager : CMBehavior
{
	public float Horizontal
	{ get { return Input.GetAxis("Horizontal"); } }

	public float Vertical
	{ get { return Input.GetAxis("Vertical"); } }

	public event System.Action OnTapDown;

	void Start()
	{
		var collider = gameObject.GetOrAddComponent<BoxCollider>();
		collider.size = new Vector3(1000,1000,1000);
		collider.isTrigger = true;
		transform.parent = CameraManager.transform;
	}

	void OnMouseDown()
	{
		if (OnTapDown != null)
			OnTapDown();
	}
}
