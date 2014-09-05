using UnityEngine;
using System.Collections.Generic;

public class CMParallaxController : CMBehavior
{

	[System.Serializable]
	public class ParallaxLayer
	{
		public Transform	Root;
		public float		Multiplier;
	}

	public List<ParallaxLayer> Layers;

	public void MoveBy(Vector3 _Delta)
	{
		foreach(var l in Layers)
		{
			l.Root.position += _Delta * l.Multiplier;
		}
	}
}
