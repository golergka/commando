using UnityEngine;

public class CMHealth : CMBehavior
{
	#region Initialization

	public void InitWithMax(int _MaxHealth)
	{
		MaxHealth = _MaxHealth;
		m_Health = _MaxHealth;
	}

	#endregion

	#region Properties

	public int MaxHealth { get; private set; }
	int m_Health;

	public int Health
	{
		get { return m_Health; }
		set
		{
			if (!IsAlive)
				return;
			value = Mathf.Max(0, value);
			value = Mathf.Min(MaxHealth, value);
			float delta = value - m_Health;
			if (delta == 0)
			{ return; }
			m_Health = value;
			if (OnHealthChange != null)
			{
				OnHealthChange(delta);
			}
		}
	}

	public bool IsAlive { get { return Health > 0; } }

	#endregion

	#region Events

	public event System.Action<float> OnHealthChange;

	#endregion

}
