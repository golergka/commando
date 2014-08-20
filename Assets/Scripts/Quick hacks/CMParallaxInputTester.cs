using UnityEngine;
using System.Collections;

public class CMParallaxInputTester : CMBehavior
{
	CMParallaxController m_ParallaxController;

	public float Speed;

	void Start()
	{
		m_ParallaxController = gameObject.GetComponent<CMParallaxController>();
		if (m_ParallaxController == null)
		{
			Debug.LogError("Can't find parallax controller!");
			enabled = false;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		float movement = Input.GetAxis("Horizontal");
		m_ParallaxController.MoveBy(new Vector3(movement, 0, 0) * Speed * Time.deltaTime);
	}
}
