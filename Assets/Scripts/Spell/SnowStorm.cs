using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SnowStorm : Spell
{
	public GraphicRaycaster raycaster;

	private const int MAX_SIZE = 9;
	private const float MAX_DURATION = 8;

	private void OnDrawGizmos()
	{
		if (hitBox != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(hitBox.transform.position, hitBox.size);
		}
	}
	public override  void InitializeSpell(LayerMask layerMask, SpellCardSO card)
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
		float duration = 0.3f;
		float elapsedTime = 0f;
		float initialSize = hitBox.size.x;

		while (elapsedTime < duration)
		{
			hitBox.size = new Vector3(Mathf.Lerp(initialSize, MAX_SIZE, elapsedTime / duration), hitBox.size.y, hitBox.size.z);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		hitBox.size = new Vector3(MAX_SIZE, hitBox.size.y, hitBox.size.z);
	}

	private IEnumerator HandleAttack()
	{
		float elapsedTime = 0f;
		while (elapsedTime < MAX_DURATION)
		{
			for (int i = 0; i < characters.Count; i++)
			{
				Character character = characters[i];
				if (character != null)
				{
					if (character.GetCurrentHealth() > 0)
					{
						character.transform.GetComponent<IEffectable>().Slowed(damage);
					}
				}
			}
			yield return null;
			elapsedTime += Time.deltaTime;
		}
		foreach (Character character in characters)
		{
			if (character.GetCurrentHealth() > 0)
			{
				character.transform.GetComponent<IEffectable>().UnSlowed(damage);
				yield return null;
			}
		}
		yield return null;
		Destroy(gameObject);
	}

	#region Entities in Range Handler
	void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.layer == targetLayer)
		{
			Character collidedCharacter = collision.gameObject.GetComponent<Character>();
			if (collidedCharacter != null)
			{
				if (!characters.Contains(collidedCharacter))
				{
					characters.Add(collidedCharacter);
					collidedCharacter.GetComponent<IEffectable>().Slowed(damage);
				}
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
					collidedCharacter.GetComponent<IEffectable>().UnSlowed(damage);
				}
			}
		}
	}

	#endregion
}

