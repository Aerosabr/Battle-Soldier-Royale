using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poisoned : MonoBehaviour
{
	private float poisonDuration = 10f;
	private float damageInterval = 1f;
	private float damagePerTick = 2;

	private float poisonElapsed = 0f;
	private float damageElapsed = 0f;
	private IDamageable damageable;

	void Start()
	{
		damageable = GetComponent<IDamageable>();
		if (damageable == null)
		{
			Debug.LogWarning("No IDamageable component found on " + gameObject.name);
			Destroy(this);
		}
	}

	public void UpdatePoison(float damage, float duration)
	{
		poisonElapsed = 0;
		damagePerTick = damage;
		poisonDuration = duration;
	}

	void Update()
	{
		if (damageable == null)
		{
			return;
		}

		poisonElapsed += Time.deltaTime;
		damageElapsed += Time.deltaTime;

		if (damageElapsed >= damageInterval)
		{
			damageable.Damaged(damagePerTick, CardSO.CardType.Spell);
			damageElapsed = 0f;
		}

		if (poisonElapsed >= poisonDuration)
		{
			Destroy(this);
		}
	}
}
