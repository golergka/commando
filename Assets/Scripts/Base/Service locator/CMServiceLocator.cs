using UnityEngine;
using System.Collections;

public interface ICMServiceProvider
{
	CMCameraManager CameraController { get; }
}

public abstract class CMServiceLocator : MonoBehaviour, ICMServiceProvider
{
	public abstract CMCameraManager CameraController { get; }
}
