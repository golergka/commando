using UnityEngine;

public class CMHealth : CMBehavior
{
	public void InitWithMax(int _MaxHealth)
	{
		MaxHealth = _MaxHealth;
		Health = _MaxHealth;
	}

	public int MaxHealth { get; private set; }
	int m_Health;

	public int Health
	{
		get { return m_Health; }
		set
		{
			if (!IsAlive)
				return;
			m_Health = value;
			m_Health = Mathf.Max(0, m_Health);
			m_Health = Mathf.Min(MaxHealth, m_Health);
			if (m_Health == 0)
			{
				Die();
			}
		}
	}

	public bool IsAlive { get { return Health > 0; } }

	void Die()
	{
		// TODO
	}
}
