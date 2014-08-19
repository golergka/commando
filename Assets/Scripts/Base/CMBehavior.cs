using UnityEngine;
using System.Collections;

public class CMBehavior : MonoBehaviour
{
	protected CMServiceLocator ServiceLocator { get; private set; }

	void Awake()
	{
		ServiceLocator = gameObject.GetComponent<CMServiceLocator>();
		if (ServiceLocator == null)
		{
			ServiceLocator = CMDefaultServiceLocator.Instance;
		}
	}
}
