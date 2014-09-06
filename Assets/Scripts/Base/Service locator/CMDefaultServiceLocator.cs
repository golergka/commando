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

	static Lazy<CMCameraManager> m_CameraManager = new Lazy<CMCameraManager>(delegate
	{
		return Camera.main.gameObject.GetComponent<CMCameraManager>()
			?? Camera.main.gameObject.AddComponent<CMCameraManager>();
	});

	static Lazy<CMInputManager> m_InputManager = new Lazy<CMInputManager>(delegate
	{
		var go = new GameObject("Input manager");
		return go.AddComponent<CMInputManager>();
	});

	static Lazy<CMCommandoManager>	m_CommandoManager = new Lazy<CMCommandoManager>(delegate
	{
		var go = new GameObject("Character manager");
		return go.AddComponent<CMCommandoManager>();
	});

	#endregion

	#region Singleton

	public static CMDefaultServiceLocator Instance
	{ get { return m_Instance.Value; } }

	#endregion

	#region ICMServiceProvider

	public override CMCameraManager CameraManager
	{ get { return m_CameraManager.Value; } }

	public override CMInputManager	InputManager
	{ get { return m_InputManager.Value; } }

	public override CMCommandoManager CommandoManager
	{ get { return m_CommandoManager.Value; } }

	#endregion
}
