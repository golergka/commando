﻿using UnityEngine;
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

	CMCharacterManager m_CharacterManager;

	public CMCharacterManager CharacterManager
	{
		get
		{
			if (m_CharacterManager == null)
			{
				m_CharacterManager = ServiceLocator.CharacterManager;
			}
			return m_CharacterManager;
		}
	}

	#endregion
}
