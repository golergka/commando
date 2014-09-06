using UnityEngine;
using System.Collections;

public class CMInputManager : CMBehavior
{
	#region Public interface

	public float Horizontal
	{ get { return Input.GetAxis("Horizontal"); } }

	public float Vertical
	{ get { return Input.GetAxis("Vertical"); } }

	public event System.Action OnTapDown;
	public event System.Action OnSwipeLeft;
	public event System.Action OnSwipeRight;

	#endregion

	#region Engine methods

	void Start()
	{
		var collider = gameObject.GetOrAddComponent<BoxCollider>();
		collider.size = new Vector3(1000,1000,1);
		collider.isTrigger = true;
		transform.parent = CameraManager.transform;
	}

	void OnMouseDown()
	{
		if (OnTapDown != null)
			OnTapDown();
	}

	void Update()
	{
		if (Input.GetKey("left") && OnSwipeLeft != null)
		{
			OnSwipeLeft();
		}
		if (Input.GetKey("right") && OnSwipeRight != null)
		{
			OnSwipeRight();
		}
	}

	#endregion
}
