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
	}

	public static PlayerControlManager Instance;
	private Mode mode;

	private void Start()
	{
		Instance = this;
	}

	public int CheckMode()
	{
		if (mode == Mode.Idle)
			return 0;
		else if(mode == Mode.Command)
			return 1;
		else if(mode == Mode.Cast)
			return 2;
		return 3;
	}

	public void CardSelected(CardSO cardSO)
	{
		if (cardSO.cardType == CardSO.CardType.Character)
		{
			PlayerBlue.Instance.ProjectCard(cardSO);
			mode = Mode.Cast;
		}
		else if(cardSO.cardType == CardSO.CardType.Worker)
		{
			PlayerBlue.Instance.ProjectCard(cardSO);
			mode = Mode.Cast;
		}
		else if (cardSO.cardType == CardSO.CardType.Spell)
		{
			PlayerBlue.Instance.ProjectCard(cardSO);
			mode = Mode.Cast;
		}
		else if (cardSO.cardType == CardSO.CardType.Building)
		{
			PlayerBlue.Instance.ProjectCard(cardSO);
			mode = Mode.Cast;
		}
	}
	
	public void CardHandled()
	{
		mode = Mode.Command;
	}
}
