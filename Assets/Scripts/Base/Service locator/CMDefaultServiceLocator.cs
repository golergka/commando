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

	static Lazy<CMCameraController> m_CameraController = new Lazy<CMCameraController>(delegate
	{
		return Camera.main.gameObject.GetComponent<CMCameraController>()
			?? Camera.main.gameObject.AddComponent<CMCameraController>();
	});

	#endregion

	#region Singleton

	public static CMDefaultServiceLocator Instance
	{ get { return m_Instance.Value; } }

	#endregion

	#region ICMServiceProvider

	public override CMCameraController CameraController
	{ get { return m_CameraController.Value; } }

	#endregion
}
