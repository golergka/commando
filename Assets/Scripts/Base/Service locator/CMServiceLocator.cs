using UnityEngine;
using System.Collections;

public interface ICMServiceProvider
{
	CMCameraManager			CameraManager		{ get; }
	CMInputManager			InputManager		{ get; }
	CMCommandoManager		CommandoManager		{ get; }
	CMGameManager			GameManager			{ get; }
	CMHelicopterManager		HelicopterManager	{ get; }
}

public abstract class CMServiceLocator : MonoBehaviour, ICMServiceProvider
{
	public abstract CMCameraManager		CameraManager		{ get; }
	public abstract CMInputManager		InputManager		{ get; }
	public abstract	CMCommandoManager	CommandoManager		{ get; }
	public abstract CMGameManager		GameManager			{ get; }
	public abstract CMHelicopterManager	HelicopterManager	{ get; }
}
