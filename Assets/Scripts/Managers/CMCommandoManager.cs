using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CMCommandoManager : CMBehavior
{
	#region Commando movement configuration

	public float Speed			= 1f;
	public float JumpForce		= 5f;
	public float Gravity		= 9.8f;
	public float GroundSnap 	= 0.1f;
	public float GroundAdjust	= 1f;

	#endregion

	#region Commando list management

	public List<CMCommandoActor> Commandos
	{ get { return m_Commandos.ToList(); } }

	List<CMCommandoActor> m_Commandos = new List<CMCommandoActor>(3);

	public void RegisterCommando(CMCommandoActor _Commando)
	{
		m_Commandos.Add(_Commando);
		_Commando.OnDeath += delegate
		{
			int removedIndex = m_Commandos.IndexOf(_Commando);
			for(int i = removedIndex + 1; i < m_Commandos.Count; i++)
			{
				m_Commandos[i].SwitchWith(_Commando);
			}
			m_Commandos.Remove(_Commando);
			SortCommandos();
			if (m_Commandos.Count == 0)
			{
				GameManager.GameOver();
			}
		};
		SortCommandos();
	}

	void SortCommandos()
	{
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
		if (m_Commandos.Count > 0)
		{
			CameraManager.gameObject.GetOrAddComponent<CMFollower>().Followee = m_Commandos[0].transform;
		}
		for(int i = 0; i < m_Commandos.Count; i++)
		{
			m_Commandos[i].Position = i;
		}
	}

	#endregion

	#region Orders

	abstract class Order
	{
		readonly float				m_Position;
		readonly CMCommandoManager	m_CommandoManager;

		int	m_CommandosJumped = 0;

		public Order(CMCommandoManager _CommandoManager, float _Position)
		{
			m_CommandoManager	= _CommandoManager;
			m_Position			= _Position;
		}

		public bool TryExecute()
		{
			for(int i = m_CommandosJumped; i < m_CommandoManager.Commandos.Count; i++)
			{
				if (m_CommandoManager.Commandos[i].transform.position.x >= m_Position)
				{
					ExecuteOn(m_CommandoManager.Commandos[i]);
					m_CommandosJumped++;
				}
			}
			return m_CommandosJumped == m_CommandoManager.Commandos.Count;
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
			_Actor.Mover.Jump();
		}
	}

	List<Order> m_Orders = new List<Order>();

	#endregion

	#region Input

	public void Jump()
	{
		if (m_Commandos.Count == 0)
		{
			return;
		}
		m_Orders.Add(new JumpOrder(this, m_Commandos[0].transform.position.x));
	}

	void SwitchCommandos(bool _Right)
	{
		for(int i = 0; i < m_Commandos.Count - 1; i++)
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
			m_Commandos[commandoIndex].SwitchWith(m_Commandos[i]);
		}
		SortCommandos();
	}

	#endregion

	#region Engine methods

	void Start()
	{
		// TODO: memory leaks
		InputManager.OnClick += Jump;
		InputManager.OnSwipeRight += () => SwitchCommandos(true);
		InputManager.OnSwipeLeft += () => SwitchCommandos(false);
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
