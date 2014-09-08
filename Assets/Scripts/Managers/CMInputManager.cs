using UnityEngine;
using System.Collections;

public class CMInputManager : CMBehavior
{
	#region Public interface

	public float Horizontal
	{ get { return Input.GetAxis("Horizontal"); } }

	public float Vertical
	{ get { return Input.GetAxis("Vertical"); } }

	public event System.Action OnClick;
	public event System.Action OnHoldStart;
	public event System.Action OnHoldStop;
	public event System.Action OnSwipeLeft;
	public event System.Action OnSwipeRight;

	#endregion

	#region Tap state

	float?	m_ClickStart = null;
	bool	m_Hold = false;

	const float HOLD_TIME = 0.2f;

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
		m_ClickStart = Time.time;
	}

	void OnMouseUp()
	{
		m_ClickStart = null;
		if (m_Hold)
		{
			m_Hold = false;
			if (OnHoldStop != null)
			{
				OnHoldStop();
			}
		}
		else
		{
			if (OnClick != null)
			{
				OnClick();
			}
		}
	}

	void Update()
	{
		if (Input.GetKeyDown("left") && OnSwipeLeft != null)
		{
			OnSwipeLeft();
		}
		if (Input.GetKeyDown("right") && OnSwipeRight != null)
		{
			OnSwipeRight();
		}
		if (!m_Hold && m_ClickStart != null && (Time.time - m_ClickStart) >= HOLD_TIME)
		{
			m_Hold = true;
			if (OnHoldStart != null)
			{
				OnHoldStart();
			}
		}
	}

	#endregion
}
