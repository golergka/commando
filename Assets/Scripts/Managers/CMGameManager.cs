using UnityEngine;

public class CMGameManager : CMBehavior
{
	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}

	public void Play()
	{
		Application.LoadLevel("mission");
	}

	public void GameOver()
	{
		Application.LoadLevel("menu");
	}
}
