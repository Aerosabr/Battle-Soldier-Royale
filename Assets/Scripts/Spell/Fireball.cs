using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
	private const int MAX_SIZE = 8;
	private const float MAX_DURATION = 4;

	private BoxCollider hitBox;
	private int targetLayer = 6;
	private int damage;
	private float duration = 0; 

	[SerializeField]
	private List<Character> characters = new List<Character>();

	private void Update()
	{
		HandleHitBox();
		if (duration < MAX_DURATION)
		{
			HandleAttack();
			duration += 1.0f * Time.deltaTime;
		}
		
	}

	private void HandleHitBox()
	{
		
	}

	private void HandleAttack()
	{
		float counter = 0;
		while(counter < duration)
		{
			foreach (Character character in characters)
			{
				if (character.GetCurrentHealth() > 0)
				{
					character.transform.GetComponent<IDamageable>().Damaged(damage);
				}
			}
		}
	}

	public void InitializeFireball(int layer, int damage)
	{
		targetLayer = layer;
		this.damage = damage;
	}

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
}
