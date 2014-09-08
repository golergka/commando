using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CMCommandoActor))]
public class CMCommandoActorEditor : CMCharacterActorEditor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (!EditorApplication.isPlaying)
		{
			var actor = target as CMCommandoActor;
			GUILayout.Label("Position: " + actor.Position);
			actor.Position = Mathf.FloorToInt(GUILayout.HorizontalSlider(actor.Position, 0f, 2f));
		}
	}
}
