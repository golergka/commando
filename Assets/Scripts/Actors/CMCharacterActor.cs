using UnityEngine;
using System.Collections;

public abstract class CMCharacterActor : CMBehavior
{
	#region Firing bullets

	protected abstract Vector3	BulletSpawnPosition { get; }
	protected abstract bool		BulletDirectedRight { get; }

	public CMBulletActor	BulletPrefab;
	public float			FireRate = 1f;

	IEnumerator m_FireCoroutine;

	protected void StartFire()
	{
		StartCoroutine(m_FireCoroutine = Fire());
	}

	protected void StopFire()
	{
		if (m_FireCoroutine != null)
		{
			StopCoroutine(m_FireCoroutine);
			m_FireCoroutine = null;
		}
	}

	IEnumerator Fire()
	{
		while(true)
		{
			if (FireRate == 0f)
			{
				Debug.LogError("Fire rate can't be 0!");
				yield break;
			}
			if (BulletPrefab == null)
			{
				Debug.LogError("Can't instantiate null prefab!");
				yield break;
			}
			var bullet = Instantiate(BulletPrefab, BulletSpawnPosition, Quaternion.identity) as CMBulletActor;
			if (bullet != null)
			{
				bullet.Speed = BulletDirectedRight ? bullet.Speed : -bullet.Speed;
			}
			yield return new WaitForSeconds(1/FireRate);
		}
	}

	#endregion
}
