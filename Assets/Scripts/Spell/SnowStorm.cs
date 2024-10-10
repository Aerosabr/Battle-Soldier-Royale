using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SnowStorm : Spell
{
	protected override IEnumerator HandleHitBox()
	{
		float duration = 0.3f;
		float elapsedTime = 0f;
		float initialSize = hitBox.size.x;

		while (elapsedTime < duration)
		{
			hitBox.size = new Vector3(Mathf.Lerp(initialSize, cardSO.Size, elapsedTime / duration), hitBox.size.y, hitBox.size.z);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		hitBox.size = new Vector3(cardSO.Size, hitBox.size.y, hitBox.size.z);
	}

	protected override IEnumerator HandleAttack()
	{
		float elapsedTime = 0f;
		while (elapsedTime < cardSO.Duration)
		{
			for (int i = 0; i < characters.Count; i++)
			{
				IDamageable character = characters[i];
				if (character != null)
				{
					if (character is Entity entity && entity.GetCurrentHealth() > 0)
					{
						entity.GetComponent<IEffectable>().Slowed((int)cardSO.Attack[cardSO.level-1]);
					}
				}
			}
			yield return null;
			elapsedTime += Time.deltaTime;
		}
		foreach (Character character in characters)
		{
			if (character is Entity entity && entity.GetCurrentHealth() > 0)
			{
				entity.GetComponent<IEffectable>().UnSlowed((int)cardSO.Attack[cardSO.level - 1]);
				yield return null;
			}
		}
		yield return null;
		Destroy(gameObject);
	}

	#region Entities in Range Handler
	void OnTriggerEnter(Collider collision)
	{
		Debug.Log(collision.name);
		if (collision.gameObject.layer == targetLayer && collision.transform.tag != "Base")
		{
			IDamageable collidedCharacter = collision.gameObject.GetComponent<IDamageable>();
			if (collidedCharacter != null)
			{
				if (!characters.Contains(collidedCharacter))
				{
					characters.Add(collidedCharacter);
					IEffectable effectedCharacter = collidedCharacter as IEffectable;
					effectedCharacter.Slowed((int)cardSO.Attack[cardSO.level-1]);
				}
			}
		}
	}
	void OnTriggerExit(Collider collision)
	{
		if (collision.gameObject.layer == targetLayer && collision.transform.tag != "Base")
		{
			IDamageable collidedCharacter = collision.gameObject.GetComponent<IDamageable>();
			if (collidedCharacter != null)
			{
				if (characters.Contains(collidedCharacter))
				{
					characters.Remove(collidedCharacter);
					IEffectable effectedCharacter = collidedCharacter as IEffectable;
					effectedCharacter.UnSlowed((int)cardSO.Attack[cardSO.level - 1]);
				}
			}
		}
	}

	#endregion
}

