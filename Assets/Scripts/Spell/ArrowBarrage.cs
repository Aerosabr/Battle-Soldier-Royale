using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ArrowBarrage : Spell
{
	protected override IEnumerator HandleHitBox()
	{
		hitBox.size = new Vector3(cardSO.Size, hitBox.size.y, hitBox.size.z);
		yield return null;
	}

	protected override IEnumerator HandleAttack()
	{
		while(characters.Count == 0)
		{
			yield return null;
		}
		float elapsedTime = 0f;
		float damageInterval = 0.5f;
		while (elapsedTime < cardSO.Duration)
		{
			foreach (IDamageable character in characters)
			{
				if (character is Entity entity && entity.GetCurrentHealth() > 0)
				{
					character.Damaged(cardSO.Attack[cardSO.level-1]);
				}
			}
			yield return new WaitForSeconds(damageInterval);
			elapsedTime += damageInterval;
		}
		yield return null;
		Destroy(gameObject);
	}

	#region Entities in Range Handler
	void OnTriggerEnter(Collider collision)
	{
		Debug.Log(collision.name);
		if (collision.gameObject.layer == targetLayer)
		{
			IDamageable collidedCharacter = collision.gameObject.GetComponent<IDamageable>();
			if (collidedCharacter != null)
			{
				if (!characters.Contains(collidedCharacter))
				{
					characters.Add(collidedCharacter);
					Debug.Log("Added new character to the list.");
				}
			}
			else
			{
				Debug.Log("Collision object does not have a Character component.");
			}
		}
	}

	void OnTriggerExit(Collider collision)
	{
		if (collision.gameObject.layer == targetLayer)
		{
			IDamageable collidedCharacter = collision.gameObject.GetComponent<IDamageable>();
			if (collidedCharacter != null)
			{
				if (characters.Contains(collidedCharacter))
				{
					characters.Remove(collidedCharacter);
					Debug.Log("Remove new character to the list.");
				}
			}
			else
			{
				Debug.Log("Collision object does not have a Character component.");
			}
		}
	}

	#endregion
}
