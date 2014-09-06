using UnityEngine;
using System.Collections.Generic;

public class CMCommandoManager : CMBehavior
{

	List<CMCommandoActor>	m_Commandos = new List<CMCommandoActor>(3);
	List<Vector3>			m_CommandoDelta;

	public void RegisterCommando(CMCommandoActor _Commando)
	{
		m_Commandos.Add(_Commando);
		m_Commandos.Sort(delegate(CMCommandoActor _X, CMCommandoActor _Y)
		{
			float result = _X.transform.position.x - _Y.transform.position.x;
			if (result < 0)
			{ return -1; }
			else if (result > 0)
			{ return 1; }
			else
			{ return 0; }
		});
		m_CommandoDelta = new List<Vector3>();
		m_CommandoDelta.Add(Vector3.zero);
		for(int i = 1; i < m_Commandos.Count; i++)
		{
			m_CommandoDelta.Add(
					m_Commandos[i].transform.position - m_Commandos[0].transform.position
				);
		}
	}

}
