using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(CMCommandoActor))]
public class CMCommandoActorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		CMCommandoActor actor = target as CMCommandoActor;
		SpriteRenderer renderer = actor.GetComponent<SpriteRenderer>();
		Texture2D texture = renderer.sprite.texture;
		string spriteSheet = AssetDatabase.GetAssetPath(texture);
		actor.Profile.Sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToArray();
		DrawDefaultInspector();
	}
}
