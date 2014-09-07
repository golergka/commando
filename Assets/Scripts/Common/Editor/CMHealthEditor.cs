using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CMHealth))]
public class CMHealthEditor : Editor
{
	public override void OnInspectorGUI()
	{
		if (EditorApplication.isPlaying)
		{
			var health = target as CMHealth;
			GUILayout.Label("Health: " + health.Health + "/" + health.MaxHealth);
		}
		else
		{
			DrawDefaultInspector();
		}
	}
}
