using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlManager : MonoBehaviour
{
	private enum Mode
	{
		Idle,
		Command,
		Cast,
		Build,
	}

	public static PlayerControlManager Instance;
	private Mode mode;

	private void Start()
	{
		Instance = this;
	}

	public void CardSelected(CardSO cardSO)
	{
		if (cardSO.cardType == CardSO.CardType.Character)
		{
			PlayerBlue.Instance.SpawnCharacter(cardSO);
			mode = Mode.Command;
		}
		else if (cardSO.cardType == CardSO.CardType.Spell)
		{
			PlayerBlue.Instance.SpawnSpell(cardSO);
			mode = Mode.Cast;
		}
		else if (cardSO.cardType == CardSO.CardType.Building)
		{
			//PlayerBlue.Instance.SpawnBuilding(cardSO);
			mode = Mode.Build;
		}
	}

	public IEnumerator CardConfirmation()
	{
		while(Mouse.current.leftButton.isPressed) //More Held Down Inputs
		{
			yield return null;
		}
		Vector2 mousePosition = Mouse.current.position.ReadValue();
		Debug.Log(mousePosition);
	}
}
