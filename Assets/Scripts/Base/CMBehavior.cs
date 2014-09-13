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

	public CMCameraManager CameraManager
	{ get { return ServiceLocator.CameraManager; } }

	public CMInputManager InputManager
	{ get { return ServiceLocator.InputManager; } }

	public CMCommandoManager CommandoManager
	{ get { return ServiceLocator.CommandoManager; } }

	public CMGameManager GameManager
	{ get { return ServiceLocator.GameManager; } }

	public CMHelicopterManager HelicopterManager
	{ get { return ServiceLocator.HelicopterManager; } }

	#endregion
}
