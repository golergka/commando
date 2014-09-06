using UnityEngine;
using System.Collections;

public class CMDefaultServiceLocator : CMServiceLocator
{
	#region Private realization

	static Lazy<CMDefaultServiceLocator> m_Instance = new Lazy<CMDefaultServiceLocator>(delegate
	{
		var go = new GameObject("Default service locator");
		DontDestroyOnLoad(go);
		return go.AddComponent<CMDefaultServiceLocator>();
	});

	CMCameraManager m_CameraManager;

	static Lazy<CMInputManager> m_InputManager = new Lazy<CMInputManager>(delegate
	{
		var go = new GameObject("Input manager");
		DontDestroyOnLoad(go);
		return go.AddComponent<CMInputManager>();
	});

	static Lazy<CMCommandoManager>	m_CommandoManager = new Lazy<CMCommandoManager>(delegate
	{
		var go = new GameObject("Character manager");
		DontDestroyOnLoad(go);
		return go.AddComponent<CMCommandoManager>();
	});

	#endregion

	#region Singleton

	public static CMDefaultServiceLocator Instance
	{ get { return m_Instance.Value; } }

	#endregion

	#region ICMServiceProvider

	public override CMCameraManager CameraManager
	{ 
		get 
		{ 
			if (m_CameraManager == null)
			{
				m_CameraManager = Camera.main.gameObject.GetOrAddComponent<CMCameraManager>();
			}
			return m_CameraManager; 
		} 
	}

	public override CMInputManager	InputManager
	{ get { return m_InputManager.Value; } }

	public override CMCommandoManager CommandoManager
	{ get { return m_CommandoManager.Value; } }

	#endregion
}
