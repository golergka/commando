using UnityEngine;
using System.Collections;

public class CMCameraController : CMBehavior
{
	#region Screen size

	int m_ScreenHeight;
	int m_PixelSizeOnScreen;
	float m_PixelsInUnit;

	int ScreenHeight
	{
		get { return m_ScreenHeight; }
		set
		{
			if (m_ScreenHeight == value)
				return;

			m_ScreenHeight = value;
			RefreshSize();
		}
	}
	
	public int PixelSizeOnScreen = 1;
	public float PixelsInUnit = 1f;

	void RefreshSize()
	{
		camera.orthographicSize = ((float) ScreenHeight) / (2 * PixelSizeOnScreen * m_PixelsInUnit);
	}

	#endregion

	#region MonoBehavior methods

	// Update is called once per frame
	void Update ()
	{
		ScreenHeight = Screen.height;
		if (m_PixelSizeOnScreen != PixelSizeOnScreen)
		{
			m_PixelSizeOnScreen = PixelSizeOnScreen;
			RefreshSize();
		}
		if (m_PixelsInUnit != PixelsInUnit)
		{
			m_PixelsInUnit = PixelsInUnit;
			RefreshSize();
		}
	}

	#endregion

	#region World size

	float WorldHeight
	{ get { return 2 * camera.orthographicSize; } }

	float WorldWidth
	{ get { return 2 * camera.aspect * camera.orthographicSize; } }

	public Bounds WorldBounds
	{
		get
		{
			return new Bounds(
				transform.position,
				new Vector3(
					WorldHeight,
					WorldWidth,
					float.PositiveInfinity
				));
		}
	}

	#endregion
}
