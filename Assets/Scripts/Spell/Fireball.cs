using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Fireball : Spell
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
		float delay = 0.85f;
		float elapsedTime = 0f;
		while (elapsedTime < delay)
		{
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		foreach (IDamageable character in characters)
		{
			if (character is Entity entity && entity.GetCurrentHealth() > 0)
			{
				character.Damaged(cardSO.Attack[cardSO.level - 1]);
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
