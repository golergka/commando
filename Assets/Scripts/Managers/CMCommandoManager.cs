using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CMCommandoManager : CMBehavior
{
	#region Commando list management

	public List<CMCommandoActor> Commandos
	{ get { return m_Commandos.ToList(); } }

	List<CMCommandoActor> m_Commandos = new List<CMCommandoActor>(3);

	public void RegisterCommando(CMCommandoActor _Commando)
	{
		m_Commandos.Add(_Commando);
		m_Commandos.Sort(delegate(CMCommandoActor _X, CMCommandoActor _Y)
		{
			float result = _Y.transform.position.x - _X.transform.position.x;
			if (result < 0)
			{ return -1; }
			else if (result > 0)
			{ return 1; }
			else
			{ return 0; }
		});
	}

	#endregion

	#region Orders

	abstract class Order
	{
		readonly float				m_Position;
		readonly CMCommandoManager	m_CommandoManager;

		List<CMCommandoActor> m_CommandosJumped = new List<CMCommandoActor>();

		public Order(CMCommandoManager _CommandoManager, float _Position)
		{
			m_CommandoManager	= _CommandoManager;
			m_Position			= _Position;
		}

		public bool TryExecute()
		{
			bool everyoneJumped = true;
			foreach(var cm in m_CommandoManager.Commandos)
			{
				if (m_CommandosJumped.Contains(cm))
				{
					continue;
				}
				else if (cm.transform.position.x < m_Position)
				{
					everyoneJumped = false;
					continue;
				}
				else
				{
					ExecuteOn(cm);
					m_CommandosJumped.Add(cm);
				}
			}
			return everyoneJumped;
		}

		protected abstract void ExecuteOn(CMCommandoActor _Actor);
	}

	class JumpOrder : Order
	{
		public JumpOrder(CMCommandoManager _CommandoManager, float _Position)
			: base(_CommandoManager, _Position)
		{}

		protected override void ExecuteOn(CMCommandoActor _Actor)
		{
			_Actor.Jump();
		}
	}

	List<Order> m_Orders = new List<Order>();

	#endregion

	#region Input

	void Jump()
	{
		if (m_Commandos.Count == 0)
		{
			Debug.LogError("Can't jump without any commandos!");
			return;
		}
		m_Orders.Add(new JumpOrder(this, m_Commandos[0].transform.position.x));
	}

	void SwitchProfiles(bool _Right)
	{
		var profiles = m_Commandos.Select(cm => cm.Profile).ToList();
		for(int i = 0; i < m_Commandos.Count; i++)
		{
			int commandoIndex = i + (_Right ? 1 : -1);
			while (commandoIndex >= m_Commandos.Count)
			{
				commandoIndex -= m_Commandos.Count;
			}
			while (commandoIndex < 0)
			{
				commandoIndex += m_Commandos.Count;
			}
			m_Commandos[commandoIndex].Profile = profiles[i];
		}
	}

	#endregion

	#region Engine methods

	void Start()
	{
		// TODO: memory leaks
		InputManager.OnTapDown += Jump;
		InputManager.OnSwipeRight += () => SwitchProfiles(true);
		InputManager.OnSwipeLeft += () => SwitchProfiles(false);
	}

	void Update()
	{
		foreach(var o in m_Orders.ToList())
		{
			if (o.TryExecute())
			{
				m_Orders.Remove(o);
			}
		}
	}

	#endregion

}
