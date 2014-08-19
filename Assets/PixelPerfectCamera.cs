using UnityEngine;
using System.Collections;

public class PixelPerfectCamera : MonoBehaviour
{
	int m_ScreenHeight;

	int ScreenHeight
	{
		get { return m_ScreenHeight; }
		set
		{
			if (m_ScreenHeight == value)
				return;

			camera.orthographicSize = ((float) value) / 2;
			Debug.Log("Set camera size to " + camera.orthographicSize);
			m_ScreenHeight = value;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		ScreenHeight = Screen.height;
	}
}
