using UnityEngine;
using System.Collections;

public class CMDefaultServiceLocator : CMServiceLocator
{
	static Lazy<CMDefaultServiceLocator> m_Instance = new Lazy<CMDefaultServiceLocator>(delegate
	{
		var go = new GameObject("Default service locator");
		return go.AddComponent<CMDefaultServiceLocator>();
	});

	public static CMDefaultServiceLocator Instance
	{ get { return m_Instance.Value; } }
}
