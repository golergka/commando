using UnityEngine;
using System.Collections;

public class ParallaxInputTester : MonoBehaviour
{
	ParallaxController m_ParallaxController;

	void Start()
	{
		m_ParallaxController = gameObject.GetComponent<ParallaxController>();
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
		m_ParallaxController.MoveBy(new Vector3(movement, 0, 0));
	}
}
