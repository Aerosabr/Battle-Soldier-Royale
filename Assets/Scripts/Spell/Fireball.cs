using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Fireball : Spell
{
	private const int MAX_SIZE = 9;
	private const float MAX_DURATION = 4;

	private void Start()
	{
		hitBox = GetComponent<BoxCollider>();
		StartCoroutine(HandleHitBox());
		StartCoroutine(HandleAttack());
	}

	private void OnDrawGizmos()
	{
		if (hitBox != null)
		{
			Gizmos.color = Color.red;
			// Draw the hitbox as a red wire cube
			Gizmos.DrawWireCube(hitBox.transform.position, hitBox.size);
		}
	}
	public void InitializeFireball(int layer)
	{
		targetLayer = layer;
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


	public override IEnumerator Project(int layer)
	{
		InitializeFireball(layer);
		transparentObject.gameObject.SetActive(true);
		visualObject.gameObject.SetActive(false);
		while (transparentObject.gameObject.activeSelf)
		{
			Vector3 mouse_position = Input.mousePosition;

			float x = mouse_position.x;
			transform.position = new Vector3(mouse_position.x, transform.position.y, transform.position.z);
			yield return null;
		}

		if (visualObject.gameObject.activeSelf)
			yield return true;
		else
			yield return false;
		
	}

	public override bool Placed(bool isPlaced)
	{
		if (isPlaced)
		{
			transparentObject.gameObject.SetActive(false);
			visualObject.gameObject.SetActive(true);
			return true;
		}
		else
		{
			transparentObject.gameObject.SetActive(false);
			visualObject.gameObject.SetActive(false);
			return false;
		}
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
