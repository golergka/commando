using UnityEngine;

public static class GameObjectExtensions
{
	public static T GetOrAddComponent<T>(this GameObject _GameObject) where T : Component
	{
		return _GameObject.GetComponent<T>() ?? _GameObject.AddComponent<T>();
	}
}
