using UnityEngine;

public class CMStationaryEnemy : CMCharacterActor
{
	#region Commando detection

	Vector3 ClosestCommando
	{
		get
		{
			Vector3 result = Vector3.zero;
			float resultDistance = float.PositiveInfinity;
			foreach(var cm in GameObject.FindGameObjectsWithTag("Commando"))
			{
				float distance = (cm.transform.position - transform.position).magnitude;
				if (distance < resultDistance)
				{
					result = cm.transform.position;
					resultDistance = distance;
				}
			}
			return result;
		}
	}

	protected override bool DirectedRight
	{
		get
		{
			return ClosestCommando.x > transform.position.x;
		}
	}

	protected override bool LooksBack
	{ get { return false; } }

	public float FireDistance;

	#endregion

	#region Engine methods

	protected override void Update()
	{
		base.Update();
		if ((ClosestCommando - transform.position).magnitude < FireDistance)
		{
			StartFire();
		}
		else
		{
			StopFire();
		}
	}

	#endregion
}
