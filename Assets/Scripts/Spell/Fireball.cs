using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Fireball : Spell
{
	public GraphicRaycaster raycaster; // Reference to the UI Raycaster (should be set in the inspector)

	private const int MAX_SIZE = 9;
	private const float MAX_DURATION = 4;

	private void OnDrawGizmos()
	{
		if (hitBox != null)
		{
			Gizmos.color = Color.red;
			// Draw the hitbox as a red wire cube
			Gizmos.DrawWireCube(hitBox.transform.position, hitBox.size);
		}
	}
	public void InitializeFireball(int layer, int damage, int cost)
	{
		targetLayer = layer;
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
		float counter = 0;
		while(counter < 1)
		{
			foreach (Character character in characters)
			{
				if (character.GetCurrentHealth() > 0)
				{
					character.transform.GetComponent<IDamageable>().Damaged((int)damage);
					yield return null;
				}
			}
			yield return null;

		}
		yield return null;
	}


	public override IEnumerator Project(int layer, int damage, int cost)
	{
		float cameraDistance = 0.75f;
		InitializeFireball(layer, damage, cost);
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
			PlayerBlue.Instance.SubtractGold(cost);
			transparentObject.gameObject.SetActive(false);
			visualObject.gameObject.SetActive(true);
			hitBox.enabled = true;
			StartCoroutine(HandleHitBox());
			StartCoroutine(HandleAttack());
			Destroy(gameObject, MAX_DURATION);
		}

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
		Debug.Log("Collided");
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
