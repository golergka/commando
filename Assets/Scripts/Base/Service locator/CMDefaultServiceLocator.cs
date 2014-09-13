using UnityEngine;
using System.Collections;

public class CMDefaultServiceLocator : CMServiceLocator
{
	#region Private realization

	static CMDefaultServiceLocator m_Instance;
	CMCameraManager			m_CameraManager;
	CMInputManager			m_InputManager;
	CMCommandoManager		m_CommandoManager;
	CMGameManager			m_GameManager;
	CMHelicopterManager		m_HelicopterManager;

	#endregion

	#region Singleton

	public static CMDefaultServiceLocator Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = LoadOrCreateManager<CMDefaultServiceLocator>("Service locator");
			}
			return m_Instance;
		}
	}

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
	{ 
		get 
		{ 
			if (m_InputManager == null)
			{
				m_InputManager = LoadOrCreateManager<CMInputManager>("Input manager");
			}
			return m_InputManager;
		} 
	}

	public override CMCommandoManager CommandoManager
	{
		get
		{
			if (m_CommandoManager == null)
			{
				m_CommandoManager = LoadOrCreateManager<CMCommandoManager>("Commando manager");
			}
			return m_CommandoManager;
		}
	}

	public override CMGameManager GameManager
	{
		get
		{
			if (m_GameManager == null)
			{
				m_GameManager = LoadOrCreateManager<CMGameManager>("Game manager");
			}
			return m_GameManager;
		}
	}

	public override CMHelicopterManager HelicopterManager
	{
		get
		{
			if (m_HelicopterManager == null)
			{
				m_HelicopterManager = LoadOrCreateManager<CMHelicopterManager>("Helicopter manager");
			}
			return m_HelicopterManager;
		}
	}

	#endregion

	#region Service methods

	static T LoadOrCreateManager<T>(string _Name) where T : Component
	{
		var go = GameObject.Find(_Name);
		if (go != null)
		{
			return go.GetOrAddComponent<T>();
		}
		else
		{
			var result = new GameObject(_Name).AddComponent<T>();
			DontDestroyOnLoad(result);
			DontDestroyOnLoad(result.gameObject);
			return result;
		}
	}

	#endregion
}
