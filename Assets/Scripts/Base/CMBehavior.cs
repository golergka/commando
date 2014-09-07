using UnityEngine;
using System.Collections;

public class CMBehavior : MonoBehaviour, ICMServiceProvider
{
	#region Service locator

	protected CMServiceLocator ServiceLocator { get; private set; }

	protected virtual void Awake()
	{
		ServiceLocator = gameObject.GetComponent<CMServiceLocator>();
		if (ServiceLocator == null)
		{
			ServiceLocator = CMDefaultServiceLocator.Instance;
			if (ServiceLocator == null)
			{
				Debug.LogError("Can't locate default service locator!");
			}
		}
	}

	#endregion

	#region ICMServiceProvider

	CMCameraManager m_CameraManager;

	public CMCameraManager CameraManager
	{ 
		get 
		{
			if (m_CameraManager == null)
			{
				m_CameraManager = ServiceLocator.CameraManager;
			}
			return m_CameraManager; 
		}
	}

	CMInputManager m_InputManager;

	public CMInputManager InputManager
	{
		get
		{
			if (m_InputManager == null)
			{
				m_InputManager = ServiceLocator.InputManager;
			}
			return m_InputManager;
		}
	}

	CMCommandoManager m_CommandoManager;

	public CMCommandoManager CommandoManager
	{
		get
		{
			if (m_CommandoManager == null)
			{
				m_CommandoManager = ServiceLocator.CommandoManager;
			}
			return m_CommandoManager;
		}
	}

	#endregion
}
