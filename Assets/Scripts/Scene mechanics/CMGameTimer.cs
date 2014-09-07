using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CMGameTimer : CMBehavior
{
	string TimeLeftDisplay
	{
		get
		{
			var span = new System.TimeSpan(0, 0, Mathf.FloorToInt(TimeLeft));
			return string.Format(
					"{0}:{1:00}",
					(int)span.TotalMinutes,
					span.Seconds
				);
		}
	}

	public float TimeLeft
	{
		get
		{
			return m_StopTime - Time.time;
		}
	}

	Text m_Text;

	public float GameTime;

	float m_StopTime;

	void Start()
	{
		StartCoroutine(Timeout());
		m_Text = gameObject.GetOrAddComponent<Text>();
	}

	IEnumerator Timeout()
	{
		m_StopTime = Time.time + GameTime;
		yield return new WaitForSeconds(GameTime);
		GameManager.GameOver();
	}

	void Update()
	{
		m_Text.text = TimeLeftDisplay;
	}
}
