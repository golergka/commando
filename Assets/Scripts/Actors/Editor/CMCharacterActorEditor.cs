using UnityEngine;
using UnityEditor;
using System.Linq;

public abstract class CMCharacterActorEditor : Editor
{
	const string BACK_SUFFIX = "_back";

	public override void OnInspectorGUI()
	{
		if (!EditorApplication.isPlaying)
		{
			var actor = target as CMCharacterActor;
			if (GUILayout.Button("Auto-assign sprites"))
			{
				SpriteRenderer renderer = actor.GetComponent<SpriteRenderer>();
				Texture2D texture = renderer.sprite.texture;
				string currentSpriteSheet = AssetDatabase.GetAssetPath(texture);
				int foundBack = currentSpriteSheet.IndexOf(BACK_SUFFIX);
				string spriteSheet;
				if (foundBack == -1)
				{
					spriteSheet = currentSpriteSheet;
				}
				else
				{
					spriteSheet = currentSpriteSheet.Substring(0,foundBack) + ".png";
				}
				string spriteSheetBack = spriteSheet.Split('.')[0] + "_back.png";
				Debug.Log("Loading sprites from:\n" + spriteSheet + "\n" + spriteSheetBack);
				actor.Sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToArray();
				actor.SpritesBack = AssetDatabase.LoadAllAssetsAtPath(spriteSheetBack).OfType<Sprite>().ToArray();
			}
		}
		DrawDefaultInspector();
	}
}
