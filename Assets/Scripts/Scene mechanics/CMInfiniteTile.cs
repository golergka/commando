using UnityEngine;
using System.Collections.Generic;

public class CMInfiniteTile : CMBehavior
{
	#region Designer configuration

	public List<CMInfiniteTile> PossibleNeighbours;

	public Vector3 LeftNeighbourPosition;
	public Vector3 RightNeighbourPosition;

	#endregion

	#region Fields

	CMInfiniteTile m_LeftNeighbour;
	CMInfiniteTile m_RightNeighbour;

	#endregion

	#region Logic properties

	bool ShouldHaveNeighbours
	{
		get
		{
			return CameraController.WorldBounds.Contains(transform.position);
		}
	}

	#endregion
	
	#region MonoBehavior methods

	void Update()
	{
		if (ShouldHaveNeighbours)
		{
			if (m_LeftNeighbour == null)
			{
				m_LeftNeighbour = Instantiate(
						RandomNeighbourPrefab(),
					   	transform.position + LeftNeighbourPosition,
					   	Quaternion.identity
					) as CMInfiniteTile;
				m_LeftNeighbour.transform.parent = transform.parent;
				m_LeftNeighbour.m_RightNeighbour = this;
			}
			if (m_RightNeighbour == null)
			{
				m_RightNeighbour = Instantiate(
						RandomNeighbourPrefab(),
						transform.position + RightNeighbourPosition,
						Quaternion.identity
					) as CMInfiniteTile;
				m_RightNeighbour.transform.parent = transform.parent;
				m_RightNeighbour.m_LeftNeighbour = this;
			}
		}
	}

	#endregion

	#region Helper functions

	CMInfiniteTile RandomNeighbourPrefab()
	{
		return PossibleNeighbours[Mathf.FloorToInt(Random.Range(0,PossibleNeighbours.Count))];
	}

	#endregion
}
