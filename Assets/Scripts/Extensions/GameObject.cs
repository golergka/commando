using UnityEngine;

public static class GameObjectExtensions
{
	public static T GetOrAddComponent<T>(this GameObject _GameObject) where T : Component
	{
		if (_GameObject == null)
		{
			Debug.LogError("_GameObject is null!");
			return null;
		}
		var result = _GameObject.GetComponent<T>();
		if (result == null)
		{
			result = _GameObject.AddComponent<T>();
		}
		if (result == null)
		{
			Debug.LogError("Couldn't add the component!");
		}
		return result;
	}
}
