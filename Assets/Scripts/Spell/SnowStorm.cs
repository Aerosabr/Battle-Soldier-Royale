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
	public void InitializeSnowStorm(LayerMask layerMask, int damage, int cost)
	{
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
		this.damage = damage;
		this.cost = cost;
		hitBox = GetComponent<BoxCollider>();
		raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
		hitBox.enabled = false;
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
		while (characters.Count == 0)
		{
			yield return null;
		}
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


	public override IEnumerator Project(LayerMask layerMask, int damage, int cost)
	{
		float cameraDistance = 0.75f;
		InitializeSnowStorm(layerMask, damage, cost);
		transparentObject.gameObject.SetActive(true);
		visualObject.gameObject.SetActive(false);
		while (Mouse.current.leftButton.isPressed)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				Vector3 worldPosition = hit.point;
				transform.position = new Vector3(worldPosition.x, transform.position.y, cameraDistance);
			}
			yield return null;
		}

		if (IsMouseOverUI())
		{
			transparentObject.gameObject.SetActive(false);
			visualObject.gameObject.SetActive(false);
			Destroy(gameObject);
		}
		else
		{
			player.SubtractGold(cost);
			transparentObject.gameObject.SetActive(false);
			visualObject.gameObject.SetActive(true);
			hitBox.enabled = true;
			StartCoroutine(HandleHitBox());
			StartCoroutine(HandleAttack());
		}
		PlayerControlManager.Instance.CardHandled();

	}

	private bool IsMouseOverUI()
	{
		Vector3[] corners = CharacterBarUI.Instance.GetCancelArea();
		if (corners == null)
		{
			return false;
		}
		Vector3 mousePosition = Input.mousePosition;
		if (mousePosition.x >= corners[0].x && mousePosition.x <= corners[2].x && mousePosition.y >= corners[0].y && mousePosition.y <= corners[2].y)
		{
			return true;
		}

		return false;
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

