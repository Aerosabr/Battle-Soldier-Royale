using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ArrowBarrage : Spell
{
    [SerializeField] private ArrowBarrageSound sound;

    protected override IEnumerator HandleHitBox()
	{
		hitBox.size = new Vector3(cardSO.Size, hitBox.size.y, hitBox.size.z);
        yield return null;
	}

	protected override IEnumerator HandleAttack()
	{
		float elapsedTime = 0f;
		float damageInterval = 1f;
        sound.Attack();
        while (elapsedTime <= cardSO.Duration)
		{
			foreach (IDamageable character in characters)
			{
				if (character is Entity entity && entity.GetCurrentHealth() > 0)
				{
					character.Damaged(cardSO.Attack[cardSO.level-1], CardSO.CardType.Spell);
				}
			}
			yield return new WaitForSeconds(damageInterval);
			elapsedTime += damageInterval;
			Debug.Log(elapsedTime);
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
		if (collision.gameObject.layer == targetLayer && collision.transform.tag != "Base")
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
