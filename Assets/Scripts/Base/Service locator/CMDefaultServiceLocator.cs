using UnityEngine;
using System.Collections;

public class CMDefaultServiceLocator : CMServiceLocator
{
	#region Lazies

	static Lazy<CMDefaultServiceLocator> m_Instance = new Lazy<CMDefaultServiceLocator>(delegate
	{
		var go = new GameObject("Default service locator");
		return go.AddComponent<CMDefaultServiceLocator>();
	});

	static Lazy<CMCameraManager> m_CameraController = new Lazy<CMCameraManager>(delegate
	{
		return Camera.main.gameObject.GetComponent<CMCameraManager>()
			?? Camera.main.gameObject.AddComponent<CMCameraManager>();
	});

	#endregion

	#region Singleton

	public static CMDefaultServiceLocator Instance
	{ get { return m_Instance.Value; } }

	#endregion

	#region ICMServiceProvider

	public override CMCameraManager CameraController
	{ get { return m_CameraController.Value; } }

	#endregion
}
