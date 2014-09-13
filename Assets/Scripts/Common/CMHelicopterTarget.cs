using UnityEngine;

[RequireComponent(typeof(CMHealth))]
public class CMHelicopterTarget : CMBehavior
{
	public bool Protected
	{
		get
		{
			return ProtectedCounter > 0;
		}
	}

	public int ProtectedCounter { get; set; }
}
