using UnityEngine;

public class CMMainMenuActor : CMBehavior
{
	public void Play()
	{
		Application.LoadLevel("mission");
	}
}
