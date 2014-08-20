using UnityEngine;
using System.Collections;

public class CMBehavior : MonoBehaviour, ICMServiceProvider
{
	#region Service locator

	protected CMServiceLocator ServiceLocator { get; private set; }

	void Awake()
	{
		ServiceLocator = gameObject.GetComponent<CMServiceLocator>();
		if (ServiceLocator == null)
		{
			ServiceLocator = CMDefaultServiceLocator.Instance;
		}
	}

	#endregion

	#region ICMServiceProvider

	CMCameraController m_CameraController;

	public CMCameraController CameraController
	{ 
		get 
		{
			if (m_CameraController == null)
			{
				m_CameraController = ServiceLocator.CameraController;
			}
			return m_CameraController; 
		}
	}

	#endregion
}
