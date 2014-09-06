using UnityEngine;
using System.Collections;

public interface ICMServiceProvider
{
	CMCameraManager		CameraManager		{ get; }
	CMInputManager		InputManager		{ get; }
	CMCharacterManager	CharacterManager	{ get; }
}

public abstract class CMServiceLocator : MonoBehaviour, ICMServiceProvider
{
	public abstract CMCameraManager		CameraManager		{ get; }
	public abstract CMInputManager		InputManager		{ get; }
	public abstract	CMCharacterManager	CharacterManager	{ get; }
}
