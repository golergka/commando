using UnityEngine;
using System.Collections;

public interface ICMServiceProvider
{
	CMCameraController CameraController { get; }
}

public abstract class CMServiceLocator : MonoBehaviour, ICMServiceProvider
{
	public abstract CMCameraController CameraController { get; }
}
