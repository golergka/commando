using UnityEngine;
using System.Collections;

public class CMInputManager : CMBehavior
{
	public float Horizontal
	{ get { return Input.GetAxis("Horizontal"); } }

	public float Vertical
	{ get { return Input.GetAxis("Vertical"); } }

	public event System.Action OnTapDown;
}
