using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	int m_ScreenHeight;
	int m_PixelSize;

	int ScreenHeight
	{
		get { return m_ScreenHeight; }
		set
		{
			if (m_ScreenHeight == value)
				return;

			m_ScreenHeight = value;
			UpdateSize();
		}
	}
	public int PixelSize = 1;

	// Update is called once per frame
	void Update ()
	{
		ScreenHeight = Screen.height;
		if (m_PixelSize != PixelSize)
		{
			m_PixelSize = PixelSize;
			UpdateSize();
		}
	}

	void UpdateSize()
	{
		camera.orthographicSize = ((float) ScreenHeight) / (2 * PixelSize);
	}
}
