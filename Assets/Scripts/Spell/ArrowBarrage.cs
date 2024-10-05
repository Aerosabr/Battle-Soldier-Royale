using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ArrowBarrage : Spell
{
	public GraphicRaycaster raycaster;

	private const int MAX_SIZE = 9;
	private const float MAX_DURATION = 3;

	private void OnDrawGizmos()
	{
		if (hitBox != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(hitBox.transform.position, hitBox.size);
		}
	}
	public override void InitializeSpell(LayerMask layerMask, SpellCardSO card)
	{
		transparentObject.gameObject.SetActive(false);
		visualObject.gameObject.SetActive(true);
		if (layerMask == 6)
		{
			player = PlayerBlue.Instance;
			targetLayer = 7;
		}
		else
		{
			player = PlayerRed.Instance;
			targetLayer = 6;
		}
		this.damage = card.Attack[card.level - 1];
		hitBox = GetComponent<BoxCollider>();
		raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
		hitBox.enabled = true;
		StartCoroutine(HandleHitBox());
		StartCoroutine(HandleAttack());
	}

	private IEnumerator HandleHitBox()
	{
		hitBox.size = new Vector3(MAX_SIZE, hitBox.size.y, hitBox.size.z);
		yield return null;
	}

	private IEnumerator HandleAttack()
	{
		while(characters.Count == 0)
		{
			yield return null;
		}
		float elapsedTime = 0f;
		float damageInterval = 0.5f;
		while (elapsedTime < MAX_DURATION)
		{
			foreach (Character character in characters)
			{
				if (character.GetCurrentHealth() > 0)
				{
					character.transform.GetComponent<IDamageable>().Damaged(damage);
				}
			}
			yield return new WaitForSeconds(damageInterval);
			elapsedTime += damageInterval;
		}
		yield return null;
	}

	#region Entities in Range Handler
	void OnTriggerEnter(Collider collision)
	{
		if(collision.gameObject.layer == targetLayer)
		{
			Character collidedCharacter = collision.gameObject.GetComponent<Character>();
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
			Character collidedCharacter = collision.gameObject.GetComponent<Character>();
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
