using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CMHelicopterManager : CMBehavior
{
	#region Entry types

	[System.Serializable]
	public class FlyBy
	{
		public List<GameObject>		Points;
		public void OnDrawGizmos()
		{
			for(int i = 0; i < Points.Count - 1; i++)
			{
				Gizmos.color = Color.white;
				Gizmos.DrawLine(Points[i].transform.position, Points[i+1].transform.position);
			}
			if (Points.Count > 1)
			{
				Gizmos.DrawIcon(Points[0].transform.position, "gizmo_play");
				Gizmos.DrawIcon(Points[Points.Count-1].transform.position, "gizmo_stop");
			}
		}
	}

	#endregion

	#region Public configuration

	public List<FlyBy>				FlyBys;
	public List<GameObject>			CampPoints;

	#endregion

	#region Gizmos

	void OnDrawGizmos()
	{
		foreach(var fb in FlyBys)
		{
			fb.OnDrawGizmos();
		}
		foreach(var cp in CampPoints)
		{
			Gizmos.DrawIcon(cp.transform.position, "gizmo_helicopter");
		}
	}

	#endregion

	#region Public interface

	public GameObject NextCampPoint()
	{
		float minimumX = float.NegativeInfinity;
		foreach(var cm in CommandoManager.Commandos)
		{
			if (cm.transform.position.x > minimumX)
			{
				minimumX = cm.transform.position.x;
			}
		}

		if (CampPoints == null)
		{
			Debug.LogError("CampPoints cant be null!");
			return null;
		}

		var toTheRight = CampPoints.Where(cp => cp.transform.position.x > minimumX);

		return toTheRight.Any() ?
			toTheRight.Aggregate((_Best, _Candidate) =>
					_Best.transform.position.x < _Candidate.transform.position.x ?
					_Best : _Candidate
				) : null;
	}

	#endregion
}
